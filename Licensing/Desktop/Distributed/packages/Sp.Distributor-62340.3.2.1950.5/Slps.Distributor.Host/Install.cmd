@echo OFF
SETLOCAL ENABLEEXTENSIONS
IF ERRORLEVEL 1 (
	ECHO ======================================================
	ECHO Command extensions are required to execute this script
	ECHO These are available in Windows NT4 and higher
	ECHO ======================================================
	GOTO :EXITERROR
)

REM Detect the version of windows being run and exit if lower than Server 2003 (5.2.3790)
REM See permission settings in the install function
For /f "tokens=2 delims=[]" %%G in ('ver') Do (set _version=%%G) 
For /f "tokens=2,3,4 delims=. " %%G in ('echo %_version%') Do (set _major=%%G& set _minor=%%H& set _build=%%I)
if "%_major%"=="5" if "%_minor%"=="1" goto UNSUPPORTEDVERSION

REM Detect if we are running escalated - cross-OS way of doing this is to read the LOCAL SERVICE account registry and detect an error
reg query "HKU\S-1-5-19" >NUL 2>&1
IF ERRORLEVEL 1 (
	ECHO =================================================
	ECHO This batch file requires administrator privileges
	ECHO =================================================
	GOTO :EXITERROR
)

SET PATH=%WINDIR%\system32;%PATH%
SET FRAMEWORKDIR="%WINDIR%\Microsoft.NET\Framework\v2.0.50727"
IF NOT EXIST "%FRAMEWORKDIR%\installutil.exe" (
	ECHO =======================================================================
	ECHO .NET Framework 2.0 not detected at %FRAMEWORKDIR% ^(requires .NET 3.5^)
	ECHO =======================================================================
	GOTO :EXITERROR
)

if "%1_" == "-stop_" (
	SET STOP=true
	GOTO :ARGS_OK
)
if "%1_" == "-start_" (
	SET START=true
	GOTO :ARGS_OK
)
if "%1_" == "-uninstall_" (
	SET STOP=true
	SET UNINSTALL=true
	GOTO :ARGS_OK
)
if "%1_" == "-install_" (
	SET STOP=true
	SET UNINSTALL=true
	SET INSTALL=true
	SET START=true
	GOTO :ARGS_OK
)

ECHO ===========================================
ECHO Usage is:
ECHO %0 -[stop ^| start ^| install ^| uninstall]
ECHO ===========================================
GOTO :EXITERROR

:ARGS_OK
SET SERVICE=Slps.Distributor.Host
SET LOGFILE=%0.log
SET ERRORLEVEL=
ECHO Starting %0 at %TIME% > %LOGFILE%

IF DEFINED STOP (
	ECHO.
	ECHO Stopping existing service...
	ECHO Stopping existing service... >> %LOGFILE% 2>&1
	REM is the service installed and running?
	FOR /F "tokens=4" %%A IN ('sc query %SERVICE% ^| find /i "STATE"') DO ( 
		IF NOT "%%A" == "STOPPED" (
			net stop %SERVICE% >> %LOGFILE% 2>&1
		)
	)
	ECHO ...stopped
	ECHO ...stopped >> %LOGFILE% 2>&1
)

IF DEFINED UNINSTALL (
	REM Note: Installutil.exe returns -1 on failure and even with cmdextensions enabled, %ERRORLEVEL% stays as 0
	REM thus we use NOT ERRORLEVEL 0 to detect if error occured
	ECHO.
	ECHO Uninstalling Services and Modules...
	ECHO Uninstalling Services and Modules... >> %LOGFILE% 2>&1
	sc query %SERVICE% > NUL
 	IF NOT ERRORLEVEL 1  (
 		"%FRAMEWORKDIR%\installutil.exe" -u Slps.Distributor.Host.exe  >> %LOGFILE% 2>&1
 		IF NOT ERRORLEVEL 0  (
			ECHO ==========================================================
 			ECHO Failed to uninstall Slps.Distributor.Host.exe. Continuing.
 			ECHO Failed to uninstall Slps.Distributor.Host.exe. Continuing. >> %LOGFILE% 2>&1
			ECHO ==========================================================
		)
 		"%FRAMEWORKDIR%\installutil.exe" -u Services\Slps.Distributor.Services.dll  >> %LOGFILE% 2>&1
 		IF NOT ERRORLEVEL 0  (
			ECHO =======================================================================
 			ECHO Failed to uninstall Services\Slps.Distributor.Services.dll. Continuing.
 			ECHO Failed to uninstall Services\Slps.Distributor.Services.dll. Continuing. >> %LOGFILE% 2>&1
			ECHO =======================================================================
		)
	)
	ECHO ...uninstalled.
	ECHO ...uninstalled. >> %LOGFILE% 2>&1
)

