/*
 * buttons.c
 *
 * Created: 23.03.2015 19:21:09
 *  Author: Kevin
 */ 
#include "gpio.h"

typedef struct {
	
} gpioInitPacket_t;

typedef struct {
	
} gpioPushPacket_t;

typedef struct {
	
} gpioPullPacket_t;

void GPIOInit(initArgs args)
{
	gpioInitPacket_t *arg = (gpioInitPacket_t*)args;
	
	//in
	//DDRD &= ~(1 << DDD5);
	//PORTD |= 1 << DDD5;
	
	//out
	//DDRD |= 1 << DDD7;
}

void GPIOPushPacket(genericBuffer buf, packetSize size)
{
	if (size < sizeof(gpioPushPacket_t))
	{
		//we didn't got enough data
		return;
	}
	
	//if ((PIND & (1 << PD5)))
	//{
	//return (void*)0;
	//}
	//return (void*)1;
		
	gpioPushPacket_t *arg = (gpioPushPacket_t*)buf;
}

packetSize GPIOPullPacket(genericBuffer buf)
{
	gpioPullPacket_t *arg = (gpioPullPacket_t*)buf;
	
	//if (1)
	//{
		//PORTD |= 1 << PD7;
	//}
	//else
	//{
		//PORTD &= ~(1 << PD7);
	//}
	
	return sizeof(gpioPullPacket_t);
}