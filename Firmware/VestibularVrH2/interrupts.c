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
ISR(USARTD0_RXC_vect, ISR_NAKED)
{
	app_regs.REG_ANALOG_INPUT = USARTD0_DATA;
	
	core_func_send_event(ADD_REG_ANALOG_INPUT, true);
	
	reti();
}