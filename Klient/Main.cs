namespace Klient
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Meldinger;
    using NServiceBus;

    public class Main : IWantToRunAtStartup
    {
        public static Func<string, string> LesFil =
            filnavn => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Filer", filnavn));

        public IBus Bus { get; set; }

        public void Run()
        {
            Console.Out.WriteLine(@"KLIENT: Sender en hilsen til den som måtte være interessert.");
            Bus.Send("Server", new HalloMelding
            {
                Hallo = "Hei på deg!!!"
            });

            StartSaga(Bus);
        }

        public void Stop()
        {
        }

        private static void StartSaga(IBus bus)
        {
            const string Tekst = "en veldig lang tekst skal dette bli og det er ingen grenser for hvor frekk den kan være";
            var tekstHash = Tekst.GetHashCode();

            var sagaId = Guid.NewGuid();
            Console.Out.WriteLine(@"KLIENT: Starter saga med id '{0}'.", sagaId);

            // Starter sagaen
            bus.Send("Server", new TekstSagaStartMelding
            {
                SagaId = sagaId, 
                TekstHash = tekstHash
            });
            
            // Sender tekstfragment
            bus.Send("Server", new TekstSagaFragment
            {
                SagaId = sagaId,
                FragmentId = 1,
                Fragment = Tekst
            });
        }
    }
}
