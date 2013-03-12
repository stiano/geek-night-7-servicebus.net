namespace Meldinger
{
    using NServiceBus;



    public class HalloMelding : IMessage
    {
        public string Hallo { get; set; } 
    }

    public class HalloTilbakeMelding : IMessage
    {
        public string HeiIgjen { get; set; }
    }
}