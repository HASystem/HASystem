/*
 * adc.h
 *
 * Created: 26.03.2015 00:18:14
 *  Author: Bernhard
 */ 


#ifndef ADC_H_
#define ADC_H_

#include "../hasSystem.h"

void ADCInit(initArgs args);
void ADCPushPacket(genericBuffer buf, packetSize size);
packetSize ADCPullPacket(genericBuffer buf);

#endif /* ADC_H_ */