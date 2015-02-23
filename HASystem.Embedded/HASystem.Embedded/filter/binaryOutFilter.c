/*
* BinaryOut.c
*
* Created: 01.11.2014 21:53:54
*  Author: Bernhard
*/

#include "BinaryOutFilter.h"
#include <stdlib.h>
#include <stdbool.h>
#include <avr/io.h>

bool binaryOut_loaded = false;

void* binaryOut_process(void* arg)
{
	if (!binaryOut_loaded)
	{
		DDRD |= 1 << DDD7;
		binaryOut_loaded = true;
	}
	if (arg != (void*)0)
	{
		PORTD |= 1 << PD7;
	}
	else
	{
		PORTD &= ~(1 << PD7);
	}
	return NULL;
}