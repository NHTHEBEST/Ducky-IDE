struct cs_usb_dev_handle;
typedef struct cs_usb_dev_handle cs_usb_dev_handle;

typedef struct _cs_micronucleus_version {
  unsigned char major;
  unsigned char minor;
} cs_micronucleus_version;

typedef struct _cs_micronucleus {
  cs_usb_dev_handle *device;
  // general information about device
  cs_micronucleus_version version;
  unsigned int flash_size;  // programmable size (in bytes) of progmem
  unsigned int page_size;   // size (in bytes) of page
  unsigned int bootloader_start; // Start of the bootloader
  unsigned int pages;       // total number of pages to program
  unsigned int write_sleep; // milliseconds
  unsigned int erase_sleep; // milliseconds
  unsigned char signature1; // only used in protocol v2
  unsigned char signature2; // only used in protocol v2
} cs_micronucleus;

typedef void (*cs_micronucleus_callback)(float progress);