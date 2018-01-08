using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security.Tokens;
using System.Xml;
using System.Configuration;

namespace EMCSSampleConsoleApp
{
    class Program
    {
        /// <summary>
        /// Run sample
        /// </summary>
        static void Main(string[] args)
        {
            // Read configuration from App.config
            // These four settings configure the custom binding.
            string identityDns = ConfigurationManager.AppSettings["client.endpoint.identity.dns"];
            string clientEndpointAddress = ConfigurationManager.AppSettings["client.endpoint.address"];
            string clientCertificateFindValue = ConfigurationManager.AppSettings["behaviors.endpointBehaviors.behavior.clientCredentials.clientCertificate.findValue"];
            string serviceCertificateFindValue = ConfigurationManager.AppSettings["behaviors.endpointBehaviors.behavior.clientCredentials.serviceCertificate.findValue"];

            // Build custom binding.
            CustomBinding binding = BuildCustomBinding();

            // Create the endpoint address and identity to be used for the client.
            EndpointAddress address =
               new EndpointAddress(new Uri(clientEndpointAddress),
               EndpointIdentity.CreateDnsIdentity(identityDns));

            Console.WriteLine("+Sample client started");
            EMCS.OIOLedsageDokumentOpretServicePortTypeClient client = new EMCS.OIOLedsageDokumentOpretServicePortTypeClient(binding, address);
            // Set client and server certificate
            // The next two lines are required in combination with the CustomBinding
            client.ClientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindByThumbprint, clientCertificateFindValue);
            client.ClientCredentials.ServiceCertificate.SetDefaultCertificate(StoreLocation.LocalMachine, StoreName.TrustedPeople, X509FindType.FindByThumbprint, serviceCertificateFindValue);

            // Now build request
            EMCS.OIOLedsageDokumentOpret_IType request = new EMCS.OIOLedsageDokumentOpret_IType();
            EMCS.VirksomhedIdentifikationStrukturType struktur = new EMCS.VirksomhedIdentifikationStrukturType();
            EMCS.VirksomhedIdentifikationStrukturTypeIndberetter indberetter = new EMCS.VirksomhedIdentifikationStrukturTypeIndberetter();
            indberetter.VirksomhedSENummerIdentifikator = "30808460";
            struktur.Indberetter = indberetter;
            struktur.AfgiftOperatoerPunktAfgiftIdentifikator = "DK82065873300";
            EMCS.HovedOplysningerType ho = new EMCS.HovedOplysningerType();
            ho.TransaktionIdentifikator = Guid.NewGuid().ToString();
            ho.TransaktionTid = System.DateTime.Now;
            ho.TransaktionTidSpecified = true;
            request.HovedOplysninger = ho;
            request.VirksomhedIdentifikationStruktur = struktur;
            // CHANGE LOCATION if the ie815.xml file is in another location!
            request.IE815Struktur = loadElement("C:/ie815.xml"); 
            // Print request to console
            Console.WriteLine("**** REQUEST ****");
            Console.WriteLine("** TransaktionIdentifikator = " + ho.TransaktionIdentifikator);
            Console.WriteLine("** TransaktionTid = " + ho.TransaktionTid);
            Console.WriteLine("** VirksomhedIdentifikationStruktur/Indberetter/VirksomhedSENummerIdentifikator = " + request.VirksomhedIdentifikationStruktur.Indberetter.VirksomhedSENummerIdentifikator);
            Console.WriteLine("** VirksomhedIdentifikationStruktur/AfgiftOperatoerPunktAfgiftIdentifikator = " + request.VirksomhedIdentifikationStruktur.AfgiftOperatoerPunktAfgiftIdentifikator);
            Console.WriteLine("** IE815 = " + request.IE815Struktur.OuterXml);
            // Send request
            EMCS.OIOLedsageDokumentOpret_OType response = client.getOIOLedsageDokumentOpret(request);

