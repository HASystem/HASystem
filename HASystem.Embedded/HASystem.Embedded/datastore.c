/*
 * eprom.c
 *
 * Created: 08.06.2015 17:59:56
 *  Author: Bernhard
 */ 
#include "datastore.h"

#include <inttypes.h>
#include <stdbool.h>
#include <avr/eeprom.h>

bool datastore_read_value (uint16_t address, uint16_t length, unsigned char* buffer)
{
	bool hasValue = false;
	
	for (uint8_t i = 0; i < length; i++)
	{
		eeprom_busy_wait ();
		buffer[i] = eeprom_read_byte(address + i);
		if (buffer[i] != 0)
		{
			hasValue = true;
		}
	}
	return hasValue;
}