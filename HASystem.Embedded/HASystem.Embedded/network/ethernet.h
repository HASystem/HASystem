/*
 * ethernet.h
 *
 * Created: 12.01.2015 17:30:05
 *  Author: Bernhard
 */ 


#ifndef ETHERNET_H_
#define ETHERNET_H_

#include <inttypes.h>

typedef struct
{
	uint8_t A;
	uint8_t B;
	uint8_t C;
	uint8_t D;
	uint8_t E;
	uint8_t F;
} macAddress_t;

typedef struct
{
	macAddress_t Dest;
	macAddress_t Source;
	uint16_t Type;
} ethernetHeader_t;

typedef struct
{
	ethernetHeader_t *Header;
	void* Data;
	uint32_t *Checksum;
} ethernetFrame_t;

#define ETHERNET_HEADER_SIZE 14
#define ETHERNET_MIN_SIZE 64
#define ETHERNET_MIN_DATA_SIZE ETHERNET_MIN_SIZE - ETHERNET_HEADER_SIZE

//ethernet types
#define ETHERNET_TYPE_IP4 0x0800
#define ETHERNET_TYPE_ARP 0x0806

#endif /* ETHERNET_H_ */