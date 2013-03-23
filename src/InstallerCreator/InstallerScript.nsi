;--------------------------------

!include "MUI.nsh"
!include "LogicLib.nsh"
!include "WordFunc.nsh"

;--------------------------------

; Flash develop install path
InstallDir "$PROGRAMFILES\FlashDevelop\"
InstallDirRegKey HKLM "Software\FlashDevelop" ""

!define VERSION "1.0.0"
!define UPDATEROOT "Update"
!define GRAPHIC_ROOT "Graphics"
!define SDK_PATH "$TOOLS\spaceport-sdk"

; NAme and window title
Name "Launchpad ${VERSION}"
Caption "Launchpad ${VERSION} Setup"
UninstallCaption "Launchpad ${VERSION} Uninstaller"

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
    
    File /r /x Update\Plugins "Update\Tools"
	File /r /x Update\Tools "Update\Plugins"

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
