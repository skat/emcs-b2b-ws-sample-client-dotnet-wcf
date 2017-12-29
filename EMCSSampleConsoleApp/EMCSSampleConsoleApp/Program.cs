using System;
using System.Xml;

namespace EMCSSampleConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("+Sample client started");
            EMCS.OIOLedsageDokumentOpretServicePortTypeClient client = new EMCS.OIOLedsageDokumentOpretServicePortTypeClient();
            EMCS.OIOLedsageDokumentOpret_IType request = new EMCS.OIOLedsageDokumentOpret_IType();
            EMCS.VirksomhedIdentifikationStrukturType struktur = new EMCS.VirksomhedIdentifikationStrukturType();
            EMCS.VirksomhedIdentifikationStrukturTypeIndberetter indberetter = new EMCS.VirksomhedIdentifikationStrukturTypeIndberetter();
            indberetter.VirksomhedSENummerIdentifikator = "12345678";
            struktur.Indberetter = indberetter;
            struktur.AfgiftOperatoerPunktAfgiftIdentifikator = "12345678";
            EMCS.HovedOplysningerType ho = new EMCS.HovedOplysningerType();
            ho.TransaktionIdentifikator = Guid.NewGuid().ToString();
            ho.TransaktionTid = System.DateTime.Now;
            ho.TransaktionTidSpecified = true;
            Console.WriteLine("** TransaktionIdentifikator = " + ho.TransaktionIdentifikator);
            Console.WriteLine("** TransaktionTid =" + ho.TransaktionTid);
            request.HovedOplysninger = ho;
            request.VirksomhedIdentifikationStruktur = struktur;
            request.IE815Struktur = GetElement("<ie:IE815 xmlns:ie=\"urn: publicid:-:EC: DGTAXUD:EMCS: PHASE3:IE815: V1.76\"></ie:IE815>");
            client.getOIOLedsageDokumentOpret(request);
            Console.WriteLine("-Sample client ended");
        }

        private static XmlElement GetElement(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc.DocumentElement;
        }

        private static XmlElement loadElement(String file)
        {
            // Create an XmlDocument object.
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.Load(file);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return xmlDoc.DocumentElement;
        }
    }
}
