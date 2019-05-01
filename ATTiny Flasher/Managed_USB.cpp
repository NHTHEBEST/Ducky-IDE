#include "unManaged_USB.h"
#include "cs_stuff.h"
#using <mscorlib.dll>
using namespace System;


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
    
private:
    void ShowDestruction()
    {
        if (unm_usb) {
            delete unm_usb;
        }
    }
    UnManaged_USB *unm_usb;
};