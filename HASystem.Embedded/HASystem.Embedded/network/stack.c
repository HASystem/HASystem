/*
 * stack.c
 *
 * Created: 11.01.2015 01:39:58
 *  Author: Bernhard
 */ 

#include <inttypes.h>

#include "stack.h"

#include "../utils/printUtils.h"
#include "../hardware/ethernet/enc28j60.h"
#include "ethernet.h"
#include "arp.h"

void handle_packet(int32_t buffer_size);

Ip localIp;
Ip netmask;
Ip broadcast;
macAddress_t localMac;

uint8_t eth_buffer[MTU_SIZE];

void stack_setup(void)
{
	//TODO: load from eprom
	localIp = create_ip(192, 168, 0, 2); //=192.168.0.2 = 3232235522
	netmask = create_ip(255, 255, 255, 0); //=255.255.255.0 = 4294967040
	
	localMac = (macAddress_t){ 0x00, 0x20, 0x20, 0x20, 0x20, 0x20 };
	
	broadcast = localIp | ~netmask;
	
	ETH_INIT();
}

void stack_getEthernetData(void)
{
	int32_t eth_packet_length = ETH_PACKET_RECEIVE(MTU_SIZE, eth_buffer);
	//dest-mac check is done by ethernet chip
	if(eth_packet_length > 0)
	{
		handle_packet(eth_packet_length);
	}
}

void handle_packet(int32_t buffer_size)
{
	printDebug("got packet size=%li\r\n", buffer_size);
	
	if (buffer_size <= ETHERNET_MIN_SIZE)
	{
		return;
	}
	
	ethernetHeader_t *header = (ethernetHeader_t*)eth_buffer;
	ethernetFrame_t frame;
	frame.Header = header;
	frame.Data = header + ETHERNET_HEADER_SIZE;
	frame.Checksum = (uint32_t*)(eth_buffer + buffer_size);
	
	switch (HTONS16(frame.Header->Type))
	{
		case ETHERNET_TYPE_IP4:
			printVerbose("got ipv4\r\n");
			break;
		case ETHERNET_TYPE_ARP:
			printVerbose("got arp\r\n");
			arp_handlePacket(&frame, buffer_size);
			break;
		default:
			printInfo("unkown ethernet type %d\r\n", frame.Header->Type);
	}
}

ethernetFrame_t* stack_createEthernetFrame(void)
{
	return (ethernetFrame_t*)eth_buffer;
}

void stack_sendEthernetFrame(ethernetFrame_t* packet, uint16_t dataSize)
{
	packet->Checksum = packet->Data + dataSize;
	//TODO: calculate checksum
	//TODO: send
}