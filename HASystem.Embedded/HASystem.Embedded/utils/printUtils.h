/*
 * printUtils.h
 *
 * Created: 25.03.2015 23:48:44
 *  Author: Bernhard
 */ 


#ifndef PRINTUTILS_H_
#define PRINTUTILS_H_

#include <stdio.h>

//see http://en.wikipedia.org/wiki/ANSI_escape_code#graphics
#define PRINTUTILS_ERROR				"\e[0;31m"
#define PRINTUTILS_WARNING				"\e[0;33m"
#define PRINTUTILS_INFO					"\e[0;34m"
#define PRINTUTILS_VERBOSE				"\e[0;30m"
#define PRINTUTILS_DEBUG				"\e[0;32m"

#define printError(msg, ...)		printf(PRINTUTILS_ERROR); printf(msg, __VA_ARGS__)
#define printWarning(msg, ...)		printf(PRINTUTILS_WARNING); printf(msg, __VA_ARGS__)
#define printInfo(msg, ...)			printf(PRINTUTILS_INFO); printf(msg, __VA_ARGS__)
#define printVerbose(msg, ...)		printf(PRINTUTILS_VERBOSE); printf(msg, __VA_ARGS__)
#define printDebug(msg, ...)		printf(PRINTUTILS_DEBUG); printf(msg, __VA_ARGS__)

#endif /* PRINTUTILS_H_ */