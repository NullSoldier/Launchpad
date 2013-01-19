@ECHO OFF

SET workingDirectory=%1
IF NOT "%~1"=="" (
	ECHO Switching working directory to %workingDirectory%
	CD "%workingDirectory%"
)

SET sourceDir=..\src\Installer\bin\Debug\
SET outputDir="..\src\Installer\bin\Debug\PluginInstallerMerged.exe"
SET finalOutputDir=..\lib\FlashDevelop\FlashDevelop\Bin\Debug\Data\Spaceport\tools\PluginInstaller.exe
SET mergeFile1="%sourceDir%PluginInstaller.exe"
SET mergeFile2="%sourceDir%Ionic.Zip.dll"
SET mergeFile3="%sourceDir%InstallerCore.dll"
SET mergeFile4="%sourceDir%log4net.dll"

ECHO Producing merged file from..
ECHO %mergeFile1%
ECHO %mergeFile2%
ECHO %mergeFile3%
ECHO %mergeFile4%

ILMerge /target:winexe /out:%outputPath%  %mergeFile1% %mergeFile2% %mergeFile3% %mergeFile4%

ECHO.
ECHO Preparing to copy merged output from %outputDir% to %finalOutputDir%
ECHO F|"C:\windows\system32\xcopy" %outputDir% %finalOutputDir%  /Y /Q

ECHO.
ECHO Copying successfull!
PAUSE;