/*
 * stack.c
 *
 * Created: 11.01.2015 01:39:58
 *  Author: Bernhard
 */ 

#include <inttypes.h>

#include "../hardware/ethernet/enc28j60.h"
#include "ethernet.h"
#include "stack.h"

void handle_packet(uint8_t *buffer, int32_t buffer_size);

Ip localIp;
Ip netmask;
Ip broadcast;

int32_t eth_packet_length = 0;
uint8_t eth_buffer[MTU_SIZE];

void setup_stack()
{
	//TODO: load from eprom
	localIp = create_ip(192, 168, 0, 2); //=192.168.0.2 = 3232235522
	netmask = create_ip(255, 255, 255, 0); //=255.255.255.0 = 4294967040
	
	broadcast = localIp | ~netmask;
	
	ETH_INIT();
}

void eth_get_data()
{
	eth_packet_length = ETH_PACKET_RECEIVE(MTU_SIZE, eth_buffer);
	if(eth_packet_length > 0)
	{
		handle_packet(eth_buffer, eth_packet_length);
	}
}

void handle_packet(uint8_t *buffer, int32_t buffer_size)
{
	printf("got packet size=%li\r\n", eth_packet_length);
	
	Ethernet_Header *header = (Ethernet_Header*)buffer;
	Ethernet_Frame frame;
	frame.Header = header;
	frame.Data = header + ETHERNET_HEADER_SIZE;
	frame.Checksum = (uint32_t*)(buffer + buffer_size);
	
	switch (HTONS16(frame.Header->Type))
	{
		case ETHERNET_TYPE_IP4:
			printf("got ipv4\r\n");
			break;
		case ETHERNET_TYPE_ARP:
			printf("got arp\r\n");
			break;
		default:
			printf("unkown ethernet type %d\r\n", frame.Header->Type);
	}
}