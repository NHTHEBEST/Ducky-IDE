@echo off

if "%1" == "clean" goto clean

:build
del *.obj *.exe *.netmodule *.dll 
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

link /libpath:.\ littleWire_util.obj unManaged_USB.obj micronucleus_lib.obj Managed_USB.obj  Flasher.netmodule /dll /out:Flasher.dll /ltcg /machine:X86 libusb.lib 

goto end

:clean
echo =================================
echo =============CLEAN===============
echo =================================
del *.obj *.exe *.netmodule *.dll 

:end
