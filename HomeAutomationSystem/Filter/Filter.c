/*
 * Filter.c
 *
 * Created: 01.11.2014 22:18:23
 *  Author: Bernhard
 */ 

#include "Filter.h"
#include <stdlib.h>
#include <avr/io.h>

filterNode* addFilterNode(filterNode* list, filterNode* node)
{
	PORTD |= 1 << PD7;
	if (list == NULL)
	{
		return node;
	}

	filterNode* last = list;
	while(last->next != NULL)
	{
		last = last->next;
	}
	last->next = node;
	return list;
}