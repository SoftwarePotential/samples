param(
    [IO.FileInfo] $file=$(Get-ChildItem -r SoftwarePotential.config), 
    [string][Parameter(Mandatory=$true)] $username, 
    [string][Parameter(Mandatory=$true)] $password)

if( -not (Test-Path $file) ) {
    throw "Please supply a valid SoftwarePotential.config file containing a username and password setting" 
}

function Edit-XmlFile( [string] $filepath, [scriptblock] $applyChanges) {
    $xml = New-Object xml
    $xml.Load( $filepath )

    $applyChanges.Invoke( $xml )

    $xml.Save( $filepath )
}

function Change-DotNetConfigurationAppSettingValue( [xml] $xml, [string] $key, [string] $value) {
    $addElement = $xml.configuration.appSettings.add | Where-Object { $_.key -eq "SoftwarePotential.LicenseManagement.Credentials.$key" }
    $addElement.SetAttribute('value', $value)
}

$changeUserNameAndPasswordValues = { param( [xml] $xml )
    Change-DotNetConfigurationAppSettingValue $xml "username" $username
    Change-DotNetConfigurationAppSettingValue $xml "password" $password
}

Edit-XmlFile $file $changeUserNameAndPasswordValues