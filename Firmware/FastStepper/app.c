#include "hwbp_core.h"
#include "hwbp_core_regs.h"
#include "hwbp_core_types.h"

#include "app.h"
#include "app_funcs.h"
#include "app_ios_and_regs.h"

#include "analog_input.h"
#include "encoder.h"
#include "stepper_motor.h"

#define F_CPU 32000000
#include <util/delay.h>

/************************************************************************/
/* Declare application registers                                        */
/************************************************************************/
extern AppRegs app_regs;
extern uint8_t app_regs_type[];
extern uint16_t app_regs_n_elements[];
extern uint8_t *app_regs_pointer[];
extern void (*app_func_rd_pointer[])(void);
extern bool (*app_func_wr_pointer[])(void*);

/************************************************************************/
/* Initialize app                                                       */
/************************************************************************/
static const uint8_t default_device_name[] = "FastStepper";

void hwbp_app_initialize(void)
{
    /* Define versions */
    uint8_t hwH = 1;
    uint8_t hwL = 0;
    uint8_t fwH = 1;
    uint8_t fwL = 0;
    uint8_t ass = 0;
    
   	/* Start core */
    core_func_start_core(
        2120,
        hwH, hwL,
        fwH, fwL,
        ass,
        (uint8_t*)(&app_regs),
        APP_NBYTES_OF_REG_BANK,
        APP_REGS_ADD_MAX - APP_REGS_ADD_MIN + 1,
        default_device_name,
        false,	// The device is not able to repeat the harp timestamp clock
        false,	// The device is not able to generate the harp timestamp clock
        3		// Default timestamp offset
    );
}

/************************************************************************/
/* Handle if a catastrophic error occur                                 */
/************************************************************************/
void core_callback_catastrophic_error_detected(void)
{
	/* Stop motor */
	timer_type0_stop(&TCC0);
	
	/* Disable motor */
	set_MOTOR_ENABLE;
}

/************************************************************************/
/* User functions                                                       */
/************************************************************************/
/* Add your functions here or load external functions if needed */

/************************************************************************/
/* Initialization Callbacks                                             */
/************************************************************************/
void core_callback_define_clock_default(void)
{
	/* Device don't have clock input or output */
}

void core_callback_initialize_hardware(void)
{
	
	/* Initialize IOs */
	/* Don't delete this function!!! */
	init_ios();
	
	/* Initialize ADC */
	init_analog_input();
	
	/* Initialize encoder */
	init_quadrature_encoder();
	
	/* Initialize serial with 100 KHz */
	uint16_t BSEL = 19;
	int8_t BSCALE = 0;
		
	USARTD0_CTRLC = USART_CMODE_ASYNCHRONOUS_gc | USART_PMODE_DISABLED_gc | USART_CHSIZE_8BIT_gc;
	USARTD0_BAUDCTRLA = *((uint8_t*)&BSEL);
	USARTD0_BAUDCTRLB = (*(1+(uint8_t*)&BSEL) & 0x0F) | ((BSCALE<<4) & 0xF0);
	USARTD0_CTRLB = USART_RXEN_bm;
	USARTD0_CTRLA |= (INT_LEVEL_LOW << 4);
}

void core_callback_reset_registers(void)
{
	/* Initialize registers */
	app_regs.REG_CONTROL = 0;
	app_regs.REG_PULSES = 0;
	app_regs.REG_NOMINAL_PULSE_INTERVAL = 250;
	app_regs.REG_INITIAL_PULSE_INTERVAL = 2000;
	app_regs.REG_PULSE_STEP_INTERVAL = 10;
	app_regs.REG_PULSE_PERIOD = 50;
}

void core_callback_registers_were_reinitialized(void)
{
	/* Write register that have effect on other zones of the code */
	app_write_REG_CONTROL(&app_regs.REG_CONTROL);
	app_write_REG_NOMINAL_PULSE_INTERVAL(&app_regs.REG_NOMINAL_PULSE_INTERVAL);
	app_write_REG_INITIAL_PULSE_INTERVAL(&app_regs.REG_INITIAL_PULSE_INTERVAL);
	app_write_REG_PULSE_STEP_INTERVAL(&app_regs.REG_PULSE_STEP_INTERVAL);
	app_write_REG_PULSE_PERIOD(&app_regs.REG_PULSE_PERIOD);
	
	/* Read external states */
	app_read_REG_STOP_SWITCH();
}

/************************************************************************/
/* Callbacks: Visualization                                             */
/************************************************************************/
void core_callback_visualen_to_on(void)
{
	/* Update visual indicators */
	
}

void core_callback_visualen_to_off(void)
{
	/* Clear all the enabled indicators */
	
}

/************************************************************************/
/* Callbacks: Change on the operation mode                              */
/************************************************************************/
void core_callback_device_to_standby(void)
{
	/* Disables motor when goes to Standby Mode */
	uint8_t reg = B_DISABLE_MOTOR;
	app_write_REG_CONTROL(&reg);
}
void core_callback_device_to_active(void) {}
void core_callback_device_to_enchanced_active(void) {}
void core_callback_device_to_speed(void) {}

