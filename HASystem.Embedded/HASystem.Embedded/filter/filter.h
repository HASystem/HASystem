/*
 * Filter.h
 *
 * Created: 01.11.2014 22:17:59
 *  Author: Bernhard
 */ 


#ifndef FILTER_H_
#define FILTER_H_

typedef void* (*processMethodPointer)(void*);

typedef struct filterNode
{
	processMethodPointer processMethod;
	struct filterNode *next;
} filterNode;

filterNode* addFilterNode(filterNode* list, filterNode* node);

#endif /* FILTER_H_ */