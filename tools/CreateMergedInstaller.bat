@ECHO OFF

set workingDirectory=%1
IF NOT "%~1"=="" (
	ECHO Switching working directory to %workingDirectory%
	CD "%workingDirectory%"
)

SET sourceDir=..\src\Installer\bin\Debug\
SET mergeFile1="%sourceDir%PluginInstaller.exe"
SET mergeFile2="%sourceDir%Ionic.Zip.dll"
SET mergeFile3="%sourceDir%InstallerCore.dll"

ECHO Producing merged file from..
ECHO %mergeFile1%
ECHO %mergeFile2%
ECHO %mergeFile3%

SET outputPath="..\src\Installer\bin\Debug\PluginInstallerMerged.exe"
ILMerge /target:winexe /out:%outputPath%  %mergeFile1% %mergeFile2% %mergeFile3%

SET copyDestination=..\lib\FlashDevelop\FlashDevelop\Bin\Debug\Data\Spaceport\tools\PluginInstaller.exe

ECHO.
ECHO Preparing to copy merged output from %outputPath% to %copyDestination%
ECHO F|"C:\windows\system32\xcopy" %outputPath% %copyDestination%  /Y /Q

ECHO.
ECHO Copying successfull!