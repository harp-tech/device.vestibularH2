#include "stepper_motor.h"
#include "app_ios_and_regs.h"

/************************************************************************/
/* Global Parameters                                                    */
/************************************************************************/
extern AppRegs app_regs;

uint16_t m_pulse_period_us;
uint16_t m_min_pulse_interval_us;
uint16_t m_max_pulse_interval_us;
uint16_t m_pulse_step_interval_us;

int16_t ramp_steps;

/************************************************************************/
/* Update Global Parameters                                             */
/************************************************************************/

void update_nominal_pulse_interval (uint16_t time_us)
{
	m_min_pulse_interval_us = time_us >> 1;
	
	ramp_steps = (m_max_pulse_interval_us - m_min_pulse_interval_us) / m_pulse_step_interval_us;
}

void update_initial_pulse_interval (uint16_t time_us)
{
	m_max_pulse_interval_us = time_us >> 1;
	
	ramp_steps = (m_max_pulse_interval_us - m_min_pulse_interval_us) / m_pulse_step_interval_us;
}

void update_pulse_step_interval (uint16_t time_us)
{
	m_pulse_step_interval_us = time_us >> 1;
	
	ramp_steps = (m_max_pulse_interval_us - m_min_pulse_interval_us) / m_pulse_step_interval_us;
}

void update_pulse_period (uint16_t time_us)
{
	m_pulse_period_us = time_us >> 1;
	
	ramp_steps = (m_max_pulse_interval_us - m_min_pulse_interval_us) / m_pulse_step_interval_us;
}

/************************************************************************/
/* Globals                                                              */
/************************************************************************/
uint16_t motor_target;
uint16_t remaining;

uint32_t steps_target;
uint32_t steps_count;
uint32_t steps_remaining;

bool motor_is_running = false;
bool moving_positive;
bool decreasing_speed;

/************************************************************************/
/* Functions                                                            */
/************************************************************************/
void start_rotation (int32_t requested_steps)
{
	if (requested_steps > 0)
	{
		set_MOTOR_DIRECTION;
		moving_positive = true;
		steps_target = (uint32_t)requested_steps;
	}
	else
	{
		clr_MOTOR_DIRECTION;
		moving_positive = false;
		steps_target = (uint32_t)(~requested_steps + 1);
	}	
	
	steps_count = 0;				// Reset steps counter
	steps_remaining = 0;			// Reset remaining steps
	//TCD1_CNT = 0x8000;			// Reset encoder
	
	decreasing_speed = false;	// Reset decreasing speed flag
	motor_is_running = true;	// Update global with motor state
	
	/* Start the generation of pulses */
	timer_type0_pwm(&TCC0, TIMER_PRESCALER_DIV64, m_max_pulse_interval_us, m_pulse_period_us, INT_LEVEL_MED, INT_LEVEL_MED);
}

void stop_rotation (void)
{
	timer_type0_stop(&TCC0);
	motor_is_running = false;
}

int32_t user_sent_request (int32_t requested_steps)
{
	if (!motor_is_running)
	{
		start_rotation(requested_steps);
		return 0;
	}
	else
	{
		if ((requested_steps > 0) && (moving_positive == true))
		{			
			steps_target += requested_steps;
			return 0;
		}
		
		if ((requested_steps < 0) && (moving_positive == false))
		{
			steps_target += (uint32_t)(~requested_steps + 1);
			return 0;
		}
		
		if ((requested_steps > 0) && (moving_positive == false))
		{
			if (decreasing_speed)
			{
				return requested_steps;
			}
			else if (steps_count <= ramp_steps)
			{
				return requested_steps;
			}
			else
			{
				uint32_t available_steps_to_decrease = steps_remaining - ramp_steps - 1;
				
				if (requested_steps <= available_steps_to_decrease)
				{
					steps_target -= requested_steps;
					return 0;
				}
				else
				{
					steps_target -= available_steps_to_decrease;
					return requested_steps - available_steps_to_decrease;
				}
			}
		}
		
		if ((requested_steps < 0) && (moving_positive == true))
		{
			if (decreasing_speed)
			{
				return requested_steps;
		}
		else if (steps_count <= ramp_steps)
		{
			return requested_steps;
		}
			else
			{
				uint32_t available_steps_to_decrease = steps_remaining - ramp_steps - 1;
				
				if ((~requested_steps+1) <= available_steps_to_decrease)
				{
					steps_target -= (uint32_t)(~requested_steps + 1);
					return 0;
				}
				else
				{
					steps_target -= available_steps_to_decrease;
					return requested_steps + available_steps_to_decrease;
				}
			}
		}
	}
}

ISR(TCC0_OVF_vect/*, ISR_NAKED*/)
{	
	if (TCC0_INTCTRLB == 0)
	{
		TCC0_PER = (app_regs.REG_IMMEDIATE_PULSES >> 1) - 1;
		TCC0_CCA = app_regs.REG_IMMEDIATE_PULSES >> 2;
		
		return;
	}
	
	remaining = motor_target - steps_count;
	
	
	steps_count++;
	
	steps_remaining = steps_target - steps_count; 
		
	if ((steps_remaining <= steps_count) && (steps_remaining <= ramp_steps))
	{		
		decreasing_speed = true;
		
		/* Decrease motor speed */
		if (TCC0_PER < m_max_pulse_interval_us)
		{
			TCC0_PER = (TCC0_PER + m_pulse_step_interval_us > m_max_pulse_interval_us)? m_max_pulse_interval_us : TCC0_PER + m_pulse_step_interval_us;
		}
	}
	else
	{
		decreasing_speed = false;
		
		/* Increase motor speed */
		if (TCC0_PER > m_min_pulse_interval_us)
		{
			TCC0_PER = (TCC0_PER - m_pulse_step_interval_us < m_min_pulse_interval_us)? m_min_pulse_interval_us : TCC0_PER - m_pulse_step_interval_us;
		}	
	}
}

bool send_motor_stopped_notification = false;

ISR(TCC0_CCA_vect/*, ISR_NAKED*/)
{		
	if (steps_count == steps_target)
	{
		/* Stop motor */
		stop_rotation();
		
		/* Since this is used at MID level interrupts, send an event from here can happen in the middle of other event */
		send_motor_stopped_notification = true;
	}
}

