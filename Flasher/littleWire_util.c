#include "littleWire_util.h"

/* Delay in miliseconds */
void delay(unsigned int duration) {

    // use windows sleep api with milliseconds
    // * 2 to make it run at half speed, because windows seems to have some trouble with this...
    Sleep(duration * 2);

}
