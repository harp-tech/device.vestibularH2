#ifndef _APP_IOS_AND_REGS_H_
#define _APP_IOS_AND_REGS_H_
#include "cpu.h"

void init_ios(void);
/************************************************************************/
/* Definition of input pins                                             */
/************************************************************************/
// STOP_SWITCH            Description: Emergency stop indication

#define read_STOP_SWITCH read_io(PORTB, 0)      // STOP_SWITCH

/************************************************************************/
/* Definition of output pins                                            */
/************************************************************************/
// MOTOR_ENABLE           Description: Enable the motor external driver
// MOTOR_PULSE            Description: Moves one step
// MOTOR_DIRECTION        Description: Sets the motor direction

/* MOTOR_ENABLE */
#define set_MOTOR_ENABLE clear_io(PORTC, 3)
#define clr_MOTOR_ENABLE set_io(PORTC, 3)
#define tgl_MOTOR_ENABLE toggle_io(PORTC, 3)
#define read_MOTOR_ENABLE read_io(PORTC, 3)

/* MOTOR_PULSE */
#define set_MOTOR_PULSE clear_io(PORTC, 0)
#define clr_MOTOR_PULSE set_io(PORTC, 0)
#define tgl_MOTOR_PULSE toggle_io(PORTC, 0)
#define read_MOTOR_PULSE read_io(PORTC, 0)

/* MOTOR_DIRECTION */
#define set_MOTOR_DIRECTION clear_io(PORTC, 6)
#define clr_MOTOR_DIRECTION set_io(PORTC, 6)
#define tgl_MOTOR_DIRECTION toggle_io(PORTC, 6)
#define read_MOTOR_DIRECTION read_io(PORTC, 6)


/************************************************************************/
/* Registers' structure                                                 */
/************************************************************************/
typedef struct
{
	uint8_t REG_CONTROL;
	int32_t REG_PULSES;
	uint16_t REG_NOMINAL_PULSE_INTERVAL;
	uint16_t REG_INITIAL_PULSE_INTERVAL;
	uint16_t REG_PULSE_STEP_INTERVAL;
	uint16_t REG_PULSE_PERIOD;
	int16_t REG_ENCODER;
	int16_t REG_ANALOG_INPUT;
	uint8_t REG_STOP_SWITCH;
	uint8_t REG_MOVING;
	int16_t REG_IMMEDIATE_PULSES;
} AppRegs;

/************************************************************************/
/* Registers' address                                                   */
/************************************************************************/
/* Registers */
#define ADD_REG_CONTROL                     32 // U8     Controls the device's modules.
#define ADD_REG_PULSES                      33 // I32    Sends the number of pulses written in this register and set the direction according to the number's signal.
#define ADD_REG_NOMINAL_PULSE_INTERVAL      34 // U16    Sets the motor's pulse interval when running at nominal speed.
#define ADD_REG_INITIAL_PULSE_INTERVAL      35 // U16    Sets the motor's maximum pulse interval, used as the first and last pulse interval of a rotation.
#define ADD_REG_PULSE_STEP_INTERVAL         36 // U16    Sets the acceleration. The pulse's interval is decreased by this value when accelerating and increased when de-accelerating.
#define ADD_REG_PULSE_PERIOD                37 // U16    Sets the period of the pulse.
#define ADD_REG_ENCODER                     38 // I16    Contains the reading of the quadrature encoder.
#define ADD_REG_ANALOG_INPUT                39 // I16    Contains the reading of the analog input.
#define ADD_REG_STOP_SWITCH                 40 // U8     Contains the state of the stop switch.
#define ADD_REG_MOVING                      41 // U8     Contains the state of the motor.
#define ADD_REG_IMMEDIATE_PULSES            42 // I16    Sets immediately the motor's pulse interval. The value's signal defines the direction.

/************************************************************************/
/* PWM Generator registers' memory limits                               */
/*                                                                      */
/* DON'T change the APP_REGS_ADD_MIN value !!!                          */
/* DON'T change these names !!!                                         */
/************************************************************************/
/* Memory limits */
#define APP_REGS_ADD_MIN                    0x20
#define APP_REGS_ADD_MAX                    0x2A
#define APP_NBYTES_OF_REG_BANK              21

/************************************************************************/
/* Registers' bits                                                      */
/************************************************************************/
#define B_ENABLE_MOTOR                     (1<<0)       // 
#define B_DISABLE_MOTOR                    (1<<1)       // 
#define B_ENABLE_ANALOG_IN                 (1<<2)       // 
#define B_DISABLE_ANALOG_IN                (1<<3)       // 
#define B_ENABLE_QUAD_ENCODER              (1<<4)       // 
#define B_DISABLE_QUAD_ENCODER             (1<<5)       // 
#define B_RESET_QUAD_ENCODER               (1<<6)       // 
#define B_STOP_SWITCH                      (1<<0)       // 
#define B_IS_MOVING                        (1<<0)       // 

#endif /* _APP_REGS_H_ */