IF DEFINED INSTALL (
	REM Note: Installutil.exe returns -1 on failure and even with cmdextensions enabled, %ERRORLEVEL% stays as 0
	REM thus we use NOT ERRORLEVEL 0 to detect if error occured
	ECHO.
 	ECHO Installing Services and Modules...
	ECHO Installing Services and Modules... >> %LOGFILE% 2>&1
 	"%FRAMEWORKDIR%\installutil.exe" -i Slps.Distributor.Host.exe >> %LOGFILE% 2>&1
	IF NOT ERRORLEVEL 0 (
		ECHO ===========================================
 		ECHO Failed to install Slps.Distributor.Host.exe
		ECHO ===========================================
 		GOTO :EXITERROR
	)

 	"%FRAMEWORKDIR%\installutil.exe" -i Services\Slps.Distributor.Services.dll >> %LOGFILE% 2>&1
	IF NOT ERRORLEVEL 0 (
		ECHO ========================================================
 		ECHO Failed to install Services\Slps.Distributor.Services.dll
		ECHO ========================================================
 		GOTO :EXITERROR
	)

	ECHO ...installed.
	ECHO ...installed. >> %LOGFILE% 2>&1

	REM S-1-5-20 is the SID for NT Authority\Network Services 
	REM In order to avoid i18n issues, we want to express the grant in terms of a SID. 
	REM cacls.exe (which is also on XP) doesnt support specifying principals as SIDs. 
	REM Thus we have to use icacls.exe (even if that means that the Distributor Server is not OOTB easily installable on XP)
	ECHO.
	ECHO Setting Permissions...
	ECHO Setting Permissions... >> %LOGFILE% 2>&1
	icacls ./* /grant "*S-1-5-20:(W)" /c >> %LOGFILE% 2>&1
 	IF ERRORLEVEL 1  (
		ECHO ==============================================
 		ECHO Failed to set Network Service permissions on .
		ECHO ==============================================
		GOTO :EXITERROR
	)

	icacls Web /grant "*S-1-5-20:(W)" /c >> %LOGFILE% 2>&1
 	IF ERRORLEVEL 1  (
		ECHO ================================================
 		ECHO Failed to set Network Service permissions on Web
		ECHO ================================================
		GOTO :EXITERROR
	)

	icacls Services /grant "*S-1-5-20:(W)" /c >> %LOGFILE% 2>&1
 	IF ERRORLEVEL 1  (
		ECHO =====================================================
 		ECHO Failed to set Network Service permissions on Services
		ECHO =====================================================
		GOTO :EXITERROR
	)
	ECHO ...Permissions set.
	ECHO ...Permissions set. >> %LOGFILE% 2>&1
) 
 
IF DEFINED START (
	ECHO.
	ECHO Starting service...
	ECHO Starting service... >> %LOGFILE% 2>&1
	sc query %SERVICE% > NUL
 	IF NOT ERRORLEVEL 1  (
		REM Test if the service is installed and stopped
		FOR /F "tokens=4" %%A IN ('sc query %SERVICE% ^| find /i "STATE"') DO ( 
			IF NOT "%%A" == "RUNNING" (
				net start %SERVICE% >> %LOGFILE% 2>&1
			)
		)
	) ELSE (
		ECHO =======================================================
		ECHO %SERVICE% is not installed, so cannot start
		ECHO =======================================================
		GOTO :EXITERROR
	)
	ECHO ...started.
	ECHO ...started.  >> %LOGFILE% 2>&1
 )

GOTO :DONE

:UNSUPPORTEDVERSION
ECHO ======================================================
ECHO Unsupported OS Version
ECHO Minimum supported OS Version for this installation script is Server 2003 (5.2.3790)
ECHO ======================================================
GOTO :EXITERROR
	
:EXITERROR
ECHO Completed %0 with errors at %TIME% >> %LOGFILE%
ENDLOCAL
EXIT /B 1

:DONE
ECHO Completed %0 at %TIME% >> %LOGFILE%
ENDLOCAL
