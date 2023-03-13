#ifndef _ANALOGINPUT_H_
#define _ANALOGINPUT_H_
#include <avr/io.h>

// Define if not defined
#ifndef bool
	#define bool uint8_t
#endif
#ifndef true
	#define true 1
	#define false 0
#endif

#define ADC_OFFSET_CONSECUTIVE_EQUAL_READINGS 8

void init_analog_input (void);
void start_analog_conversion (void);
int16_t get_analog_input (void);

#endif /* _ANALOGINPUT_H_ */