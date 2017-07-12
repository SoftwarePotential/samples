param( $installPath, $toolsPath, $package, $project )

$ErrorActionPreference = "Stop"

function VerifyNoOtherSpAgentIsInstalled()
{
	$spAgentsAlreadyInstalledInProject = ( $project | Get-Package | Where-Object { $_.Id -like "Sp.Agent-*" -and $_.Id -ne $package.Id } )
	if($spAgentsAlreadyInstalledInProject -ne $null -and $spAgentsAlreadyInstalledInProject.Count -gt 0){
		throw "Installing this Package alongside the existing Sp.Agent Package(s) would be ambiguous. Please remove the clashing package(s): [$spAgentsAlreadyInstalledInProject] prior to Installing this Package"
	}
}

VerifyNoOtherSpAgentIsInstalled