@ECHO OFF

SET workingDirectory=%1
IF NOT "%~1"=="" (
	ECHO Switching working directory to %workingDirectory%
	CD "%workingDirectory%"
)

SET releaseMode=Debug
SET FlashDevelopPluginDir="..\lib\FlashDevelop\FlashDevelop\Bin\Debug\Plugins"

SET spaceportPluginPath="..\src\PluginSpaceport\bin\x86\%releaseMode%\SpaceportPlugin.dll"
SET spaceportPluginDebugPath="..\src\PluginSpaceport\bin\x86\%releaseMode%\SpaceportPlugin.pdb"
SET installerCorePath="..\src\InstallerCore\bin\x86\%releaseMode%\InstallerCore.dll"
SET ionicPath="..\lib\Ionic.Zip.dll"
SET log4net="..\lib\log4net.dll"

ECHO "Copying all assemblies to FlashDevelop plugins folder in %releaseMode% mode, at %FlashDevelopPluginDir%"
ECHO.

"C:\windows\system32\xcopy" "%spaceportPluginPath%" "%FlashDevelopPluginDir%" /Y /Q
"C:\windows\system32\xcopy" "%spaceportPluginDebugPath%" "%FlashDevelopPluginDir%" /Y /Q
"C:\windows\system32\xcopy" "%installerCorePath%" "%FlashDevelopPluginDir%" /Y /Q
"C:\windows\system32\xcopy" "%ionicPath%" "%FlashDevelopPluginDir%" /Y /Q
"C:\windows\system32\xcopy" "%log4net%" "%FlashDevelopPluginDir%" /Y /Q

ECHO.
ECHO Copying successfull!