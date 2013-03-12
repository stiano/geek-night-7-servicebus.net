namespace Klient
{
    using System;
    using Meldinger;
    using NServiceBus;

    public class HalloIgjenHandler : IHandleMessages<HalloTilbakeMelding>
    {
        public void Handle(HalloTilbakeMelding melding)
        {
            Console.Out.WriteLine(@"KLIENT: mottatt melding med hilsen '{0}' ble mottatt.", melding.HeiIgjen);
        }
    }
}