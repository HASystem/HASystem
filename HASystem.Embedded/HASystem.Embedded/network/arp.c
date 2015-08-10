/* * arp.c
 *
 * Created: 11.01.2015 21:19:57
 *  Author: Bernhard
 */ 

#include "stack.h"
#include "arp.h"
#include "ethernet.h"
#include "../utils/printUtils.h"

typedef struct	{
	int16_t HWType;
	int16_t PRType;
	char HWLen;
	char PRLen;
	int16_t ARP_Op;
	macAddress_t SrcMAC;
	Ip SrcIP;
	macAddress_t DestMAC;
	Ip DestIP;
} arp_packet_t;

typedef struct
{
	macAddress_t mac;
	Ip ip;
	uint32_t lastUpdate;
} arp_tableEntry_t;

#define ARP_PROTOCOL_HWTYPE_ETHERNET		0x0001
#define ARP_PROTOCOL_HWTYPE_PRTYPE			0x0800
#define ARP_PROTOCOL_HWLEN					0x06
#define ARP_PROTOCOL_PRLEN					0x04
#define ARP_PROTOCOL_ARPREQUEST				0x0001
#define ARP_PROTOCOL_ARPRESPONSE			0x0001

#define ARP_MAX_LIVETIME			60000


arp_tableEntry_t arpTable[ARP_MAX_ARP_ENTRIES];

void arp_addEntry(arp_packet_t* arpPacket);
void arp_sendResponse(arp_packet_t* request);

bool arp_entryValid(arp_tableEntry_t* entry)
{
	return entry->lastUpdate < ARP_MAX_LIVETIME;
}

void arp_handlePacket(ethernetFrame_t* frame, int bufferSize)
{
	arp_packet_t* arpPacket = (arp_packet_t*)frame->Data;
	
	//no buffer-size check because 4 * 7 < ETHERNET_MIN_DATA_SIZE
    if( arpPacket->HWType  == HTONS16(ARP_PROTOCOL_HWTYPE_ETHERNET) &&
		arpPacket->PRType  == HTONS16(ARP_PROTOCOL_HWTYPE_PRTYPE) &&
		arpPacket->HWLen   == ARP_PROTOCOL_HWLEN &&
		arpPacket->PRLen   == ARP_PROTOCOL_PRLEN &&
		arpPacket->DestIP == localIp)
    {
		//handle
		if (arpPacket->ARP_Op == HTONS16(ARP_PROTOCOL_ARPREQUEST))
		{
			//someone asked us for our mac
			printDebug("Got ARP-Request-Packet");
			arp_addEntry(arpPacket);
			
			arp_sendResponse(arpPacket);
		}
		else if (arpPacket->ARP_Op == HTONS16(ARP_PROTOCOL_ARPRESPONSE))
		{
			//we got our response for our request
			printDebug("Got ARP-Response-Packet");
			arp_addEntry(arpPacket);
		}
		else
		{
			printInfo("got unknown ARP-Packet");
		}
	}
}

bool arp_knowsMac(Ip ip)
{
	for(int i = 0; i < ARP_MAX_ARP_ENTRIES; i++)
	{
		if (arpTable[i].ip == ip)
		{
			return arp_entryValid(&arpTable[i]);
		}
	}
	return false;
}

void arp_addEntry(arp_packet_t* arpPacket)
{
	arp_update(arpPacket->SrcIP, arpPacket->SrcMAC);
}

void arp_update(Ip ip, macAddress_t mac)
{
	arp_tableEntry_t* entry = &arpTable[0];
	for(int i = 0; i < ARP_MAX_ARP_ENTRIES; i++)
	{
		if (arpTable[i].ip == ip)
		{
			//we already have an entry for this ip
			entry = &arpTable[i];
			break;
		}
		else if (!arp_entryValid(&arpTable[i]))
		{
			//this entry is invalid (never used or outdated)
			entry = &arpTable[i];
			break;
		}
		else
		{
			//otherwise we drop the oldest arp-entry
			if (entry->lastUpdate < arpTable[i].lastUpdate)
			{
				entry = &arpTable[i];
			}
		}
	}
	entry->ip = ip;
	entry->mac = mac;
	entry->lastUpdate = ARP_MAX_LIVETIME;
}

void arp_sendResponse(arp_packet_t* request)
{
	ethernetFrame_t* ethFrame = stack_createEthernetFrame();
	arp_packet_t* arpPacket = (arp_packet_t*)ethFrame->Data;
	ethFrame->Header->Type = HTONS16(ETHERNET_TYPE_ARP);
	arpPacket->DestIP = request->SrcIP;
	arpPacket->DestMAC = request->SrcMAC;
	arpPacket->SrcIP = localIp;
	arpPacket->SrcMAC = localMac;
	
	arpPacket->HWType = HTONS16(ARP_PROTOCOL_HWTYPE_ETHERNET);
	arpPacket->PRType = HTONS16(ARP_PROTOCOL_HWTYPE_PRTYPE);
	arpPacket->HWLen = HTONS16(ARP_PROTOCOL_HWLEN);
	arpPacket->PRLen = HTONS16(ARP_PROTOCOL_PRLEN);
	
	stack_sendEthernetFrame(ethFrame, sizeof(arp_packet_t));
}