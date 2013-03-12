namespace Test
{
    using NServiceBus;
    using NServiceBus.Testing;
    using NUnit.Framework;

    public abstract class TestOppsett
    {
        [TestFixtureSetUp]
        public void TestSetup()
        {
            MessageConventionExtensions.IsMessageTypeAction =
                t => t.Namespace != null && t.Namespace.EndsWith("Meldinger");

            Test.Initialize();
        }
    }
}