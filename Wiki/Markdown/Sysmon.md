# What is Sysmon?
Sysmon is a monitor service developed by Mark Russinovich and Thomas Garnier. They describe Sysmon as followed:
> _System Monitor (Sysmon) is a Windows system service and device driver that, once installed on a system, remains resident across system reboots to monitor and log system activity to the Windows event log. It provides detailed information about process creations, network connections, and changes to file creation time.<sup>1</sup>_

# Why Sysmon?
This is the conclusion to Sysmon from our study thesis: 
> _Sysmon logs several events on the system which are partly logged by default too. For example, the event ``A new process has been created''  with the identifier (ID) 4688 is logged by Sysmon with the ID 1 ``Process Creation'' . The problem is that the default logged event with the ID 4688 logs only the executable file (EXE) name as well as the including path. But attackers want to stay below the radar, so they might replace the original EXE a with malicious one and rename it like the original. Hence, there is no way to determine with the system based event log entry 4688 if the original EXE was executed. Sysmon eliminates exactly this gap by logging not only the name and path of the EXE but also the hash value of the EXE. Ergo Sysmon brings a big advantage to detect if a malicious EXE was executed or not. Therefore a reference hash value of the executed EXE is required to compare the hash values on its correctness._

More detailed information can be found at [Sysmon](https://docs.microsoft.com/en-us/sysinternals/downloads/sysmon)


***
<sup>1</sup>[Mark Russinovich & Thomas Garnier, Sysmon v9.0, https://docs.microsoft.com/enus/sysinternals/downloads/sysmon, February 2019]
