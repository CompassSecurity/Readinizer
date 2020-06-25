## Operating System
The Readinizer runs on all Windows 10 Professional Version 1709  operated systems as well as on all servers with the operating system Windows Server 2016.

## User Authorizations
To run the Readinizer successfully, the user needs administrator rights on the executing machine. Additionally, he needs Local Administrator and Remote Desktop User rights in every domain that is going to be analyzed. It is recommended to create a custom user/user group.

## Firewall Settings
To enable the Readinizer to read the Resultant Set of Policies, the Remote Server Administration Tools (RSAT) must be installed/activated on the executing device.

## Pre-Installed Software
### Remote Server Administration Tool
To enable the Readinizer to read the Resultant Set of Policies, the Remote Server Administration Tools (RSAT) must be installed/activated on the executing device.

#### Version 1803 and older
For computers with Windows 10 Version 1803 and older the RSAT can be downloaded here:

[RSAT](https://www.microsoft.com/en-us/download/details.aspx?id=45520)

The installation is simple and self-explanatory.

#### Version 1809 and newer
Since the October 2018 update, the RSAT is pre-installed on Windows Professional machines. However, it still has to be activated. To do so open on your system:   
* _Settings --> App_ 
* The click on **Manage optional features**
* Then click the **Add a feature** button.
* Scroll down unti you see the **RSAT: Group Policy Management Tools** and install this feature.

### SQLLocalDB
To display the complexity of an Active Directory, the Readinizer needs a database. For this a lightweight database is used, a SQLLocalDB. To install the LocalDB download the SQL Server Express installer. It can be downloaded here:
[SQL Server 2017 Express edition](https://www.microsoft.com/en-us/sql-server/sql-server-editions-express) 
After executing the downloaded installer, a installation type has to be selected. Choose **Download Media**
* Select the **LocalDB**-toggle and select where the LocalDB-installer should be saved.
* After this installer is downloaded, open it at the provided path. Install SQLLocalDB by using the installation wizard.