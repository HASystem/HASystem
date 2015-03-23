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
#include "filter/binaryInFilter.h"
#include "filter/binaryOutFilter.h"
#include "filter/analogInFilter.h"
#include "filter/analogCompare.h"
#include "filter/filter.h"
#include "UART.h"

#include "network/stack.h"
#include "hardware/onewire/onewire.h"


void startUp(void);
void updateLoop(void);
filterNode* createFilterNode(processMethodPointer processMethod);


filterNode* filterList = NULL;

int main(void)
{
	startUp();
	
	updateLoop();
}

void startUp(void)
{
	//filterList = addFilterNode(filterList, createFilterNode(&binaryIn_process));
	//addFilterNode(filterList, createFilterNode(&binaryOut_process));
	
	setup_uart();
	//print colored hallo welt
	//see http://en.wikipedia.org/wiki/ANSI_escape_code#graphics
	printf("\e[0;31mhallo welt\r\n");
	
	filterList = addFilterNode(filterList, createFilterNode(&analogIn_process));
	addFilterNode(filterList, createFilterNode(&analogCompare_process));
	addFilterNode(filterList, createFilterNode(&binaryOut_process));
	
	printf("setup network\r\n");
	setup_stack();
	printf("finished\r\n");
}

filterNode* createFilterNode(processMethodPointer processMethod)
{
	filterNode* newNode = (filterNode*) malloc(sizeof(filterNode));
	newNode->processMethod = processMethod;
	newNode->next = NULL;
	return newNode;
}

void updateLoop(void)
{
	void* arg = NULL;
	while(1)
	{
		//filterNode* current = filterList;
		//
		//while(current != NULL)
		//{
		//arg = current->processMethod(arg);
		//current = current->next;
		//}
		//
		eth_get_data();
	}
}