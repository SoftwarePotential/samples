@echo off
SETLOCAL
set DotNetDir=%windir%\Microsoft.NET\Framework\v4.0.30319%
set MSBuildExe=%DotNetDir%\msbuild.exe

IF NOT EXIST "%DotNetDir%" GOTO :NODOTNET
IF NOT EXIST "%MSBuildExe%" GOTO :NODOTNET

%MSBuildExe% Sp.Samples.Agent.WpfApplicationWithInstaller/Sp.Samples.Agent.WpfApplication/Sp.Samples.Agent.WpfApplication.csproj /t:StripPermutationInfo
%MSBuildExe% Sp.Samples.IntegratingObfuscators\Sp.Samples.IntegratingObfuscators/Sp.Samples.IntegratingObfuscators.csproj /t:StripPermutationInfo
%MSBuildExe% Sp.Samples.IntegratingObfuscators\Sp.Samples.IntegratingObfuscators.Library/Sp.Samples.IntegratingObfuscators.Library.csproj /t:StripPermutationInfo

IF ERRORLEVEL 1 GOTO :ERROR

echo Sp.Samples.Agent.WpfApplication permutation configuration has been reset.
echo Sp.Samples.IntegratingObfuscators permutation configuration has been reset.
echo If you have your solution open in Visual Studio, please re-open it.
GOTO :EOF

:NODOTNET
ECHO Error: Could not locate MSBuild. Looked in %DotNetDir%
GOTO :EOF

:ERROR
ECHO ON
EXIT /b %ERRORLEVEL%

:EOF