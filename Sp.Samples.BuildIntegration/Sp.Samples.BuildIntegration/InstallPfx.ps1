﻿<#
* Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
*
* This code is licensed under the BSD 3-Clause License included with this source
*
* ALSO SEE: https://github.com/SoftwarePotential/samples/wiki/License
#>

$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
$pfx = Join-Path $scriptPath "Sp.Samples.IntegratingObfuscators\key.pfx"
$password = "SpSample"
$keyContainerName = "SpSampleCspContainer"

$PWD

$cert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2
$cert.Import($pfx, $password, [System.Security.Cryptography.X509Certificates.X509KeyStorageFlags]::Exportable)
$exportPrivateKeyInformation = $true
$certXml = $cert.PrivateKey.ToXmlString($exportPrivateKeyInformation)

$csp = New-Object System.Security.Cryptography.CspParameters
$csp.KeyContainerName = $keyContainerName
$csp.Flags = [System.Security.Cryptography.CspProviderFlags]::UseMachineKeyStore -bor [System.Security.Cryptography.CspProviderFlags]::NoPrompt # -bor is biwise or
$csp.KeyNumber = [System.Security.Cryptography.KeyNumber]::Signature

$rsa = New-Object System.Security.Cryptography.RSACryptoServiceProvider $csp
$rsa.FromXmlString($certXml)
$rsa.Clear()

Write-Output "Sucesfully imported $pfx into StrongName CSP store"