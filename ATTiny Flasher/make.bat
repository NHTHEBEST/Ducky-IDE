@echo off
REM check if vs console
if "%VSCMD_VER%"=="" goto start
del *.obj *.exe *.netmodule
cl.exe /Zi /MD /c littleWire_util.c
echo =================================
cl.exe /Zi /MD /c micronucleus_lib.c
echo =================================
cl.exe /Zi /MD /c unManaged_USB.cpp
echo =================================
cl.exe /Zi /clr /c Managed_USB.cpp
echo =================================
csc /target:module /addmodule:Managed_USB.obj Flasher.cs /unsafe
echo =================================
REM link /libpath:.\ littleWire_util.obj unManaged_USB.obj micronucleus_lib.obj Managed_USB.obj  Flasher.netmodule /entry:Attiny.Flasher.Main /out:Flasher.exe /subsystem:console /ltcg /machine:X86 libusb.lib 
link /libpath:.\ littleWire_util.obj unManaged_USB.obj micronucleus_lib.obj Managed_USB.obj  Flasher.netmodule /dll /out:Flasher.dll /ltcg /machine:X86 libusb.lib 
echo =================================
echo ===========TESTING===============
echo =================================
REM Flasher.exe
goto end

:start
if exist "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\VC\Auxiliary\Build\vcvars32.bat" (
	cmd /k "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\VC\Auxiliary\Build\vcvars32.bat"
) else (
	cmd /k "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\Tools\VsDevCmd.bat"
)
:end
