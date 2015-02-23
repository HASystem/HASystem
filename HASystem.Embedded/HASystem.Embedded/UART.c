/*
 * UART.c
 *
 * Created: 11.11.2014 21:23:56
 *  Author: Bernhard
 */ 

#include "UART.h"
#include <stdio.h>
#include <avr/io.h>

void setup_uart()
{
	UBRRH = UBRR_VAL >> 8;
	UBRRL = UBRR_VAL & 0xFF;
 
	UCSRB |= (1<<TXEN);  // UART TX einschalten
	UCSRC = (1<<URSEL)|(1<<UCSZ1)|(1<<UCSZ0);  // Asynchron 8N1
	
	static FILE mystdout = FDEV_SETUP_STREAM(uart_putc, NULL,_FDEV_SETUP_WRITE);

	stdout = &mystdout;
}

int uart_putc(unsigned char c)
{
	while (!(UCSRA & (1<<UDRE)))  /* warten bis Senden moeglich */
	{
	}
	
	UDR = c;                      /* sende Zeichen */
	return 0;
}


/* puts ist unabhaengig vom Controllertyp */
void uart_puts (char *s)
{
	while (*s)
	{   /* so lange *s != '\0' also ungleich dem "String-Endezeichen(Terminator)" */
		uart_putc(*s);
		s++;
	}
}