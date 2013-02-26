param(
    [IO.FileInfo] $file=$(Get-ChildItem -recurse SoftwarePotential.config), 
    [string][Parameter(Mandatory=$true)] $username, 
    [string][Parameter(Mandatory=$true)] $password)

if( -not (Test-Path $file) ) {
    throw "Please supply a valid SoftwarePotential.config file containing a username and password setting" 
}

function UseAndDispose { 
param(
    [System.IDisposable] $inputObject = $(throw "The parameter -inputObject is required."), 
    [ScriptBlock] $scriptBlock = $(throw "The parameter -scriptBlock is required.")
) 

    Try {
        & $scriptBlock
    } 
    Finally {
        if ($inputObject -ne $null) {
            if ($inputObject.psbase -eq $null) {
                $inputObject.Dispose()
            } else {
                $inputObject.psbase.Dispose()
            }
        }
    }
}

$sharedSecret = "12345"

[Reflection.Assembly]::LoadWithPartialName("System.Security") | Out-Null

function Encrypt-String($String, $passPhrase) {
    UseAndDispose ($aesAlg = new-Object System.Security.Cryptography.RijndaelManaged) {
        
        $salt = [Text.Encoding]::ASCII.GetBytes("o4865465sdK5c3")    
        $aesAlg.Key = (new-Object Security.Cryptography.Rfc2898DeriveBytes $sharedSecret, $salt).GetBytes($aesAlg.KeySize/8) #256/8    
        $encryptor = $aesAlg.CreateEncryptor($aesAlg.Key, $aesAlg.IV)

        UseAndDispose ($msEncrypt = new-Object IO.MemoryStream) {
            # Prepend the initialization vector
            $msEncrypt.Write( [BitConverter]::GetBytes($aesAlg.IV.Length), 0, 4)            
            $msEncrypt.Write($aesAlg.IV, 0, $aesAlg.IV.Length)
            UseAndDispose ($csEncrypt = new-Object Security.Cryptography.CryptoStream $msEncrypt, $encryptor, "Write") {
                UseAndDispose ($swEncrypt = new-Object IO.StreamWriter $csEncrypt) {
                    $swEncrypt.Write($String)
                }
                return [Convert]::ToBase64String($msEncrypt.ToArray())
            }
        }
    }
}

$encryptedPassword = Encrypt-String $password $sharedSecret

function Edit-XmlFile( [string] $filepath, [scriptblock] $applyChanges) {
    $xml = New-Object xml
    $xml.Load( $filepath )

    $applyChanges.Invoke( $xml )

    $xml.Save( $filepath )
}

function Change-DotNetConfigurationAppSettingValue( [xml] $xml, [string] $key, [string] $value) {
    $addElement = $xml.SelectSingleNode("/configuration/appSettings/add[@key='SoftwarePotential.LicenseManagement.Credentials.$key']")
    $addElement.SetAttribute('value', $value)
}

$changeUserNameAndPasswordValues = { param( [xml] $xml )
    Change-DotNetConfigurationAppSettingValue $xml "username" $username
    Change-DotNetConfigurationAppSettingValue $xml "password" $encryptedPassword
}

Edit-XmlFile $file $changeUserNameAndPasswordValues