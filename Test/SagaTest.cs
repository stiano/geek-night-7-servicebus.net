namespace Test
{
    using System;
    using Meldinger;
    using NUnit.Framework;
    using NServiceBus.Testing;
    using Server;

    [TestFixture]
    public class SagaTest : TestOppsett
    {
        [Test]
        public void VerifiserAtSagaFungererSomTiltenkt()
        {
            var sagaId = Guid.NewGuid();
            const string Tekst = "Hei";

            Test.Saga<TekstSaga>()
                .ExpectReplyToOrginator<TekstSagaResultat>(m => m.SagaId == sagaId && m.Ok == true)
                .ExpectTimeoutToBeSetIn<TekstSagaTimeoutMelding>((tilstand, span) => span == TimeSpan.FromSeconds(5))
                .When(saga =>
                    {
                        // Start saga
                        saga.Handle(new TekstSagaStartMelding
                        {
                            SagaId = sagaId,
                            TekstHash = Tekst.GetHashCode()
                        });

                        // Tekst som skal prosesseres.
                        {
                            saga.Handle(new TekstSagaFragment { SagaId = sagaId, FragmentId = 1, Fragment = "Hei" });
                        }

                        // Timeout
                        saga.Timeout(new TekstSagaTimeoutMelding
                        {
                            SagaId = sagaId
                        });

                    })
                .AssertSagaCompletionIs(true);
        }
    }
}
