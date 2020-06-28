# What is Windows Event Forwarding?
Windows Event Forwarding (WEF) allows system administrators that logs are no longer stored on individual clients and servers (further referred as only clients) within the organisation, but centrally on a server. A Windows Event Collector (WEC) server is defined as the central instance responsible for collecting the client logs. The event logs are written on the individual clients and then forwarded to the WEC. 


On the WEC, subscriptions can be created for the clients, which define which event logs the clients should forward to the WEC. WEF subscription can be set up as push or pull procedure. In principle, however, the pull procedure should not be used, as the WEC queries all clients for their event logs that have not yet been sent. This means that at certain times the network is stressed by many clients. In contrast, the push procedure does not stress the network as much as the clients themselves decide when to send the event logs to the WEC.

# Advantages with WEF
WEF is a passive system with regard to event logging, which ensures the completeness and a longer lifetime of the event logs. Even with WEF, events are still logged on clients and servers, but forwarded to the central instance. This in turn allows a much faster forensic analysis in case of advanced persistent threat (APT) or lateral movement - conventional event logging (like specific application logs) can also be stored centrally. With the extended lifetime of event logs, APTs can be better tracked and analyzed. From the technical report on the "RUAG cyber espionage case" it is clear that a long lifetime of log files can improve a complete forensic analysis:

> _Unfortunately, log files at RUAG only go back until September 2014, where we still see C&C activity. Additionally, many suspicious devices have been reinstalled in the meantime; Hence we cannot determine the initial attack vector. <sup>1</sup>_ 

This manual is mostly based on Jessica Paynes article "Monitoring what matters – Windows Event Forwarding for everyone (even if you already have a SIEM.)" <sup>2</sup>

***
<sup>1</sup> [GovCERT.ch, APTCaseRUAG(EspionageCaseatRUAG), Technical report, MELANI:GovCERT, May 2016](https://www.melani.admin.ch/melani/en/home/dokumentation/reports/technical-reports/technical-report_apt_case_ruag.htm)
<sup>2</sup> [JessicaPayne, Monitoring what matters - Windows Event Forwarding for everyone(even if you already have a SIEM.), November 2015](https://blogs.technet.microsoft.com/jepayne/2015/11/23/monitoring-what-matters-windows-event-forwarding-for-everyone-even-if-you-already-have-a-siem/)