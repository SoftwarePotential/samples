# http://stackoverflow.com/a/15403662/11635
$readmeUrl = "https://support.softwarepotential.com/hc/en-us/articles/115001365849--SingleUser-Configuration-README"
$DTE.ItemOperations.Navigate($readmeUrl, [EnvDTE.vsNavigateOptions]::vsNavigateOptionsNewWindow)