# Starting the Readinizer
Execute the Readinizer.exe as an administrator. After a few seconds the Readinizer home screen opens.

# Screens
## Homescreen
![Homescreen](https://github.com/clma91/Readinizer/blob/master/Wiki/Images/UserManual/homescreen.PNG)
1. Provide the name of the domain that should be analyzed. If this field is left blank, the forest root domain is selected by default.
2. If this toggle is not activated, just the provided domain or the default domain is analyzed. If the toggle is activated all subdomains and treedomains of the provided domain are analyzed as well.
3. If this toggle is activated all reachable computers in the network are checked if the Sysmon service is running. Sysmon is a monitoring tool by Mark Russinovich. For more information check: [Sysmon](https://docs.microsoft.com/en-us/sysinternals/downloads/sysmon)
4. For security reasons it is recommended that the Sysmon service is run under a different name. This can prevent a potential attacker from detecting that Sysmon logs events. To check how many machines Sysmon is installed on, you can enter the changed service name here. Default value is Sysmon.
5. By clicking on this button the analysis will be performed with the above settings. First, information about domains, sites, organizations units and computers is loaded from the Active Directory. Then an attempt is made to contact a computer from each organization unit and read out a RSoP. A RSoP contains the active computer logging settings. These settings are then compared with the recommended settings and the result is displayed.

## Forest Result Screen
![Forest Result Screen](https://github.com/clma91/Readinizer/blob/master/Wiki/Images/UserManual/resultscreen.png)
1. The result reflects the structure of a forest. At the top is the forest root domain or the parent domain, beneath are the subdomains and tree domains.
2. The progressbar reflects the readiness of a domain or a group of identical security settings. The progressbar of the groups of identical security settings is calculated by matching settings. For the domain, the lowest value of its children is used.
3. The group of identical security settings can be expanded to show the organizational units that are members of that group.
4. This button opens a more detailed view for domains and groups of identical security settings.
5. This section lists all organizational units where no computer could be reached and therefore no RSoP was extracted.
6. With this button RSoPs can be inserted as XML files for the unanalyzed organizational units.
7. This section lists all domains that are members of the forest but could not be contacted.
8. By clicking this button you get an overview of the Sysmon installation rate in the network. This button is only showed if the corresponding toggle on the home screen was selected.

## Domain Result Screen
![Domain Result Screen](https://github.com/clma91/Readinizer/blob/master/Wiki/Images/UserManual/domainscreen.png)
1. The name of the domain that is displayed.
2. The number of correct groups of identical security settings shown in a pie chart.
3. The groups of identical security settings that match the recommended settings. A click on the name opens a more detailed overview of the group of identical security settings.
4. The groups of identical security settings that do not match the recommended settings. A click on the name opens a more detailed overview of the group of identical security settings.
5. This button brings you back to the forest result overview.

## Group of Identical Security Settings Result Screen
![Group of Identical Security Settings Result Screen](https://github.com/clma91/Readinizer/blob/master/Wiki/Images/UserManual/gissscreen.png)
1. The name/number of the group of identical security settings that is displayed.
2. The name of the setting.
3. The target value of the setting.
4. The current value of the setting.
5. The status of the setting, a green checkmark if matching, a red cross if there is no match, a orange exclamation mark if undefined.
6. A list of organization units that are member in this group of identical security settings. A click on the name opens a more detailed overview of the organization unit.
7. This button brings you back to the domain result overview.

## Organizational Unit Result Screen
![Organizational Unit Result Screen](https://github.com/clma91/Readinizer/blob/master/Wiki/Images/UserManual/ouscreen.png)
1. The name of the Organizational Unit that is displayed.
2. The name of the Group Policy Object which set this setting.
3. The name of the setting.
4. The target value of the setting.
5. The current value of the setting.
6. The status of the setting, a green checkmark if matching, a red cross if no there is match, a orange exclamation mark if undefined.
7. This button brings you back to the group of identical security settings result overview.

## Sysmon Result Screen
![Sysmon Result Screen](https://github.com/clma91/Readinizer/blob/master/Wiki/Images/UserManual/sysmonscreen.png)
1. The percentage of computers on the network on which Sysmon is installed is displayed in a pie chart.
2. List of computer on which sysmon is installed.
3. List of computer on which sysmon is not installed.
4. This button brings you back to the forest result overview.

## Navigationbar
![Navigationbar](https://github.com/clma91/Readinizer/blob/master/Wiki/Images/UserManual/navfile.png)
* Close: Terminates the application
* New Analysis: Truncates the database, prepares the Readinizer for a new analysis

## Export
![Export](https://github.com/clma91/Readinizer/blob/master/Wiki/Images/UserManual/navexport.png)
* Export Grouped Security Settings: Exports the collected and analyzed data to a JSON file to a specified path

## Help
![Help](https://github.com/clma91/Readinizer/blob/master/Wiki/Images/UserManual/navhelp.png)
* User Manual: Contains a link to this document
* Central Logging: Contains a link to a guide on how to implement central logging
* Sysmon: Contains a link to a guide on how to install Sysmon over an entire fleet through Group Policy Objects
* Optimized GPO: Contains a link to the recommend Group Policy File