/*
 * AnalogCompare.c
 *
 * Created: 11.11.2014 18:53:02
 *  Author: Bernhard
 */ 
#include "AnalogCompare.h"

#include <stdbool.h>
#include <avr/io.h>
#include <stdio.h>
#include <inttypes.h>

void* analogCompare_process(void* arg)
{
	uint16_t value = (uint16_t)arg;
	
	if (value > 128)
	{
		return (void*)1;
	}
	else
	{
		return (void*)0;
	}
}