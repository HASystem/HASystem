/*
 * eprom.h
 *
 * Created: 08.06.2015 18:00:07
 *  Author: Bernhard
 */ 


#ifndef EEPROM_H_
#define EEPROM_H_

#include <inttypes.h>
#include <stdbool.h>

#define DATASTORE_ADDRESS_MAC			42
#define DATASTORE_ADDRESS_MAC_LENGTH	6

bool datastore_read_value (uint16_t address, uint16_t length, unsigned char* buffer);

#endif /* EEPROM_H_ */