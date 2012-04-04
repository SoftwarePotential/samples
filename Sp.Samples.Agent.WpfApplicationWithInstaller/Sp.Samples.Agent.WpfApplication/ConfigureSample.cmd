@echo off
SETLOCAL
set DotNetDir=%windir%\Microsoft.NET\Framework\v4.0.30319%
set MSBuildExe=%DotNetDir%\msbuild.exe

IF NOT EXIST "%DotNetDir%" GOTO :NODOTNET
IF NOT EXIST "%MSBuildExe%" GOTO :NODOTNET

IF [%1]==[] GOTO :NOPERMUTATIONID

%MSBuildExe% Sp.Samples.Agent.WpfApplication.csproj /t:ConfigurePermutationInfo /p:SlpsRuntimePermutationId=%1

IF ERRORLEVEL 1 GOTO :ERROR

echo Sp.Samples.Agent.WpfApplication has been configured for use with Software Potential permutation %1.
echo If you have your solution open in Visual Studio, please re-open it.
GOTO :EOF

:NOPERMUTATIONID
ECHO Usage: %0 ^<PermutationId^>
ECHO Example: %0 12a87dc3-09a4-ff31-8a22-235f87a09ce9
GOTO :ERROR

:NODOTNET
ECHO Error: Could not locate MSBuild. Looked in %DotNetDir%
GOTO :EOF

:ERROR
ECHO ON
EXIT /b %ERRORLEVEL%

:EOF
ECHO ON