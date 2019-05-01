#include "unManaged_USB.h"
#include <time.h>
#include <windows.h>

static int parseRaw(unsigned char *datain, unsigned char *data_buffer, int *start_address, int *end_address, size_t size);
static void printProgress(float progress);
unsigned char dataBuffer[65792];
int UnManaged_USB::cpp_flash_dev(unsigned char * program, size_t p_size, int fastmode, int timeout, int run)
{
    
    //printf("program : %x\n%x%x%x\n\n", program, program[0],program[1],program[2]);

    micronucleus *my_device = NULL;

    printf("waiting for %d sec\n", timeout);
    printf("> Please plug in the device ... \n");
    printf("> Press CTRL+C to terminate the program.\n");



    time_t start_time, current_time;
    time(&start_time);

    while (my_device == NULL){
        delay(100);
        my_device = micronucleus_connect(fastmode);
        time(&current_time);
        if (timeout && start_time + timeout < current_time){
            break;
        }
    }

    if (my_device == NULL){
        printf("> Device search timed out\n");
        return 0;
    }

    printf("> Device found!\n");

    if(!fastmode){
        float wait = 0.0f;
        while(wait < 250){
            wait += 50.0f;
            delay(50);
        }
    }

    printf("> Device has firmware version %d.%d\n",my_device->version.major,my_device->version.minor);
    if (my_device->signature1) printf("> Device signature: 0x1e%02x%02x \n",(int)my_device->signature1,(int)my_device->signature2);
    printf("> Available space for user applications: %d bytes\n", my_device->flash_size);
    printf("> Suggested sleep time between sending pages: %ums\n", my_device->write_sleep);
    printf("> Whole page count: %d  page size: %d\n", my_device->pages,my_device->page_size);
    printf("> Erase function sleep duration: %dms\n", my_device->erase_sleep);

    int startAddress = 1, endAddress = 0;

    memset(dataBuffer, 0xFF, sizeof(dataBuffer));

    parseRaw(program ,dataBuffer,&startAddress, &endAddress, p_size);// change so that data gose in

    if (startAddress >= endAddress){
        printf("> No data in input file, exiting.\n");
        return 0;
    }
    if (endAddress > my_device->flash_size){
        printf("> Program file is %d bytes too big for the bootloader!\n", endAddress - my_device->flash_size);
        return 0;
    }

    printf("> Erasing the memory ...\n");

    int res = micronucleus_eraseFlash(my_device, printProgress);

    if (res == 1) { // erase disconnection bug workaround
        printf(">> Eep! Connection to device lost during erase! Not to worry\n");
        printf(">> This happens on some computers - reconnecting...\n");
        my_device = NULL;

        delay(250);

        int deciseconds_till_reconnect_notice = 50; // notice after 5 seconds
        while (my_device == NULL) {
        delay(100);
        my_device = micronucleus_connect(fastmode);
        deciseconds_till_reconnect_notice -= 1;

        if (deciseconds_till_reconnect_notice == 0) {
            printf(">> (!) Automatic reconnection not working. Unplug and reconnect\n");
            printf("   device usb connector, or reset it some other way to continue.\n");
        }
    }

    printf(">> Reconnected! Continuing upload sequence...\n");
    } else if (res != 0) {
        printf(">> Flash erase error %d has occured ...\n", res);
        printf(">> Please unplug the device and restart the program.\n");
        return 0;
    }
    printf("> Starting to upload ...\n");
    res = micronucleus_writeFlash(my_device, endAddress, dataBuffer, printProgress);
    if (res != 0) {
        printf(">> Flash write error %d has occured ...\n", res);
        printf(">> Please unplug the device and restart the program.\n");
        return 0;
    }
    if(run){
        printf("> Starting the user app ...\n");
        printf("> Warning Ducky Script will run in 5 sec\n");
        delay(500);
        res = micronucleus_startApp(my_device);
        if (res != 0) {
            printf(">> Run error %d has occured ...\n", res);
            printf(">> Please unplug the device and restart the program. \n");
            return 0;
        }
    }
    printf(">> Micronucleus done. Thank you!\n");
    return 1;
}



/******************************************************************************/
static int parseRaw(unsigned char *datain, unsigned char *data_buffer, int *start_address, int *end_address, size_t size) {

  *start_address = 0;
  *end_address = 0;
  memcpy(data_buffer,datain,size);
  *end_address = size-1;
  return 0;
}
/******************************************************************************/
static void printProgress(float progress){
    // progres exeption
    progress+=0.01f;
    progress *= 100.0f;
    int p = static_cast<int>(progress);
    char c = '\r';
    if (p==100)c = '\n';
    printf("%3d%%%c", p, c);
    return;
}
