/*
 * UART.h
 *
 * Created: 11.11.2014 21:24:05
 *  Author: Bernhard
 */ 


#ifndef UART_H_
#define UART_H_
 
#include "config.h"
 
#define BAUD 9600UL      // Baudrate

 
// Berechnungen
#define UBRR_VAL ((F_CPU+BAUD*8)/(BAUD*16)-1)   // clever runden
#define BAUD_REAL (F_CPU/(16*(UBRR_VAL+1)))     // Reale Baudrate
#define BAUD_ERROR ((BAUD_REAL*1000)/BAUD) // Fehler in Promille, 1000 = kein Fehler.
 
#if ((BAUD_ERROR<990) || (BAUD_ERROR>1010))
#error Systematischer Fehler der Baudrate grösser 1% und damit zu hoch!
#endif



void setup_uart();
int uart_putc(unsigned char c);
void uart_puts (char *s);



#endif /* UART_H_ */