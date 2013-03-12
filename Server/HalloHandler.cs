namespace Server
{
    using System;
    using Meldinger;
    using NServiceBus;

    public class HalloHandler : IHandleMessages<HalloMelding>
    {
        public IBus Bus { get; set; }

        public void Handle(HalloMelding melding)
        {
            Console.Out.WriteLine(@"SERVER: melding med hilsen '{0}' ble mottatt.", melding.Hallo);
            Bus.Reply(new HalloTilbakeMelding
            {
                HeiIgjen = melding.Hallo + ", også"
            });
        }
    }
}






