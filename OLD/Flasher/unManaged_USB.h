#include <stdio.h>
#include <windows.h>

extern "C"{
    #include "micronucleus_lib.h"
    #include "littleWire_util.h"
}

class UnManaged_USB
{
public:
    int UnManaged_USB::cpp_flash_dev(unsigned char * program, size_t p_size, int fastmode, int timeout, int run);
};