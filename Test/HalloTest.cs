namespace Test
{
    using Meldinger;
    using NServiceBus.Testing;
    using NUnit.Framework;
    using Server;

    public class HalloTest : TestOppsett
    {
        [Test]
        public void VerifiserAtTilbakeMeldingInneholderEtSubsettAvFørsteMelding()
        {
            Test.Handler<HalloHandler>()
                .ExpectReply<HalloTilbakeMelding>(melding => melding.HeiIgjen.Contains("Hei på deg"))
                .OnMessage<HalloMelding>(melding => { melding.Hallo = "Hei på deg"; });
        }
    }
}