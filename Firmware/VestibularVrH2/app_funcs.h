#ifndef _APP_FUNCTIONS_H_
#define _APP_FUNCTIONS_H_
#include <avr/io.h>


/************************************************************************/
/* Define if not defined                                                */
/************************************************************************/
#ifndef bool
	#define bool uint8_t
#endif
#ifndef true
	#define true 1
#endif
#ifndef false
	#define false 0
#endif


/************************************************************************/
/* Prototypes                                                           */
/************************************************************************/
void app_read_REG_CONTROL(void);
void app_read_REG_PULSES(void);
void app_read_REG_NOMINAL_PULSE_INTERVAL(void);
void app_read_REG_INITIAL_PULSE_INTERVAL(void);
void app_read_REG_PULSE_STEP_INTERVAL(void);
void app_read_REG_PULSE_PERIOD(void);
void app_read_REG_ENCODER(void);
void app_read_REG_ANALOG_INPUT(void);
void app_read_REG_STOP_SWITCH(void);
void app_read_REG_MOVING(void);
void app_read_REG_IMMEDIATE_PULSES(void);


bool app_write_REG_CONTROL(void *a);
bool app_write_REG_PULSES(void *a);
bool app_write_REG_NOMINAL_PULSE_INTERVAL(void *a);
bool app_write_REG_INITIAL_PULSE_INTERVAL(void *a);
bool app_write_REG_PULSE_STEP_INTERVAL(void *a);
bool app_write_REG_PULSE_PERIOD(void *a);
bool app_write_REG_ENCODER(void *a);
bool app_write_REG_ANALOG_INPUT(void *a);
bool app_write_REG_STOP_SWITCH(void *a);
bool app_write_REG_MOVING(void *a);
bool app_write_REG_IMMEDIATE_PULSES(void *a);


#endif /* _APP_FUNCTIONS_H_ */