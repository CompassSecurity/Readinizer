This manual is a step by step guide on how to install Sysmon on a Windows domain without the use of an automatic software deployment tool. To achieve this goal, Sysmon is deployed through Group Policy Objects.

Sysmon is a monitoring service that logs events such as process creation, network connections and file access and changes. Sysmon logs events which Windows does not log and/or does this in a much more detailed way.

A network folder will be created to which each client has access. Three files are stored in this folder:
* Sysmon Executable: The regular Sysmon executable.
* Sysmon Configuration File: A XML-file which contains the configuration that will be applied to the Sysmon service.
* Batch File: The batch file will be executed remotely and check whether the Sysmon service is all ready installed and running. If this is not the case, it will install Sysmon on the computer.

A Group Policy Object will be created and applied to the domain. Within this GPO a Scheduled Task is set, which will execute the batch file in regular, defined intervals.