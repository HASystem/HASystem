/*
 * config.h
 *
 * Created: 11.01.2015 00:06:54
 *  Author: Bernhard
 */ 

#ifndef CONFIG_H_
#define CONFIG_H_

#ifndef F_CPU
// Systemtakt in Hz - Definition als unsigned long beachten
#define F_CPU 16000000UL
#warning "F_CPU war noch nicht definiert, wird nun nachgeholt mit 16000000"
#endif

#endif /* CONFIG_H_ */