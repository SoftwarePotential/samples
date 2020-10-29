# * Copyright 2013-2021 (c) Inish Technology Ventures Limited.  All rights reserved.
# This code is licensed under the BSD 3-Clause License included with this source
#
# ALSO SEE: https://github.com/SoftwarePotential/samples/wiki/License

param(

	[string] $clientId = $(Read-Host -prompt "Application ClientId"),
	[string] $clientSecret = $(Read-Host -prompt "Application Client Secret"),
	[string] $scope = $(Read-Host -prompt "Application Scope(s)"),
	[string] $authority = $(Read-Host -prompt "Software Potential Authority"),
	[string] $baseUrl = $(Read-Host -prompt "Software Potential BaseUrl")
)

$msbuildProperties=@("TestAppConfigClientId=$clientid")
$msbuildProperties=$msbuildProperties+"TestAppConfigClientSecret=$clientSecret"
$msbuildProperties=$msbuildProperties+"TestAppConfigScope=$scope"
$msbuildProperties=$msbuildProperties+"TestAppConfigauthority=$authority"
$msbuildProperties=$msbuildProperties+"TestAppConfigBaseUrl=$baseUrl"

$properties="/p:$([string]::Join(';',$msBuildProperties))"

./build build.proj -t CustomizeAppConfigs -add "$properties"