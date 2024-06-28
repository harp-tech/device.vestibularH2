#include <avr/io.h>
#include "hwbp_core_types.h"
#include "app_ios_and_regs.h"

/************************************************************************/
/* Configure and initialize IOs                                         */
/************************************************************************/
void init_ios(void)
{	/* Configure input pins */
	io_pin2in(&PORTB, 0, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // STOP_SWITCH
	io_pin2in(&PORTD, 2, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // RX

	/* Configure input interrupts */
	io_set_int(&PORTB, INT_LEVEL_LOW, 0, (1<<0), false);                 // STOP_SWITCH

	/* Configure output pins */
	io_pin2out(&PORTC, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // MOTOR_ENABLE
	io_pin2out(&PORTC, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // MOTOR_PULSE
	io_pin2out(&PORTC, 6, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // MOTOR_DIRECTION

	/* Initialize output pins */
	clr_MOTOR_ENABLE;
	clr_MOTOR_PULSE;
	clr_MOTOR_DIRECTION;
}

/************************************************************************/
/* Registers' stuff                                                     */
/************************************************************************/
AppRegs app_regs;

uint8_t app_regs_type[] = {
	TYPE_U8,
	TYPE_I32,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_U16,
	TYPE_I16,
	TYPE_I16,
	TYPE_U8,
	TYPE_U8,
	TYPE_I16
};

uint16_t app_regs_n_elements[] = {
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1
};

uint8_t *app_regs_pointer[] = {
	(uint8_t*)(&app_regs.REG_CONTROL),
	(uint8_t*)(&app_regs.REG_PULSES),
	(uint8_t*)(&app_regs.REG_NOMINAL_PULSE_INTERVAL),
	(uint8_t*)(&app_regs.REG_INITIAL_PULSE_INTERVAL),
	(uint8_t*)(&app_regs.REG_PULSE_STEP_INTERVAL),
	(uint8_t*)(&app_regs.REG_PULSE_PERIOD),
	(uint8_t*)(&app_regs.REG_ENCODER),
	(uint8_t*)(&app_regs.REG_ANALOG_INPUT),
	(uint8_t*)(&app_regs.REG_STOP_SWITCH),
	(uint8_t*)(&app_regs.REG_MOVING),
	(uint8_t*)(&app_regs.REG_IMMEDIATE_PULSES)
};