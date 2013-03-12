namespace Meldinger
{
    using System.Text;

    public static class TekstGenerator
    {
        /// <summary>
        /// Oppretter en repeterende tekststreng.
        /// </summary>
        public static string Opprett(int repetisjoner = 10, string tekst = "en veldig lang tekst skal dette bli og det er ingen grenser for hvor frekk den kan være ")
        {
            var sb = new StringBuilder();

            for (int i = 0; i < repetisjoner; i++)
            {
                sb.Append(tekst);
            }

            return sb.ToString().TrimEnd();
        }  
    }
}