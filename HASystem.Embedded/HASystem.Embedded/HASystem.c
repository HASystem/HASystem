/*
 * HASystem.c
 *
 * Created: 23.02.2015 20:58:18
 *  Author: Bernhard
 */ 

#include <avr/io.h>
#include <alloca.h>
#include <stdlib.h>
#include <stdio.h>

#include "config.h"
#include "UART.h"
#include "utils/printUtils.h"

#include "network/stack.h"

void startUp(void);
void updateLoop(void);


int main(void)
{
	startUp();
	
	updateLoop();
}

void startUp(void)
{
	setup_uart();
	printInfo("Starting HASystem\r\n");
	
	printInfo("setup network...");
	stack_init();
	printInfo("finished\r\n");
}

void updateLoop(void)
{
	for(;;)
	{
		eth_get_data();
	}
}