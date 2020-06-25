# Windows Event Collector
Choose a system (preferably a "Windows Server 2012R2" or above) for the WEC.


**Note:** Keep in mind that the logs should not be made accessible to all users! In the best case, a dedicated server to which only certain users have access is used as the WEC.
<br />
<br />
<br />
## Enable WinRM
To be able to receive events the Windows Remote Management Service (WinRM) must be enabled. 

Run an administrative command prompt and execute the following command:<br />``winrm qc``<br />

Answer the followed two questions ``Make these changes[y/n]`` with yes.

![Enable WinRM](https://github.com/clma91/Readinizer/blob/master/Wiki/Images/WEFManual/enable-winrm.png)

Do not close the administrative command prompt yet because it is needed in a further step.
<br />
<br />
<br />
## Enable Event Forwarding
The next step is to enable "Event Forwarding". Open the "Event Viewer" either by enter ``WIN + x`` or by open "Run" (``WIN + r``) and "enter"

In the "Event Viewer" click on "Subscriptions" and confirm with yes.

![Enable Subscriptions in the Event Viewer](https://github.com/clma91/Readinizer/blob/master/Wiki/Images/WEFManual/eventvwr.png)
<br />
<br />
<br />
## Group Policy Objects for the subscribers
In order for the clients to also send their events to the configured WEC, a corresponding Group Policy Object (GPO) must be defined. For the creation of this GPO, however, information is required in advance.

Use the administrative command prompt and execute the following command:<br />`wevtutil gl security`<br />

This command will give us the information about the "Security Event Log" where the permissions on the log are stored. Copy out `channelAccess` from `BAG:SYD...` through the last parenthesis and put it into a Notepad:

![Information about the Security Event Log](https://github.com/clma91/Readinizer/blob/master/Wiki/Images/WEFManual/securityEventLog.png)

If your output does not include `(A;;0x1;;;NS)` at the end - like depicted - append it to your temporary stored one in Notepad.
<br />
<br />
The next step is to create the actual GPO. There are two settings to be configured:

1. Register your created WEC for the clients: 
   * Edit the GPO and go to: `Computer Configuration > Policies > Admin Templates>Windows Components > Event Forwarding > Configure target subscription manager`
   * Enter: `Server=http://<FQDN-WEC>:5985/wsman/SubscriptionManager/WEC,Refresh=n`
   * Modify `<FQDN-WEC>` with the FQDN of your created WEC
   * Modify the refresh interval `Refresh=n` where `n` is in seconds (e.g. `Refresh=1800` means that every 30minutes the clients will check for new subscriptions)

![Configure GPO - Define WEC](https://github.com/clma91/Readinizer/blob/master/Wiki/Images/WEFManual/wecServer.png)

2. Configure the log access:
   * Edit the GPO and go to: `Computer Configuration > Policies > Admin Templates > Windows Components > Event Log Service>Security > Configure log access`
   * Enter the temporary stored access string (`O:BAG:SYD...`)

![Configure GPO - Log Access](https://github.com/clma91/Readinizer/blob/master/Wiki/Images/WEFManual/logAccess.png)

3. Configure the "WinRM" Service to be started automatically, so the clients are able to send their events to the WEC.
   * Edit the GPO and go to: `Computer Configuration> Preferences > Control Panel Settings > Services`
   * Right click on `Services` and create a new Service (`New > Service`)
   * Set the "Startup" to "Automatic"
   * Set the "Service action" to "Start service"

![Enable WinRM Service](https://github.com/clma91/Readinizer/blob/master/Wiki/Images/WEFManual/gpoService.png)
<br />
<br />
When everything is set, your GPO should look like this summary:

![GPO Summary](https://github.com/clma91/Readinizer/blob/master/Wiki/Images/WEFManual/gpoSettings.png)

Apply this GPO to all your computers which you want to forward their events to the WEC.
<br />
<br />
<br />
# WEF Subscription
The last step is to create a proper subscription that defines which events the clients will forward to the WEC. Therefore, a template was defined according to the bachelor thesis. The included event log IDs can be shown in the [Appendix A: Event Log IDs](https://github.com/clma91/Readinizer/wiki/Appendix-A:-Event-Log-IDs). 

There are two ways for defining a subscription - either you configure your subscription through the GUI or you import the subscription as a XML-file via the command line. This document describes both ways to give a good insight and to ensure maintainability when adjustments are needed.

## Define a subscription through the GUI<br />
   * Open the "Event Viewer" either by enter `WIN + x` or by open "Run" (`WIN + r`) and enter `eventvwr`.<br />
   * Click "Create Subscription" within the tab "Actions" on the left hand side of the Event Viewer.<br />
   * Enter a proper name for your new subscription and choose if the events get pulled ("Collector initiated") or pushed ("Source computer initiated" - recommended).<br /><br />![Subscription Properties](https://github.com/clma91/Readinizer/blob/master/Wiki/Images/WEFManual/subscriptionProp1.png)<br /><br />
   * Click on "Select Computer Groups..." and choose which computers should follow their events to the WEC. In our template the groups "Domain Controllers" and "Domain Computers" are included, which should cover a wide part of the Active Directory.<br /><br />![Subscription Computer Groups](https://github.com/clma91/Readinizer/blob/master/Wiki/Images/WEFManual/subscriptionProp2.png)<br /><br />
   * Within the next step we will configure the query filter which defines the events that get forwarded by the subscribed clients, therefore open "Select Events..". Select the setting "By log" and then "Security" or whatever you want to log. Afterwards select the Event ID which you want to track. Click "OK" when finished.<br /><br />![Subscription Query Filter](https://github.com/clma91/Readinizer/blob/master/Wiki/Images/WEFManual/subscriptionProp3.png)<br /><br />
   * The last step is to define the delivery mode. You can select between the following options:<br />
      * Normal: Ensures reliability and does not conserve bandwidth (batch timeout 15 minutes with 5 items)<br />
      * Minimize bandwidth: Ensures bandwidth (batch timout 6 hours)<br />
      * Minimize latency: Ensures minimal delay of event delivering<br /><br />![Subscription Advanced Settings](https://github.com/clma91/Readinizer/blob/master/Wiki/Images/WEFManual/subscriptionProp4.png)<br />


This settings can now be exported with the command (no administrative rights needed):<br />`gs SubscriptionName /f:xml > filename.xml`<br />

This will create a XML-file with the following content:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<Subscription xmlns="http://schemas.microsoft.com/2006/03/windows/events/subscription">
    <SubscriptionId>Audit Account Lockout</SubscriptionId>
    <SubscriptionType>SourceInitiated</SubscriptionType>
    <Description></Description>
    <Enabled>true</Enabled>
    <Uri>http://schemas.microsoft.com/wbem/wsman/1/windows/EventLog</Uri>
    <ConfigurationMode>MinLatency</ConfigurationMode>
    <Delivery Mode="Push">
        <Batching>
            <MaxLatencyTime>30000</MaxLatencyTime>
        </Batching>
        <PushSettings>
            <Heartbeat Interval="3600000"/>
        </PushSettings>
    </Delivery>
    <Query>
        <![CDATA[
            <QueryList>
                <Query Id="0">
                    <Select Path="Security">*[System[(EventID=4625)]]</Select>
                </Query>
            </QueryList>
        ]]>
    </Query>
    <ReadExistingEvents>true</ReadExistingEvents>
    <TransportName>HTTP</TransportName>
    <ContentFormat>RenderedText</ContentFormat>
    <Locale Language="en-US"/>
    <LogFile>ForwardedEvents</LogFile>
    <PublisherName>Microsoft-Windows-EventCollector</PublisherName>
    <AllowedSourceNonDomainComputers>
        <AllowedIssuerCAList></AllowedIssuerCAList>
    </AllowedSourceNonDomainComputers>
    <AllowedSourceDomainComputers>
        O:NSG:BAD:P(A;;GA;;;DC)(A;;GA;;;DD)S: 
    </AllowedSourceDomainComputers>
</Subscription>   
```
* **SubscriptionId:** Subscription name
* **SubscriptionType:** Clients which listens on subscriptions of the WEC will pull the subscription (does not mean pull procedure regarding event forwarding)
* **ConfigurationMode:** Defines the delivery mode options (normal, minimize bandwidth, minimize latency) 
* **Delivery Mode="<Mode>":** Defines the delivery mode (pull/push)
* **Query:** Defines the query which EventIDs are forwarded
* **ReadExistingEvents:** Defines if existing events should be forwarded (`true`) or only new ones (`false`)
* **LogFile:** Defines where the events - which the clients forwarded to the WEC - should be saved (you can define your own .evtx-File for each subscription)
* **AllowedSourceDomainComputers:**  Defines which computers should listen to this subscription. If you have multiple domains, you have to get the identifiers of every domain connecting to your Windows Event Collector (WEC). The most simple way to do this is to make a new subscription from the GUI and export -> then copy the identifiers and import the new file. With the identifier of the above XML "Domain Users" and "Domain Computers" will listen to this subscription.


## Import the subscription with a XML-file 
   * Run an administrative command prompt and execute the following command to import a single subscription:<br />`wecutil cs C:\path\to\filename.xml`<br />
   * To import multiple subscriptions defined as an XML-file run, an administrative command prompt and execute the following command:<br />`for \%f in (C:\path\to\your\subscriptions\*.xml) do  wecutil cs "\%f"`<br />
   * The subscription "ReadinizerWEFRecommendation.xml" (see [Appendix B: ReadinizerWEFRecommendation.xml](https://github.com/clma91/Readinizer/wiki/Appendix-B:-ReadinizerWEFRecommendation.xml) which is based on [Appendix A: Event Log IDs](https://github.com/clma91/Readinizer/wiki/Appendix-A:-Event-Log-IDs)) defines event logs which should be audited with the Windows Event Collector for a better lateral movement analysis.