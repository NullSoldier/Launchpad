;--------------------------------

!include "MUI.nsh"
!include "LogicLib.nsh"
!include "WordFunc.nsh"

;--------------------------------
SetCompress off
SetCompressor zlib
CRCCheck on
RequestExecutionLevel admin

; Flash develop install path
InstallDir "$PROGRAMFILES\FlashDevelop\"
InstallDirRegKey HKLM "Software\Launchpad" ""

!define VERSION "1.0.0"
!define UPDATEROOT "Update"
!define GRAPHIC_ROOT "Graphics"
!define SDK_PATH "$TOOLS\spaceport-sdk"

; NAme and window title
Name "Launchpad"
Caption "Launchpad Setup"
UninstallCaption "Launchpad Uninstaller"

; The output of this installer
OutFile "LaunchpadInstaller.exe"

;--------------------------------
; Interface Configuration

!define MUI_HEADERIMAGE
!define MUI_ABORTWARNING
!define MUI_COMPONENTSPAGE_SMALLDESC
!define MUI_HEADERIMAGE_BITMAP "Graphics\Banner.bmp"
!define MUI_WELCOMEFINISHPAGE_BITMAP "Graphics\Wizard.bmp"
!define MUI_UNWELCOMEFINISHPAGE_BITMAP "Graphics\Wizard.bmp"

;--------------------------------
; Pages

!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_LANGUAGE "English"

;--------------------------------


Section "Launchpad" Main

	SetOverwrite on
	SetOutPath "$INSTDIR"

	; Remove the old spaceport-SDK if it exists
	RMDir /r "Update\Tools\spaceport-sdk"
	File /r /x Update\Plugins /x Update/Data "Update\Tools"
	File /r /x Update\Tools /x Update/Data "Update\Plugins"
	
	IfFileExists "$INSTDIR\.local" +1 0
		SetOutPath "$LOCALAPPDATA\FlashDevelop"
	
	File /r /x Update\Plugins /x Update\Tools "Update\Data"
	
	; Create uninstaller and needed metadata.
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Launchpad" "DisplayName" "Launchpad"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Launchpad" "UninstallString" '"$INSTDIR\UninstallLaunchpad.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Launchpad" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Launchpad" "NoRepair" 1
  WriteUninstaller "UninstallLaunchpad.exe"

SectionEnd

Section "un.Launchpad" UninstMain

	RMDir /r "$INSTDIR\Tools\spaceport-sdk"
	Delete "$INSTDIR\Plugins\Launchpad.dll"
	Delete "$INSTDIR\UninstallLaunchpad.exe"
	
	IfFileExists "$INSTDIR\.local" Skip 0
		SetOutPath "$LOCALAPPDATA\FlashDevelop"
		RMDir /r "Data\Launchpad"
	
	Skip:

SectionEnd

Function GetFDInstDir

	Push $0
	ClearErrors
	ReadRegStr $0 HKLM Software\FlashDevelop ""
	IfErrors 0 +2
	StrCpy $0 "not_found"
	Exch $0

FunctionEnd

Function .onInit

	Call GetFDInstDir
	Pop $0
	${IF} $0 == "not_found"
			 MessageBox MB_OK|MB_ICONEXCLAMATION "You need to install FlashDevelop before you can install the Launchpad plugin for FlashDevelop"
			 Abort
	${EndIf}

FunctionEnd
