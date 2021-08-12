# EMCS B2B Sample Web Service Client written in .NET v4.6 using WCF

> **PLEASE NOTE**: This sample code will not run on .NET Core 3.0. For more information: [.NET Core is the Future of .NET](https://devblogs.microsoft.com/dotnet/net-core-is-the-future-of-net/) and [Windows Communication Framework: Should You Leave?](https://blog.inedo.com/dotnet/windows-comm-framework)

Sample client for the EMCS B2B Web Service Gateway developed in C# / .NET v4.6 using WCF.

**IMPORTANT NOTICE**: SKAT does not provide any kind of support for the code in this repository.
This .NET-client is just one example of how a B2B web service can be accessed. The client must not be 
perceived as a piece of production code but more as an example one can take inspiration from and can use
to quickly get started to test whether your company can implement a successful call to one of the B2B web 
service using the company's digital signature. SKAT can not be held responsible if a company uses this client 
or parts of it in their own systems. 

**VIGTIG MEDDELELSE**: SKAT yder ikke support på kildekoden i nærværende kodebibliotek.
Denne .NET-klient er kun et eksempel på hvordan B2B webservicene kan tilgås. Klienten skal således ikke 
opfattes som et stykke produktionskode men mere som en eksempel man kan lade sig inspirere af og kan bruge 
til hurtigt at komme i gang og få afprøvet om ens virksomhed kan gennemføre et succesfuldt kald til en af 
B2B webservicene ved at bruge virksomhedens digitale signatur. SKAT kan ikke stå til ansvar hvis en virksomhed
anvender klienten eller dele af denne i deres egne systemer. 




## Setup Project

The sample client must be configured with **four** parameters that are necessary for the client to run and
call the test environment of EMCS B2B Web Service Gateway. The parameters can be obtained by contacting 
SKAT Help Desk.

Once obtained, four **attribute values** in `App.config` must be replaced with the corresponding value provided by
SKAT Help Desk:

```
<appSettings>
    <add key="client.endpoint.identity.dns" value="CHANGE_ME_CERT_IDENTITY" />
    <add key="client.endpoint.address" value="CHANGE_ME_ENDPOINT" />
    <add key="behaviors.endpointBehaviors.behavior.clientCredentials.clientCertificate.findValue" value="CHANGE_ME_THUMBPRINT_CLIENT" />
    <add key="behaviors.endpointBehaviors.behavior.clientCredentials.serviceCertificate.findValue" value="CHANGE_ME_THUMBPRINT_SERVER" />
</appSettings>
```

## Setup Client Certificate

The client certificate is used for authentication, signing
the request message, and decrypting the response message.

**Step 1:** Import PKCS#12 file 

On the host machine double click on the PKCS#12 file provided in the **Systems Integrator Package** (also downloadable from [here](https://github.com/skat/emcs-b2b-ws-sample-client-java/blob/master/p12/VOCES_gyldig.p12)) to activate the **Certificate Import Wizard**. 

Follow these steps:

* Select **Current User** and then **Next**.
* Then **Next** to confirm it's the right PKCS#12 file
* Enter the **Password** and keep default **Import options**.
* Keep default values for **Certificate Store** and then **Next**.
* Finally, select **Finish**

Start the **Microsoft Management Console (MMC)** and add the **Certificates** snap-in and opt-in for managing certificates for **My user account**. Then expand **Personal > Certificates** and verify that the PKCS12 file was imported.

**Example:**
![mmc_personal_certificates](/assets/mmc_personal_certificates.png)
 
**Step 2:** Locate Thumbprint

Click on the imported certificate and go to the **Details** tab to locate the **Thumbprint** attribute.

**Example:**
![mmc_personal_certificates_friendlyname](/assets/mmc_personal_certificates_friendlyname.png)

This value is used in:

```
<add key="behaviors.endpointBehaviors.behavior.clientCredentials.clientCertificate.findValue" value="..." />
```
*IMPORTANT*: Thumbprint must not contain any white spaces.

## Setup Server Certificate

**Step 1:** Import certificate

Double click on the **server certificate** provided in the **Systems Integrator package** (also downloadable from [here](https://github.com/skat/emcs-b2b-ws#server-certificates)).

Select **Install Certificate** to activate the **Certificate Import Wizard**. 

Follow these actions:

* Select **Local Machine** and then **Next**.
* Change **Certificate Store** to **Trusted People** (browse to location) then **Next**.
* Finally, select **Finish**

Start the **Microsoft Management Console (MMC)** and add the **Certificates** snap-in and opt-in for managing certificates for **Computer account**. 

**Example:**
![mmc_server_certificate_add_snap-in](/assets/mmc_server_certificate_add_snap-in.png)

On the **Select Computer** screen keep default value and then **Finish**

**Example:**
![mmc_server_certificate_add_snap-in_localcomputer](/assets/mmc_server_certificate_add_snap-in_localcomputer.png)

Then expand **Trusted People > Certificates** and verify that the certificate was imported.

**Example:**
![mmc_server_certificate_add_snap-in_viewcert_1](/assets/mmc_server_certificate_add_snap-in_viewcert_1.png)

**Step 2:** Locate thumbprint

Click on certificate and on the details tab locate the
**Thumbprint** value. 

**Example:**
![mmc_server_certificate_add_snap-in_viewcert_2](/assets/mmc_server_certificate_add_snap-in_viewcert_2.png)

This value is used in:

```
<add key="behaviors.endpointBehaviors.behavior.clientCredentials.serviceCertificate.findValue" value="..." />
```

*IMPORTANT*: Thumbprint must not contain any white spaces.

## Run Project

Once `App.config` and certificates have been installed then copy `ie815.xml` to `C:/IE815.xml` and set (for testing only):

```
<ns26:DeferredSubmissionFlag>1</ns26:DeferredSubmissionFlag>
```

Run `Program.cs` and console should produce this output:

```
+Sample client started
**** REQUEST ****
** TransaktionIdentifikator = a3f02d87-190f-439f-8df1-c8264bce94f8
** TransaktionTid = 1/5/2018 10:00:09 AM
** VirksomhedIdentifikationStruktur/Indberetter/VirksomhedSENummerIdentifikator = 30808460
** VirksomhedIdentifikationStruktur/AfgiftOperatoerPunktAfgiftIdentifikator = DK82065873300
** IE815 = <ie:IE815 xmlns:ie="urn:publicid:-:EC:DGTAXUD:EMCS:PHASE3:IE815:V1.92">
    <ie:Header xmlns:tms="urn:publicid:-:EC:DGTAXUD:EMCS:PHASE3:TMS:V1.92">
        <tms:MessageSender>NDEA.DK</tms:MessageSender>
        <tms:MessageRecipient>NDEA.DK</tms:MessageRecipient>
        <tms:DateOfPreparation>2011-10-26</tms:DateOfPreparation>
        <tms:TimeOfPreparation>11:23:00.803</tms:TimeOfPreparation>
        <tms:MessageIdentifier>9e1e74a5-aaae-41d6-8280-c3892246e613</tms:MessageIdentifier>
    </ie:Header>
    <ns26:Body xmlns:ns26="urn:publicid:-:EC:DGTAXUD:EMCS:PHASE3:IE815:V1.92">
    ...
    REMOVED
    ...
    </ns26:Body>
</ie:IE815>
**** RESPONSE ****
** TransaktionIdentifikator = a3f02d87-190f-439f-8df1-c8264bce94f8
** ServiceIdentifikator = FS2_OIOLedsageDokumentOpret
** TransaktionTid = 1/5/2018 10:00:09 AM
** AdvisStruktur..
**** FejlTekst: Message identifier has been already used
**** FejlIdentifikator: 494
Press key to end sample...
```

## About the code

* The EMCS Service Reference was built from this
[respository file (wsdl)](https://raw.githubusercontent.com/skat/emcs-b2b-ws/master/wsdl/OIOLedsageDokumentOpret/OIOLedsageDokumentOpret.wsdl). Consequently, updates to this **Connected Service** will require access to the GitHub repository hosted on github.com. This approach is NOT RECOMMENDED
for real integration projects. Note: The code herein is just sample code!
* Configuration of WCF Security through `App.config` to comply with the full WS-Security Policy on the EMCS B2B Services Gateway is currently *NOT* possible in WCF. The one configuration missing is signing of the BinarySecurityToken (BST) in the request. This is in this sample solved by moving the custom binding configuration to `Program.cs` (based on the discussion in this 
[thread](https://stackoverflow.com/questions/13453921/how-to-make-wcf-client-sign-securitytokenreferencereference)). Consequently, the behaviour and endpoint configuration in `App.config` had to be moved as well. However, the configation in `App.config` is still present in the event
a future version WCF extends `App.config` to enable signing of the BST, but is **NOT** in use by the client. 

## Debugging

The sample client logs messages and other tracing information to this default path:

```
C:\logs\TracingAndLogging-client.svclog
```

This log is viewable using **Microsoft Service Trace Viewer** where, for example, the plain text (xml) and encrypted requests are found.

**Example:**
![mstv_capture](/assets/mstv_capture.png)

### Advanced debugging (outbound requests)

The **Microsoft Service Trace Viewer** pretty prints the XML request and does no show all values, e.g.:

```
<o:BinarySecurityToken>
<!-- Removed-->
</o:BinarySecurityToken>
```

In the event that you require to see the actual on-the-wire **outbound** raw xml request being sent the following recipe may be used as inspiration.

Setup a HTTP service that accepts `POST` operations and logs the full
request including HTTP headers, but does not have to return a valid response. We just want to see the request. Such a service could be a **SoapUI Mock Service**, a self-built HTTP service, or even a cloud based service.

If your service does not accept HTTPS connections change `HttpsTransportBindingElement` to `HttpTransportBindingElement` 
in `Program.cs`, e.g:

```
HttpTransportBindingElement httpbinding = new HttpTransportBindingElement();
```

Given such a service exist and the service endpoint is known
(for instance: `http://localhost:8080/mymockservice`),
change the client endpoint in `App.config`, e.g.:

```
<appSettings>
    <add key="client.endpoint.address" value="http://localhost:8080/mymockservice" />
</appSettings>
```

**IMPORTANT**: If a cloud based mock service **with public bin** is used for debugging observe that these bins are **public** and viewable for anyone who has the URL of the bin, e.g.:

![mstv_capture](/assets/requestbin_inspect.png)
