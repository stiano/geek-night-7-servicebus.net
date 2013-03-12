namespace Server
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Meldinger;
    using NServiceBus;
    using NServiceBus.Saga;

    public class TekstSaga : Saga<SplittOrdSagaData>,
                             IAmStartedByMessages<TekstSagaStartMelding>,
                             IHandleMessages<TekstSagaFragment>,
                             IHandleTimeouts<TekstSagaTimeoutMelding>
    {
        private readonly TimeSpan timeOutVerdi = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Knytter saga-meldingene sammen.
        /// </summary>
        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<TekstSagaStartMelding>(s => s.SagaId, m => m.SagaId);
            ConfigureMapping<TekstSagaFragment>(s => s.SagaId, m => m.SagaId);
            ConfigureMapping<TekstSagaTimeoutMelding>(s => s.SagaId, m => m.SagaId);
        }

        /// <summary>
        /// Definerer starten på en saga.
        /// </summary>
        public void Handle(TekstSagaStartMelding melding)
        {
            // Property markert som Unique må settes manuelt.
            Data.SagaId = melding.SagaId;

            // Den originale hashen spares.
            Data.OriginalTekstHash = melding.TekstHash;

            // Neste melding må inntreffe innen gitt tidsfrist.
            RequestUtcTimeout(timeOutVerdi, new TekstSagaTimeoutMelding { SagaId = Data.SagaId });
        }

        /// <summary>
        /// Håndterer mottatte tekstfragmenter.
        /// </summary>
        public void Handle(TekstSagaFragment melding)
        {
            // Persisterer til ordliste.
            Data.OrdListe.Add(melding.FragmentId, melding.Fragment);

            // Neste melding må inntreffe innen gitt tidsfrist.
            RequestUtcTimeout(timeOutVerdi, new TekstSagaTimeoutMelding { SagaId = Data.SagaId });
        }

        public void Timeout(TekstSagaTimeoutMelding state)
        {
            var mottattTekst = Data.GenererTekstBasertPåOrdliste();

            Console.Out.WriteLine(
                "Tekst med id '{0}' og innhold '{1}' ble prosessert på server av tråd {2}.", 
                Data.SagaId, 
                mottattTekst, 
                Thread.CurrentThread.ManagedThreadId);

            var mottattTekstHash = mottattTekst.GetHashCode();

            // Sender verifikasjon tilbake til avsender.
            ReplyToOriginator(new TekstSagaResultat
            {
                Ok = mottattTekstHash == Data.OriginalTekstHash,
                SagaId = Data.SagaId
            });    

            // Avslutter sagaen. 
            MarkAsComplete();
        }
    }
}
