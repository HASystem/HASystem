/*
 * AnalogInFilter.c
 *
 * Created: 11.11.2014 18:29:47
 *  Author: Bernhard
 */ 
#include "AnalogInFilter.h"

#include <stdbool.h>
#include <avr/io.h>

bool analogIn_loaded = false;

uint16_t ADC_Read( uint8_t channel )
{
	// Kanal waehlen, ohne andere Bits zu beeinfluﬂen
	ADMUX = (ADMUX & ~(0x1F)) | (channel & 0x1F);
	ADCSRA |= (1<<ADSC);            // eine Wandlung "single conversion"
	while (ADCSRA & (1<<ADSC) ) {   // auf Abschluss der Konvertierung warten
	}
	return ADCH;                    // ADC auslesen und zur¸ckgeben
}
	
void* analogIn_process(void* arg)
{
	if (!analogIn_loaded)
	{
		DDRA = 0x00;
		ADCSRA |= (1 << ADPS2) | (1 << ADPS1) | (1 << ADPS0); // Set ADC prescalar to 128 - 125KHz sample rate @ 16MHz

		ADMUX |= (1 << REFS0); // Set ADC reference to AVCC
		ADMUX |= (1 << ADLAR); // Left adjust ADC result to allow easy 8 bit reading

		// No MUX values needed to be changed to use ADC0

		//ADCSRA |= (1 << ADFR);  // Set ADC to Free-Running Mode

		ADCSRA |= (1 << ADEN);  // Enable ADC
		ADCSRA |= (1 << ADSC);  // Start A2D Conversions
		
		analogIn_loaded = true;
	}
	
	return (void*)ADC_Read(4);
}