#include "encoder.h"

void init_quadrature_encoder (void)
{
	/* Set up quadrature decoding event */
	EVSYS_CH0MUX = EVSYS_CHMUX_PORTC_PIN4_gc;
	EVSYS_CH0CTRL = EVSYS_QDEN_bm | EVSYS_DIGFILT_2SAMPLES_gc;

	/* Stop and reset timer */
	TCD1_CTRLA = TC_CLKSEL_OFF_gc;
	TCD1_CTRLFSET = TC_CMD_RESET_gc;

	/* Configure timer */
	TCD1_CTRLD = TC_EVACT_QDEC_gc | TC_EVSEL_CH0_gc;
	TCD1_PER = 0xFFFF;
	TCD1_CNT = 0x8000;

	/* Start timer */
	TCD1_CTRLA = TC_CLKSEL_DIV1_gc;
}

int16_t get_quadrature_encoder (void)
{
	int16_t timer_cnt = TCD1_CNT;
	
	if (timer_cnt > 32768)
	{
		return 0xFFFF - timer_cnt;
	}
	else
	{
		return (32768 - timer_cnt) * -1;
	}
}

void reset_quadrature_encoder (void)
{
	TCD1_CNT = 0x8000;
}