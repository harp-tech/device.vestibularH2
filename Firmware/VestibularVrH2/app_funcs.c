#include "app_funcs.h"
#include "app_ios_and_regs.h"
#include "hwbp_core.h"

#include "encoder.h"
#include "stepper_motor.h"

/************************************************************************/
/* Create pointers to functions                                         */
/************************************************************************/
extern AppRegs app_regs;

void (*app_func_rd_pointer[])(void) = {
	&app_read_REG_CONTROL,
	&app_read_REG_PULSES,
	&app_read_REG_NOMINAL_PULSE_INTERVAL,
	&app_read_REG_INITIAL_PULSE_INTERVAL,
	&app_read_REG_PULSE_STEP_INTERVAL,
	&app_read_REG_PULSE_PERIOD,
	&app_read_REG_ENCODER,
	&app_read_REG_ANALOG_INPUT,
	&app_read_REG_STOP_SWITCH,
	&app_read_REG_MOVING
};

bool (*app_func_wr_pointer[])(void*) = {
	&app_write_REG_CONTROL,
	&app_write_REG_PULSES,
	&app_write_REG_NOMINAL_PULSE_INTERVAL,
	&app_write_REG_INITIAL_PULSE_INTERVAL,
	&app_write_REG_PULSE_STEP_INTERVAL,
	&app_write_REG_PULSE_PERIOD,
	&app_write_REG_ENCODER,
	&app_write_REG_ANALOG_INPUT,
	&app_write_REG_STOP_SWITCH,
	&app_write_REG_MOVING
};


/************************************************************************/
/* REG_CONTROL                                                          */
/************************************************************************/
bool reg_control_was_updated = false;
uint8_t temporary_reg_control;

void app_read_REG_CONTROL(void)
{
	uint8_t temp = 0;
	
	if (app_regs.REG_CONTROL & B_ENABLE_MOTOR)
	{
		temp |= B_ENABLE_MOTOR;
	}
	else
	{
		temp |= B_DISABLE_MOTOR;
	}
	
	if (app_regs.REG_CONTROL & B_ENABLE_ANALOG_IN)
	{
		temp |= B_ENABLE_ANALOG_IN;
	}
	else
	{
		temp |= B_DISABLE_ANALOG_IN;
	}
	
	if (app_regs.REG_CONTROL & B_ENABLE_QUAD_ENCODER)
	{
		temp |= B_ENABLE_QUAD_ENCODER;
	}
	else
	{
		temp |= B_DISABLE_QUAD_ENCODER;
	}

	app_regs.REG_CONTROL = temp;
}

bool app_write_REG_CONTROL(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg & B_ENABLE_MOTOR)  { temporary_reg_control |=  B_ENABLE_MOTOR; temporary_reg_control &=  ~B_DISABLE_MOTOR; }
	if (reg & B_DISABLE_MOTOR) { temporary_reg_control &= ~B_ENABLE_MOTOR; temporary_reg_control |=   B_DISABLE_MOTOR; }
	
	if (reg & B_ENABLE_ANALOG_IN)  { temporary_reg_control |=  B_ENABLE_ANALOG_IN; temporary_reg_control &=  ~B_DISABLE_ANALOG_IN; }
	if (reg & B_DISABLE_ANALOG_IN) { temporary_reg_control &= ~B_ENABLE_ANALOG_IN; temporary_reg_control |=   B_DISABLE_ANALOG_IN; }
	
	if (reg & B_ENABLE_QUAD_ENCODER)  { temporary_reg_control |=  B_ENABLE_QUAD_ENCODER; temporary_reg_control &=  ~B_DISABLE_QUAD_ENCODER; }
	if (reg & B_DISABLE_QUAD_ENCODER) { temporary_reg_control &= ~B_ENABLE_QUAD_ENCODER; temporary_reg_control |=   B_DISABLE_QUAD_ENCODER; }
	
	if (reg & B_RESET_QUAD_ENCODER)
	{
		reset_quadrature_encoder();
	}
	
	if (temporary_reg_control & B_ENABLE_MOTOR)
	{
		set_MOTOR_ENABLE;
	}
	else
	{
		clr_MOTOR_ENABLE;
	}
	
	reg_control_was_updated = true;
	
	app_regs.REG_CONTROL = reg;
	return true;
}


/************************************************************************/
/* REG_PULSES                                                           */
/************************************************************************/
extern int32_t user_requested_steps;

