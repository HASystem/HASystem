/*
* HomeAutomationSystem.c
*
* Created: 01.11.2014 21:33:18
*  Author: Bernhard
*/

#include <avr/io.h>
#include <alloca.h>
#include <stdlib.h>
#include <stdio.h>

#include "Filter/BinaryInFilter.h"
#include "Filter/BinaryOutFilter.h"
#include "Filter/AnalogInFilter.h"
#include "Filter/AnalogCompare.h"
#include "Filter/Filter.h"
#include "UART.h"


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
	printf("\e[0;31mhallo welt");
	
	filterList = addFilterNode(filterList, createFilterNode(&analogIn_process));
	addFilterNode(filterList, createFilterNode(&analogCompare_process));
	addFilterNode(filterList, createFilterNode(&binaryOut_process));
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
		filterNode* current = filterList;

		while(current != NULL)
		{
			arg = current->processMethod(arg);
			current = current->next;
		}
	}
}