/************************************************************************/
/* Callbacks: 1 ms timer                                                */
/************************************************************************/
int16_t quadrature_previous_value = 0;

extern bool send_motor_stopped_notification;

void core_callback_t_before_exec(void)
{
	/* Read ADC */
	if (app_regs.REG_CONTROL & B_ENABLE_ANALOG_IN)
	{
		core_func_mark_user_timestamp();
		start_analog_conversion();
	}
	
	/* Read quadrature encoder */
	app_regs.REG_ENCODER = get_quadrature_encoder();
		
	if (app_regs.REG_ENCODER != quadrature_previous_value)
	{
		if (app_regs.REG_CONTROL & B_ENABLE_QUAD_ENCODER)
		{
			core_func_send_event(ADD_REG_ENCODER, true);
		}
	}
		
	quadrature_previous_value = app_regs.REG_ENCODER;
	
	/* Notify that motor is stopped */
	if (send_motor_stopped_notification)
	{		
		send_motor_stopped_notification = false;
		
		app_regs.REG_MOVING = 0;
		core_func_send_event(ADD_REG_MOVING, true);
	}
}
void core_callback_t_after_exec(void) {}
void core_callback_t_new_second(void) {}

extern bool reg_control_was_updated;
extern uint8_t temporary_reg_control;

void core_callback_t_500us(void)
{
	/* Update REG_CONTROL with the temporary register */
	/* Writing to a register happens before this function (core_callback_t_500us) */
	if (reg_control_was_updated)
	{
		reg_control_was_updated = false;
		
		app_regs.REG_CONTROL = temporary_reg_control;
	}
}

int32_t user_requested_steps = 0;

void core_callback_t_1ms(void)
{
	if ((app_regs.REG_CONTROL & B_ENABLE_MOTOR) == false)
	{
		/* Disable medium and high level interrupts */
		/* Medium are enough but we can win some precious cpu time here */
		PMIC_CTRL = PMIC_RREN_bm | PMIC_LOLVLEN_bm;
		
		/* Stop motor */
		stop_rotation();
		
		/* Re-enable all interrupt levels */
		PMIC_CTRL = PMIC_RREN_bm | PMIC_LOLVLEN_bm | PMIC_MEDLVLEN_bm | PMIC_HILVLEN_bm;
	}
	
	if (user_requested_steps != 0)
	{	
		/* Disable medium and high level interrupts */
		/* Medium are enough but we can win some precious cpu time here */
		PMIC_CTRL = PMIC_RREN_bm | PMIC_LOLVLEN_bm;
		
		/* Update steps with the user request */
		user_requested_steps = user_sent_request(user_requested_steps);
		
		/* Re-enable all interrupt levels */
		PMIC_CTRL = PMIC_RREN_bm | PMIC_LOLVLEN_bm | PMIC_MEDLVLEN_bm | PMIC_HILVLEN_bm;
	}
}

/************************************************************************/
/* Callbacks: clock control                                             */
/************************************************************************/
void core_callback_clock_to_repeater(void) {}
void core_callback_clock_to_generator(void) {}
void core_callback_clock_to_unlock(void) {}
void core_callback_clock_to_lock(void) {}

/************************************************************************/
/* Callbacks: uart control                                              */
/************************************************************************/
void core_callback_uart_rx_before_exec(void) {}
void core_callback_uart_rx_after_exec(void) {}
void core_callback_uart_tx_before_exec(void) {}
void core_callback_uart_tx_after_exec(void) {}
void core_callback_uart_cts_before_exec(void) {}
void core_callback_uart_cts_after_exec(void) {}

/************************************************************************/
/* Callbacks: Read app register                                         */
/************************************************************************/
bool core_read_app_register(uint8_t add, uint8_t type)
{
	/* Check if it will not access forbidden memory */
	if (add < APP_REGS_ADD_MIN || add > APP_REGS_ADD_MAX)
		return false;
	
	/* Check if type matches */
	if (app_regs_type[add-APP_REGS_ADD_MIN] != type)
		return false;
	
	/* Receive data */
	(*app_func_rd_pointer[add-APP_REGS_ADD_MIN])();	

	/* Return success */
	return true;
}

/************************************************************************/
/* Callbacks: Write app register                                        */
/************************************************************************/
bool core_write_app_register(uint8_t add, uint8_t type, uint8_t * content, uint16_t n_elements)
{
	/* Check if it will not access forbidden memory */
	if (add < APP_REGS_ADD_MIN || add > APP_REGS_ADD_MAX)
		return false;
	
	/* Check if type matches */
	if (app_regs_type[add-APP_REGS_ADD_MIN] != type)
		return false;

	/* Check if the number of elements matches */
	if (app_regs_n_elements[add-APP_REGS_ADD_MIN] != n_elements)
		return false;

	/* Process data and return false if write is not allowed or contains errors */
	return (*app_func_wr_pointer[add-APP_REGS_ADD_MIN])(content);
}