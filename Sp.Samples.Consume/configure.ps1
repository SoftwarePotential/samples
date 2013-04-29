# * Copyright 2013 (c) Inish Technology Ventures Limited.  All rights reserved.
# This code is licensed under the BSD 3-Clause License included with this source
#
# ALSO SEE: https://github.com/SoftwarePotential/samples/wiki/License

// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
param(

	[string] $Username = $(Read-Host -prompt "Software Potential username (account@domain.com)"),
	[string] $password = $(Read-Host -prompt "Software Potential password")
)

$msbuildProperties=@("TestAppConfigUsername=$username")
$msbuildProperties=$msbuildProperties+"TestAppConfigPassword=$password"

$properties="/p:$([string]::Join(';',$msBuildProperties))"

./build build.proj -t CustomizeAppConfigs -add "$properties"