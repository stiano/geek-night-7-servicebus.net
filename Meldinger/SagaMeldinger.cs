namespace Meldinger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NServiceBus;
    using NServiceBus.Saga;

    /// <summary>
    /// Entitet som definerer tilstanden til en gitt saga.
    /// </summary>
    public class SplittOrdSagaData : ISagaEntity
    {
        public SplittOrdSagaData()
        {
            OrdListe = new Dictionary<long, string>();
        }

        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }

        /// <summary>
        /// Id`en til sagaen. Markert som unik.
        /// </summary>
        [Unique]
        public Guid SagaId { get; set; }

        /// <summary>
        /// Inneholder alle mottatte tekstelementer.
        /// </summary>
        public Dictionary<long, string> OrdListe { get; set; }

        /// <summary>
        /// Inneholder den originale teksthashen.
        /// </summary>
        public int OriginalTekstHash { get; set; }


        /// <summary>
        /// Hjelpemetode for å hente ut data fra ordboken.
        /// </summary>
        /// <returns></returns>
        public string GenererTekstBasertPåOrdliste()
        {
            var sb = new StringBuilder();
            foreach (var key in OrdListe.Keys.OrderBy(l => l))
            {
                string tmp = null;
                OrdListe.TryGetValue(key, out tmp);
                sb.Append(tmp + " ");
            }

            return sb.ToString().TrimEnd(new[] { ' ' });
        }
    }

    /// <summary>
    /// Melding som trigger begynnelsen på en ny saga.
    /// </summary>
    public class TekstSagaStartMelding : IMessage
    {
        /// <summary>
        /// Id`en til sagaen.
        /// </summary>
        public Guid SagaId { get; set; }

        /// <summary>
        /// Hashverdi som kan brukes for å validere riktigheten av mottatt tekst.
        /// </summary>
        public int TekstHash { get; set; }
    }

    /// <summary>
    /// Melding bestående av et subsett av en tekst.
    /// </summary>
    public class TekstSagaFragment : ICommand
    {
        /// <summary>
        /// Id`en til sagaen.
        /// </summary>
        public Guid SagaId { get; set; }

        /// <summary>
        /// Unik id til tekstfragmentet.
        /// </summary>
        public long FragmentId { get; set; }

        /// <summary>
        /// Tekstfragment.
        /// </summary>
        public string Fragment { get; set; }
    }

    /// <summary>
    /// Melding som kalles etter en gitt timeout periode inntreffer.
    /// </summary>
    public class TekstSagaTimeoutMelding
    {
        /// <summary>
        /// Id`en til sagaen.
        /// </summary>
        public Guid SagaId { get; set; }
    }

    public class TekstSagaResultat : ICommand
    {
        /// <summary>
        /// Id`en til sagaen.
        /// </summary>
        public Guid SagaId { get; set; }
        public bool Ok { get; set; }
    }
}