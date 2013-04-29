# * Copyright 2013 (c) Inish Technology Ventures Limited.  All rights reserved.
# This code is licensed under the BSD 3-Clause License included with this source
#
# ALSO SEE: https://github.com/SoftwarePotential/samples/wiki/License
param(
	[string] $solution="build.proj",
	[string] $targets="Build",
	[string] $additionalMsBuildArgs="",
	[string] $configuration="Debug", 
	[string] $verbosity="n", 
	[string] $fileVerbosity="d",
	[string] $logFile="build.log"
)

function warn( [string]$message) {
	Write-Host "$message" -BackgroundColor Yellow
}

$msbuildProperties=@("Configuration=$configuration")

$msbuild="$env:windir\Microsoft.NET\Framework\v4.0.30319\MSBuild"
$properties="/p:$([string]::Join(';',$msBuildProperties))"

warn "Starting at $([datetime]::Now)" 
$parameters=@("$solution","/v:$verbosity","/m","/t:$targets","/fl","/flp:LogFile=$logFile;Verbosity=$fileVerbosity",$properties,$additionalMsBuildArgs)
Write-Host "Running $msbuild $parameters"
. $msbuild @parameters
