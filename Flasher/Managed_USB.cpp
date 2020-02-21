#include "unManaged_USB.h"
#include "cs_stuff.h"
#using <mscorlib.dll>
using namespace System;


public delegate void CLRProgressDelegate(float progress, int stage);
public delegate void CLRTextDelegate(int code, int data1, int data2);

ref class Managed_USB
{
public:
    Managed_USB()
    {
        unm_usb = new UnManaged_USB();
        if (!unm_usb) {
            throw gcnew OutOfMemoryException();
        }
    }
    ~Managed_USB() {ShowDestruction();}
    !Managed_USB() {ShowDestruction();}

    bool Flash(unsigned char program[],int size,bool fastmode,int timeout,bool run)
    {
        return (bool)unm_usb->cpp_flash_dev(program, size, (int)fastmode, timeout, (int)run);
    }

	// event
	static event CLRProgressDelegate^ ProgressChanged;
	static void raiseProgressChangedEvent(float progress, int stage)
	{
		ProgressChanged(progress, stage);
	}
	static event CLRTextDelegate^ TextUpdate;
	static void raiseTextUpdateEvent(int code, int data1, int data2)
	{
		TextUpdate(code,data1,data2);
	}
    
private:
    void ShowDestruction()
    {
        if (unm_usb) {
            delete unm_usb;
        }
    }
    UnManaged_USB *unm_usb;
};

//extern void Micronucleus::Flasher::onUpdate(float val);
void flashevent(float value, int stage)
{
	Managed_USB::raiseProgressChangedEvent(value, stage);
	//Micronucleus::Flasher::onUpdate(value);
}
void mprintText(int code, int data1, int data2)
{
	Managed_USB::raiseTextUpdateEvent(code, data1, data2);
}