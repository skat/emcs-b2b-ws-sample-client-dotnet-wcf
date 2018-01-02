# EMCS B2B Sample Web Service Client written in .NET v4.6 using WCF

Sample client for the EMCS B2B Web Service Gateway developed in .NET v4.6 using WCF.

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

The sample client must be configured with **three** parameters that are necessary for the client to run and
call the test environment of EMCS B2B Web Service Gateway. The parameters can be obtained by contacting 
SKAT Help Desk.

Once obtained three **attribute values** in `App.config` must be replaced with the corresponding valued provided by
SKAT Help Desk as follows:

```
...
<clientCertificate findValue="CHANGE_ME_FRIENDLY_NAME"
    storeName="My" x509FindType="FindBySubjectName" />
...
<defaultCertificate findValue="CHANGE_ME_THUMBPRINT"
    storeLocation="LocalMachine" storeName="TrustedPeople" x509FindType="FindByThumbprint" />
...
<endpoint address="CHANGE_ME_ENDPOINT"
```

## Setup Client Certificate

The client certificate is used for authentication, signing
the request message, and decrypting the response message.

**Step 1:** Import PKCS#12 file 

On the host machine double click on the PKCS#12 file provided in the **Systems Integrator Package** to
activate the **Certificate Import Wizard**. 

Follow these steps:

* Select **Current User** and then **Next**.
* Then **Next** to confirm it's the right PKCS#12 file
* Enter the **Password** and keep default **Import options**.
* Keep default values for **Certificate Store** and then **Next**.
* Finally, select **Finish**

Start the **Microsoft Management Console (MMC)** and add the **Certificates** snap-in and opt-in for managing certificates for **My user account**. Then expand **Personal > Certificates** and verify that the PKCS12 file was imported.

**Example:**
![mmc_personal_certificates](/assets/mmc_personal_certificates.png)
 
**Step 2:** Locate friendly name

Click on the imported certificate and go to the **Details** tab to locate the **Friendly name** attribute.
This name is to used later on.

**Example:**
![mmc_personal_certificates_friendlyname](/assets/mmc_personal_certificates_friendlyname.png)

## Setup Server Certificate

**Step 1:** Import certificate

Double click on the **server certificate** provided in the **Systems Integrator package**.

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
**Thumbprint** value. This value is to be used later on.

**Example:**
![mmc_server_certificate_add_snap-in_viewcert_2](/assets/mmc_server_certificate_add_snap-in_viewcert_2.png)

## Run Project

Once `App.config` and certificates have been installed simply run `Program.cs`.

## Debugging

The sample client logs messages and other tracing information to this default path:

```
C:\logs\TracingAndLogging-client.svclog
```

This log is viewable using **Microsoft Service Trace Viewer** where,  for example, the plain text (xml) and encrypted requests are found.

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

If your service does not accept HTTPS connections change `httpsTransport` to `httpTransport` for the
custom binding `emcsCustomBinding` in `App.config`, e.g:

```
<customBinding>
        <binding name="emcsCustomBinding">
        ...
        <httpTransport />
        </binding>
</customBinding>
```

Given such a service exist and the service endpoint is known
(for instance: `http://localhost:8080/mymockservice`),
change the client endpoint in `App.config`, e.g.:

```
<client>
    <endpoint address="http://localhost:8080/mymockservice"
        behaviorConfiguration="ClientAuthenticationBehavior" binding="customBinding"
        bindingConfiguration="emcsCustomBinding" contract="EMCS.OIOLedsageDokumentOpretServicePortType"
        name="OIOLedsageDokumentOpretServicePort">
        ...
    </endpoint> 
</client>
```

**IMPORTANT**: If a cloud based mock service **with public bin** is used for debugging observe that these bins are **public** and viewable for anyone who has the URL of the bin, e.g.:

![mstv_capture](/assets/requestbin_inspect.png)