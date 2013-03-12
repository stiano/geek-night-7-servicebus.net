namespace Klient
{
    using System;
    using Meldinger;
    using NServiceBus;
    using System.Linq;

    public class EksempelPåSaga
    {
        private static void StartSaga(IBus bus, int iterasjoner = 1)
        {
            const string Tekst = "en veldig lang tekst skal dette bli og det er ingen grenser for hvor frekk den kan være";
            var tekstHash = Tekst.GetHashCode();
            var ordListe = Tekst.Split(new[] { ' ' });

            for (var i = 0; i < iterasjoner; i++)
            {
                var sagaId = Guid.NewGuid();
                Console.Out.WriteLine(@"KLIENT: Starter saga med id '{0}'.", sagaId);

                bus.Send("Server", new TekstSagaStartMelding
                {
                    SagaId = sagaId,
                    TekstHash = tekstHash
                });

                for (var ordTeller = 0; ordTeller < ordListe.Count(); ordTeller++)
                {
                    bus.Send("Server", new TekstSagaFragment
                    {
                        SagaId = sagaId,
                        FragmentId = ordTeller,
                        Fragment = ordListe[ordTeller]
                    });
                }
            }
        } 
    }
}