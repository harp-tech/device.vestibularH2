#ifndef _STEPPER_MOTOR_H_
#define _STEPPER_MOTOR_H_
#include <avr/io.h>

// Define if not defined
#ifndef bool
	#define bool uint8_t
#endif
#ifndef true
	#define true 1
	#define false 0
#endif

void update_nominal_pulse_interval (uint16_t time_us);
void update_initial_pulse_interval (uint16_t time_us);
void update_pulse_step_interval (uint16_t time_us);
void update_pulse_period (uint16_t time_us);

void start_rotation (int32_t requested_steps);
void stop_rotation (void);

int32_t user_sent_request (int32_t requested_steps);

#endif /* _STEPPER_MOTOR_H_ */