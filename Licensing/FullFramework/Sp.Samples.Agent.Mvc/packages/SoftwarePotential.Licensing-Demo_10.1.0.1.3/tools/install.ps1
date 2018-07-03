# http://stackoverflow.com/a/15403662/11635
$readmeUrl = "https://support.softwarepotential.com/hc/en-us/articles/115001358849-Licensing-README"
$DTE.ItemOperations.Navigate($readmeUrl, [EnvDTE.vsNavigateOptions]::vsNavigateOptionsNewWindow)