/*
 * buttons.h
 *
 * Created: 23.03.2015 19:20:49
 *  Author: Kevin
 */ 


#ifndef GPIO_H_
#define GPIO_H_

#include "../hasSystem.h"

void GPIOInit(initArgs args);
void GPIOPushPacket(genericBuffer buf, packetSize size);
packetSize GPIOPullPacket(genericBuffer buf);

#endif /* GPIO_H_ */