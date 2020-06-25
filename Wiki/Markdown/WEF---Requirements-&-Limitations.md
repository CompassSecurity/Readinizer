# Requirements
A WEC environment can be deployed on any "Windows 10" or "Windows Server 2012R2" system and above. However, it is highly recommended to use a "Windows Server" with enough disk space. Furthermore, it is recommended to use disks which have a "high speed" write capability to increase the number of events per second that a one WEC can handle.

# Limitations
The following limitations have been derived from microsoft \footnote{Further information about WEF deployment: [Use Windows Event Forwarding to help with intrusion detection](https://docs.microsoft.com/en-us/windows/security/threat-protection/use-windows-event-forwarding-to-assist-in-intrusion-detection)
* There are no recommendations in this manual for disk sizes, as this can vary greatly depending on the number of clients within the network.
* A WEC can only handle a limited number of clients due its limitation of available TCP ports. Therefore, the number of clients which subscribe to a single WEC must be considered.
* The registry size of the WEC can increase to an unmanageable size over time. Because for every client - which connects to a WEF subscription - a registry key is created in order to store bookmark and source heartbeat information. Unfortunately, inactive or no longer existing clients are not removed. A quote from Microsoft in this regard:


> * _When a subscription has >1000 WEF sources connect to it [...] Event Viewer can become unresponsive for a few minutes when selecting the Subscriptions node in the left-navigation, but will function normally afterwards. <sup>1</sup>_
> * _At >50,000 lifetime WEF sources, Event Viewer is no longer an option and wecutil.exe (included with Windows) must be used to configure and manage subscriptions. <sup>1</sup>_
> * _At >100,000 lifetime WEF sources, the registry will not be readable and the WEC server will likely have to be rebuilt. <sup>1</sup>_


# Additional Information
* WEF can handle VPN, RAS and DirectAccess connected clients
* The clients local event log acts as a buffer in case of connection loss
* Supports IPv4 and IPv6
* In a Active Directory environment there is no need for additional settings to encrypt the events which will be sent to the WEC. By default the events are encrypted using Kerberos (with NTLM as a fallback option). More information see [WEF Encryption](https://github.com/clma91/Readinizer/wiki/WEF-Encryption)

***
<sup>1</sup> [Microsoft, Use Windows Event Forwarding to help with intrusion detection, February 2019](https://docs.microsoft.com/en-us/windows/security/threat-protection/use-windows-event-forwarding-to-assist-in-intrusion-detection)
