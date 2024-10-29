#include "cpu.h"
#include "hwbp_core_types.h"
#include "app_ios_and_regs.h"
#include "app_funcs.h"
#include "hwbp_core.h"

#include "analog_input.h"

/************************************************************************/
/* Declare application registers                                        */
/************************************************************************/
extern AppRegs app_regs;

/************************************************************************/
/* Interrupts from Timers                                               */
/************************************************************************/
// ISR(TCC0_OVF_vect, ISR_NAKED)
// ISR(TCD0_OVF_vect, ISR_NAKED)
// ISR(TCE0_OVF_vect, ISR_NAKED)
// ISR(TCF0_OVF_vect, ISR_NAKED)
//
// ISR(TCC0_CCA_vect, ISR_NAKED)
// ISR(TCD0_CCA_vect, ISR_NAKED)
// ISR(TCE0_CCA_vect, ISR_NAKED)
// ISR(TCF0_CCA_vect, ISR_NAKED)
//
// ISR(TCD1_OVF_vect, ISR_NAKED)
//
// ISR(TCD1_CCA_vect, ISR_NAKED)

/************************************************************************/
/* STOP                                                                 */
/************************************************************************/
extern bool motor_is_running;

ISR(PORTB_INT0_vect, ISR_NAKED)
{
	if (read_STOP_SWITCH)
	{
		/* Update register and send event */
		app_regs.REG_STOP_SWITCH = 0;
		core_func_send_event(ADD_REG_STOP_SWITCH, true);
	}
	else
	{		
		/* Stop motor */
		timer_type0_stop(&TCC0);
		motor_is_running = false;
		
		/* Disable motor */
		set_MOTOR_ENABLE;
		
		/* Update register and send event */
		app_regs.REG_STOP_SWITCH = B_STOP_SWITCH;
		core_func_send_event(ADD_REG_STOP_SWITCH, true);
	}
	
	reti();
}

/************************************************************************/
/* ADC                                                                  */
/************************************************************************/
ISR(ADCA_CH0_vect, ISR_NAKED)
{
	app_regs.REG_ANALOG_INPUT = get_analog_input();
	core_func_send_event(ADD_REG_ANALOG_INPUT, false);
	
	reti();
}

/************************************************************************/
/* EXTERNAL MOTOR CONTROL                                               */
/************************************************************************/
bool external_control_first_byte = true;
int16_t motor_pulse_interval;

ISR(USARTD0_RXC_vect, ISR_NAKED)
{
	if (external_control_first_byte)
	{
		external_control_first_byte = false;
		
		motor_pulse_interval = USARTD0_DATA;
		
		timer_type0_enable(&TCD0, TIMER_PRESCALER_DIV64, 100, INT_LEVEL_LOW);	// 200 us
	}
	else
	{
		external_control_first_byte = true;
		
		int16_t temp = USARTD0_DATA;
		
		motor_pulse_interval |= (temp << 8) & 0xFF00;
		
		timer_type0_stop(&TCD0);
		
		app_regs.REG_ANALOG_INPUT = motor_pulse_interval;
		core_func_send_event(ADD_REG_ANALOG_INPUT, true);
		
		app_write_REG_IMMEDIATE_PULSES(&motor_pulse_interval);
	}
	
	reti();
}


ISR(TCD0_OVF_vect, ISR_NAKED)
{
	external_control_first_byte = true;
	
	timer_type0_stop(&TCD0);
	
	reti();
}