void app_read_REG_PULSES(void)
{
	//app_regs.REG_CONTROL = 0;

}

bool app_write_REG_PULSES(void *a)
{
	int32_t reg = *((int32_t*)a);
	
	if (app_regs. REG_CONTROL & B_ENABLE_MOTOR)
	{
		user_requested_steps += reg;
	}

	app_regs.REG_PULSES = reg;
	return true;
}


/************************************************************************/
/* REG_NOMINAL_PULSE_INTERVAL                                           */
/************************************************************************/
void app_read_REG_NOMINAL_PULSE_INTERVAL(void)
{
	//app_regs.REG_NOMINAL_PULSE_INTERVAL = 0;

}

bool app_write_REG_NOMINAL_PULSE_INTERVAL(void *a)
{
	uint16_t reg = *((uint16_t*)a);
	
	if (reg < 200) return false;
	if (reg > 20000) return false;
	
	if (TCC0.CTRLA) return false;	

	update_nominal_pulse_interval(reg);
	
	app_regs.REG_NOMINAL_PULSE_INTERVAL = reg;
	return true;
}


/************************************************************************/
/* REG_INITIAL_PULSE_INTERVAL                                           */
/************************************************************************/
void app_read_REG_INITIAL_PULSE_INTERVAL(void)
{
	//app_regs.REG_INITIAL_PULSE_INTERVAL = 0;

}

bool app_write_REG_INITIAL_PULSE_INTERVAL(void *a)
{
	uint16_t reg = *((uint16_t*)a);
	
	if (reg < 200) return false;
	if (reg > 20000) return false;
	
	if (TCC0.CTRLA) return false;
	
	update_initial_pulse_interval(reg);

	app_regs.REG_INITIAL_PULSE_INTERVAL = reg;
	return true;
}


/************************************************************************/
/* REG_PULSE_STEP_INTERVAL                                              */
/************************************************************************/
void app_read_REG_PULSE_STEP_INTERVAL(void)
{
	//app_regs.REG_PULSE_STEP_INTERVAL = 0;

}

bool app_write_REG_PULSE_STEP_INTERVAL(void *a)
{
	uint16_t reg = *((uint16_t*)a);
	
	if (reg < 2) return false;
	if (reg > 2000) return false;
	
	if (TCC0.CTRLA) return false;
	
	update_pulse_step_interval(reg);
	
	app_regs.REG_PULSE_STEP_INTERVAL = reg;
	return true;
}


/************************************************************************/
/* REG_PULSE_PERIOD                                                     */
/************************************************************************/
void app_read_REG_PULSE_PERIOD(void)
{
	//app_regs.REG_PULSE_PERIOD = 0;

}

bool app_write_REG_PULSE_PERIOD(void *a)
{
	uint16_t reg = *((uint16_t*)a);
	
	if (reg < 10) return false;
	if (reg > 1000) return false;
	
	if (TCC0.CTRLA) return false;

	update_pulse_period(reg);

	app_regs.REG_PULSE_PERIOD = reg;
	return true;
}


/************************************************************************/
/* REG_ENCODER                                                          */
/************************************************************************/
void app_read_REG_ENCODER(void)
{
	//app_regs.REG_ENCODER = 0;

}

bool app_write_REG_ENCODER(void *a)
{
	int16_t reg = *((int16_t*)a);
	
	TCD1_CNT = 0x8000 + reg;

	app_regs.REG_ENCODER = reg;
	return true;
}


/************************************************************************/
/* REG_ANALOG_INPUT                                                     */
/************************************************************************/
void app_read_REG_ANALOG_INPUT(void)
{
	//app_regs.REG_ANALLOG_INPUT = 0;

}

bool app_write_REG_ANALOG_INPUT(void *a)
{
	return false;
}


/************************************************************************/
/* REG_STOP_SWITCH                                                      */
/************************************************************************/
void app_read_REG_STOP_SWITCH(void)
{
	app_regs.REG_STOP_SWITCH = (read_STOP_SWITCH) ? 0 : B_STOP_SWITCH;
}

bool app_write_REG_STOP_SWITCH(void *a)
{
	return false;
}


/************************************************************************/
/* REG_MOVING                                                           */
/************************************************************************/
void app_read_REG_MOVING(void)
{
	app_regs.REG_MOVING = (TCC0_CTRLA) ? B_IS_MOVING : 0;
}

bool app_write_REG_MOVING(void *a)
{
	return false;
}