            // Parse request
            Console.WriteLine("**** RESPONSE ****");
            Console.WriteLine("** TransaktionIdentifikator = " + response.HovedOplysningerSvar.TransaktionIdentifikator);
            Console.WriteLine("** ServiceIdentifikator = " + response.HovedOplysningerSvar.ServiceIdentifikator);
            Console.WriteLine("** TransaktionTid = " + response.HovedOplysningerSvar.TransaktionTid);
            for (int i = 0; i < response.HovedOplysningerSvar.SvarStruktur.Length; i++)
            {
                object obj = response.HovedOplysningerSvar.SvarStruktur[i];
                if (obj is EMCS.AdvisStrukturType)
                {
                    EMCS.AdvisStrukturType advisStruktur = (EMCS.AdvisStrukturType)obj;
                    Console.WriteLine("** AdvisStruktur..");
                    Console.WriteLine("**** AdvisTekst: " + advisStruktur.AdvisTekst);
                    Console.WriteLine("**** AdvisIdentifikator.." + advisStruktur.AdvisIdentifikator);
                }
                else if (obj is EMCS.FejlStrukturType)
                {
                    EMCS.FejlStrukturType fejlStruktur = (EMCS.FejlStrukturType)obj;
                    Console.WriteLine("** AdvisStruktur..");
                    Console.WriteLine("**** FejlTekst: " + fejlStruktur.FejlTekst);
                    Console.WriteLine("**** FejlIdentifikator: " + fejlStruktur.FejlIdentifikator);
                }
            }
            Console.WriteLine("Press key to end sample...");
            Console.ReadKey();
            Console.WriteLine("-Sample client ended");
        }

        /// <summary>
        /// Build custom binding and compensate for missing "sign Binary Security Token" option in App.config
        /// </summary>
        private static CustomBinding BuildCustomBinding()
        {
            var customBinding = new CustomBinding();

            SecurityBindingElement securityBindingElement = SecurityBindingElement.CreateMutualCertificateBindingElement(
                    MessageSecurityVersion.WSSecurity10WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10);

            System.ServiceModel.Channels.AsymmetricSecurityBindingElement
            asymmetricSecurityBindingElement = new AsymmetricSecurityBindingElement();

            asymmetricSecurityBindingElement.MessageSecurityVersion =
                      MessageSecurityVersion.WSSecurity10WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10;

            asymmetricSecurityBindingElement.InitiatorTokenParameters = new
              System.ServiceModel.Security.Tokens.X509SecurityTokenParameters
            { InclusionMode = SecurityTokenInclusionMode.Never };
            asymmetricSecurityBindingElement.RecipientTokenParameters = new
              System.ServiceModel.Security.Tokens.X509SecurityTokenParameters
            { InclusionMode = SecurityTokenInclusionMode.Never };
            asymmetricSecurityBindingElement.MessageProtectionOrder =
              System.ServiceModel.Security.MessageProtectionOrder.SignBeforeEncrypt;

            // Layout header must be LAX!
            asymmetricSecurityBindingElement.SecurityHeaderLayout = SecurityHeaderLayout.Lax;
            asymmetricSecurityBindingElement.EnableUnsecuredResponse = true;
            asymmetricSecurityBindingElement.IncludeTimestamp = true;
            asymmetricSecurityBindingElement.AllowSerializedSigningTokenOnReply = true;
            asymmetricSecurityBindingElement.SetKeyDerivation(false);
            asymmetricSecurityBindingElement.DefaultAlgorithmSuite =
              System.ServiceModel.Security.SecurityAlgorithmSuite.TripleDesRsa15;
            asymmetricSecurityBindingElement.EndpointSupportingTokenParameters.Signed.Add(
              new X509SecurityTokenParameters());

            customBinding.Elements.Clear();
            customBinding.Elements.Add(asymmetricSecurityBindingElement);
            customBinding.Elements.Add(new TextMessageEncodingBindingElement()
            {
                MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap11,
                AddressingVersion.None),
                WriteEncoding = new System.Text.UTF8Encoding()
            });
            // For mock service running http only, change to HttpTransportBindingElement here:
            HttpsTransportBindingElement httpbinding = new HttpsTransportBindingElement();
            httpbinding.AuthenticationScheme = AuthenticationSchemes.Anonymous;
            customBinding.Elements.Add(httpbinding);
            return customBinding;
        }


        /// <summary>
        /// Make XML as String into XMLElement
        /// </summary>
        private static XmlElement getElementFromString(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc.DocumentElement;
        }

        /// <summary>
        /// Make XML as File into XMLElement
        /// </summary>
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
