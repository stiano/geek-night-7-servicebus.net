namespace Klient
{
    using System;
    using Meldinger;
    using NServiceBus;

    public class SagaHandler : IHandleMessages<TekstSagaResultat>
    {
        public IBus Bus { get; set; }

        public void Handle(TekstSagaResultat melding)
        {
            Console.Out.WriteLine(@"KLIENT: Saga med id '{0}' ble fullført med status '{1}'.",
                melding.SagaId,
                melding.Ok ? "OK" : "IKKE OK");
        }
    }
}