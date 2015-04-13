/*
 * adc.c
 *
 * Created: 26.03.2015 00:18:23
 *  Author: Bernhard
 */ 
#include "adc.h"

typedef struct {
	
} adcInitPacket_t;

typedef struct {
	
} adcPushPacket_t;

typedef struct {
	
} adcPullPacket_t;

void ADCInit(initArgs args)
{
	adcInitPacket_t *arg = (adcInitPacket_t *)args;
	
	//DDRA = 0x00;
	//ADCSRA |= (1 << ADPS2) | (1 << ADPS1) | (1 << ADPS0); // Set ADC prescalar to 128 - 125KHz sample rate @ 16MHz
//
	//ADMUX |= (1 << REFS0); // Set ADC reference to AVCC
	//ADMUX |= (1 << ADLAR); // Left adjust ADC result to allow easy 8 bit reading
//
	//// No MUX values needed to be changed to use ADC0
//
	////ADCSRA |= (1 << ADFR);  // Set ADC to Free-Running Mode
//
	//ADCSRA |= (1 << ADEN);  // Enable ADC
	//ADCSRA |= (1 << ADSC);  // Start A2D Conversions
}

void ADCPushPacket(genericBuffer buf, packetSize size)
{
	if (size < sizeof(adcPushPacket_t))
	{
		//we didn't got enough data
		return;
	}
	adcPushPacket_t *arg = (adcPushPacket_t*)buf;
}

packetSize ADCPullPacket(genericBuffer buf)
{
	adcPullPacket_t *arg = (adcPullPacket_t*)buf;
	
	//// Kanal waehlen, ohne andere Bits zu beeinflußen
	//uint8_t channel = 4;
	//ADMUX = (ADMUX & ~(0x1F)) | (channel & 0x1F);
	//ADCSRA |= (1<<ADSC);            // eine Wandlung "single conversion"
	//while (ADCSRA & (1<<ADSC) ) {   // auf Abschluss der Konvertierung warten
	//}
	//return ADCH;                    // ADC auslesen und zurückgeben
	
	return sizeof(adcPullPacket_t);
}