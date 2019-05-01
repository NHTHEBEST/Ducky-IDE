#include "unManaged_USB.h"
#include <time.h>
#include <windows.h>

static int parseRaw(unsigned char *datain, unsigned char *data_buffer, int *start_address, int *end_address, size_t size);
static void printProgress(float progress);
static void printText(int code,int data1, int data2);
unsigned char dataBuffer[65792];
int flashstage = 0;
int UnManaged_USB::cpp_flash_dev(unsigned char * program, size_t p_size, int fastmode, int timeout, int run)
{
    

    micronucleus *my_device = NULL;

	printText(1, timeout, 0);
	printProgress(0.0f);


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
		printText(2, 0, 0);
        return 0;
    }

	printText(3, 0, 0);
	printProgress(1.0f);
    if(!fastmode){
        float wait = 0.0f;
        while(wait < 250){
            wait += 50.0f;
            delay(50);
        }
    }

	printText(4, my_device->version.major, my_device->version.minor);
	if (my_device->signature1) printText(5, (int)my_device->signature1, (int)my_device->signature2);
    printText(6, my_device->flash_size,0);
    printText(7, my_device->write_sleep,0);
	printText(8, my_device->pages,my_device->page_size);
	printText(9, my_device->erase_sleep,0);

    int startAddress = 1, endAddress = 0;

    memset(dataBuffer, 0xFF, sizeof(dataBuffer));

    parseRaw(program ,dataBuffer,&startAddress, &endAddress, p_size);// change so that data gose in

    if (startAddress >= endAddress){
		printText(10, 0, 0);
        return 0;
    }
    if (endAddress > my_device->flash_size){
		printText(11, endAddress - my_device->flash_size,0);
        return 0;
    }

	printText(12,0,0);
	flashstage++;
    int res = micronucleus_eraseFlash(my_device, printProgress);

    if (res == 1) { // erase disconnection bug workaround
        my_device = NULL;

        delay(250);

        int deciseconds_till_reconnect_notice = 50; // notice after 5 seconds
        while (my_device == NULL) {
        delay(100);
        my_device = micronucleus_connect(fastmode);
        deciseconds_till_reconnect_notice -= 1;

        if (deciseconds_till_reconnect_notice == 0) {
			printText(13, 0, 0);
        }
    }
		printText(14, 0, 0);
    } else if (res != 0) {
		printText(15, res,0);
        return 0;
    }
	printText(16, 0, 0);
	flashstage++;
    res = micronucleus_writeFlash(my_device, endAddress, dataBuffer, printProgress);
    if (res != 0) {
		printText(17, res, 0);
        return 0;
    }
    if(run){
		printText(18, 0, 0);
        delay(500);
        res = micronucleus_startApp(my_device);
        if (res != 0) {
			printText(19, res, 0);
            return 0;
        }
    }
	printProgress(0);
    //printf(">> Micronucleus done. Thank you!\n");
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
extern void flashevent(float value, int stage);
static void printProgress(float progress){
	/*  
	// progres exeption
    progress+=0.01f;
    progress *= 100.0f;
    int p = static_cast<int>(progress);
    char c = '\r';
    if (p==100)c = '\n';
    printf("%3d%%%c", p, c);
	//*/
    // rasing mcpp event
	flashevent(progress, flashstage);
}
extern void mprintText(int code, int data1, int data2);
static void printText(int code, int data1, int data2)
{
	mprintText(code,data1,data2);
}