/*
 * BinaryIn.c
 *
 * Created: 01.11.2014 21:51:21
 *  Author: Bernhard
 */ 

#include "BinaryInFilter.h"
#include <stdbool.h>
#include <avr/io.h>

bool binaryIn_loaded = false;

void* binaryIn_process(void* arg)
{
	if (!binaryIn_loaded)
	{
		DDRD &= ~(1 << DDD5);
		PORTD |= 1 << DDD5;
		binaryIn_loaded = true;
	}
	if ((PIND & (1 << PD5)))
	{
		return (void*)0;
	}
	return (void*)1;
}