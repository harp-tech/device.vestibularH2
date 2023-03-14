#include "analog_input.h"
#include "cpu.h"

#define F_CPU 32000000
#include <util/delay.h>

int16_t AdcOffset;

void init_analog_input (void)
{
	uint16_t adc[ADC_OFFSET_CONSECUTIVE_EQUAL_READINGS];
	
	/* Initialize ADCA with single ended input */
	adc_A_initialize_single_ended(ADC_REFSEL_INTVCC_gc);		// VCC/1.6 = 3.3/1.6 = 2.0625 V
	ADCA_CH0_INTCTRL |= ADC_CH_INTLVL_LO_gc;						// Enable ADC0 interrupt
	_delay_ms(100);
	
	/* Save ADC offset */
	bool reading_adc_offset = true;
		
	ADCA_CH0_MUXCTRL = 2 << 3;											// Select pin 2	
	do
	{
		for (uint8_t i = 0; i < ADC_OFFSET_CONSECUTIVE_EQUAL_READINGS; i++)
		{
			ADCA_CH0_CTRL |= ADC_CH_START_bm;						// Start conversion
			while(!(ADCA_CH0_INTFLAGS & ADC_CH_CHIF_bm));		// Wait for conversion to finish
			ADCA_CH0_INTFLAGS = ADC_CH_CHIF_bm;						// Clear interrupt bit
			adc[i] = ADCA_CH0_RES;										// Save offset
		}
		
		reading_adc_offset = false;
		
		for (uint8_t i = 0; i < ADC_OFFSET_CONSECUTIVE_EQUAL_READINGS-1; i++)
		{
			if (adc[i] != adc[i+1])
			{
				reading_adc_offset |= true;
			}
		}
	} while (reading_adc_offset);
	
	AdcOffset = adc[0];	
};


void start_analog_conversion (void)
{
	ADCA_CH0_MUXCTRL = 1 << 3;				// Select ADCA Channel 1
	ADCA_CH0_CTRL |= ADC_CH_START_bm;	// Start conversation on ADCA
}

int16_t get_analog_input (void)
{
	return ((int16_t)(ADCA_CH0_RES & 0x0FFF)) - AdcOffset;
}