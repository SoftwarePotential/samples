# http://stackoverflow.com/a/15403662/11635
$readmeUrl = "http://docs.softwarepotential.com/Configuration.Local.SingleUser-README.html"
$DTE.ItemOperations.Navigate($readmeUrl, [EnvDTE.vsNavigateOptions]::vsNavigateOptionsNewWindow)