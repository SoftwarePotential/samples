# http://stackoverflow.com/a/15403662/11635
$readmeUrl = "http://docs.softwarepotential.com/Configuration.Distributor-README.html"
$DTE.ItemOperations.Navigate($readmeUrl, [EnvDTE.vsNavigateOptions]::vsNavigateOptionsNewWindow)