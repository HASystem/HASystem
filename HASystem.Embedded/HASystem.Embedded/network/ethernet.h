/*
 * ethernet.h
 *
 * Created: 12.01.2015 17:30:05
 *  Author: Bernhard
 */ 


#ifndef ETHERNET_H_
#define ETHERNET_H_

typedef uint8_t MacAddress[6];

typedef struct Ethernet_Header
{
	MacAddress Dest;
	MacAddress Source;
	uint16_t Type;
} Ethernet_Header;

typedef struct Ethernet_Frame
{
	Ethernet_Header *Header;
	void* Data;
	uint32_t *Checksum;
} Ethernet_Frame;

#define ETHERNET_HEADER_SIZE 14
#define ETHERNET_MIN_SIZE 64
#define ETHERNET_MIN_DATA_SIZE ETHERNET_MIN_SIZE - ETHERNET_HEADER_SIZE

//ethernet types
#define ETHERNET_TYPE_IP4 0x0800
#define ETHERNET_TYPE_ARP 0x0806

#endif /* ETHERNET_H_ */