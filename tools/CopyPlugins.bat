@ECHO OFF

SET releaseMode=Debug
SET FlashDevelopPluginDir="..\lib\FlashDevelop\FlashDevelop\Bin\Debug\Plugins"

SET updaterPluginPath="..\src\PluginUpdater\bin\x86\%releaseMode%\SpaceportUpdaterPlugin.dll"
SET spaceportPluginPath="..\src\PluginSpaceport\bin\x86\%releaseMode%\SpaceportPlugin.dll"
SET pluginCommonPath="..\src\PluginCommon\bin\x86\%releaseMode%\PluginCommon.dll"
SET installerCorePath="..\src\InstallerCore\bin\x86\%releaseMode%\InstallerCore.dll"
SET ionicPath="..\lib\Ionic.Zip.dll"

ECHO "Copying all assemblies to FlashDevelop plugins folder in %releaseMode% mode, at %FlashDevelopPluginDir%"
ECHO.

"C:\windows\system32\xcopy" "%updaterPluginPath%" "%FlashDevelopPluginDir%" /Y /Q
"C:\windows\system32\xcopy" "%spaceportPluginPath%" "%FlashDevelopPluginDir%" /Y /Q
"C:\windows\system32\xcopy" "%pluginCommonPath%" "%FlashDevelopPluginDir%" /Y /Q
"C:\windows\system32\xcopy" "%installerCorePath%" "%FlashDevelopPluginDir%" /Y /Q
"C:\windows\system32\xcopy" "%ionicPath%" "%FlashDevelopPluginDir%" /Y /Q

ECHO.
ECHO Copying successfull!
pause;