/*
 * arp.h
 *
 * Created: 11.01.2015 21:20:05
 *  Author: Bernhard
 */ 


#ifndef ARP_H_
#define ARP_H_

#include <stdbool.h>
#include "ethernet.h"
#include "stack.h"

#define ARP_MAX_ARP_ENTRIES			5

void arp_handlePacket(ethernetFrame_t* frame, int bufferSize);
void arp_update(Ip ip, macAddress_t mac);
bool arp_knowsMac(Ip ip);

#endif /* ARP_H_ */