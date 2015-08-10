/* 
   DS18x20 Demo-Program
   
   V 0.9.2, 2/2011
   
   by Martin Thomas <eversmith@heizung-thomas.de>
   http://www.siwawi.arubi.uni-kl.de/avr-projects
    
   features:
   - DS18X20 and 1-Wire code is based on an example from Peter 
     Dannegger
   - uses Peter Fleury's uart-library which is very portable 
   - additional functions not found in the  uart-lib available
     in uart.h/.c
   - CRC-check based on code from Colin O'Flynn
   - accesses multiple sensors on multiple 1-Wire busses
   - example how to address every sensor in the bus by ROM-code
   - independant of system-timers (more portable) but some
     (very short) delays used
   - avr-libc's stdint.h in use 
   - no central include-file, parts of the code can be used as
     "library" easily
   - verbose output example
   - one-wire-bus can be changed at runtime if OW_ONE_BUS
     is not defined in onewire.h. There are still minor timing 
     issues when using the dynamic bus-mode
   - example on read/write of DS18x20 internal EEPROM
*/


/* This example has been tested with ATmega324P at 3.6864MHz and 16Mhz */

#if false

#include <avr/version.h>
#if __AVR_LIBC_VERSION__ < 10606UL
#error "please update to avrlibc 1.6.6 or newer, not tested with older versions"
#endif

#include "config.h"

#include <avr/io.h>
#include <avr/pgmspace.h>
#include <avr/interrupt.h>
#include <avr/eeprom.h>
#include <util/delay.h>
#include <string.h>
#include <stdint.h>

#include "uart.h"
#include "uart_addon.h"
#include "onewire.h"
#include "ds18x20.h"

#define MAXSENSORS 5

#define NEWLINESTR "\r\n"


uint8_t gSensorIDs[MAXSENSORS][OW_ROMCODE_SIZE];


static uint8_t search_sensors(void)
{
	uint8_t i;
	uint8_t id[OW_ROMCODE_SIZE];
	uint8_t diff, nSensors;
	
	printf( NEWLINESTR "Scanning Bus for DS18X20" NEWLINESTR );
	
	ow_reset();

	nSensors = 0;
	
	diff = OW_SEARCH_FIRST;
	while ( diff != OW_LAST_DEVICE && nSensors < MAXSENSORS ) {
		DS18X20_find_sensor( &diff, &id[0] );
		
		if( diff == OW_PRESENCE_ERR ) {
			printf( "No Sensor found" NEWLINESTR );
			break;
		}
		
		if( diff == OW_DATA_ERR ) {
			printf( "Bus Error" NEWLINESTR );
			break;
		}
		
		for ( i=0; i < OW_ROMCODE_SIZE; i++ )
			gSensorIDs[nSensors][i] = id[i];
		
		nSensors++;
	}
	
	return nSensors;
}

static void uart_put_temp(int16_t decicelsius)
{
	char s[10];

	uart_put_int( decicelsius );
	printf(" deci�C, ");
	DS18X20_format_from_decicelsius( decicelsius, s, 10 );
	uart_puts( s );
	printf(" �C");
}


#if DS18X20_MAX_RESOLUTION

static void uart_put_temp_maxres(int32_t tval)
{
	char s[10];

	uart_put_longint( tval );
	printf(" �Ce-4, ");
	DS18X20_format_from_maxres( tval, s, 10 );
	uart_puts( s );
	printf(" �C");
}

#endif /* DS18X20_MAX_RESOLUTION */


#if DS18X20_EEPROMSUPPORT

static void th_tl_dump(uint8_t *sp)
{
	DS18X20_read_scratchpad( &gSensorIDs[0][0], sp, DS18X20_SP_SIZE );
	printf( "TH/TL in scratchpad of sensor 1 now : " );
	uart_put_int( sp[DS18X20_TH_REG] );
	printf( " / " ); 
	uart_put_int( sp[DS18X20_TL_REG] );
	printf( NEWLINESTR ); 
}

static void eeprom_test(void)
{
	uint8_t sp[DS18X20_SP_SIZE], th, tl;
	
	printf( NEWLINESTR "DS18x20 EEPROM support test for first sensor" NEWLINESTR ); 
	// DS18X20_eeprom_to_scratchpad(&gSensorIDs[0][0]); // already done at power-on
	th_tl_dump( sp );
	th = sp[DS18X20_TH_REG];
	tl = sp[DS18X20_TL_REG];
	tl++;
	th++;
	DS18X20_write_scratchpad( &gSensorIDs[0][0], th, tl, DS18B20_12_BIT );
	printf( "TH+1 and TL+1 written to scratchpad" NEWLINESTR );
	th_tl_dump( sp );
	DS18X20_scratchpad_to_eeprom( DS18X20_POWER_PARASITE, &gSensorIDs[0][0] );
	printf( "scratchpad copied to DS18x20 EEPROM" NEWLINESTR );
	DS18X20_write_scratchpad( &gSensorIDs[0][0], 0, 0, DS18B20_12_BIT );
	printf( "TH and TL in scratchpad set to 0" NEWLINESTR );
	th_tl_dump( sp );
	DS18X20_eeprom_to_scratchpad(&gSensorIDs[0][0]);
	printf( "DS18x20 EEPROM copied back to scratchpad" NEWLINESTR );
	DS18X20_read_scratchpad( &gSensorIDs[0][0], sp, DS18X20_SP_SIZE );
	if ( ( th == sp[DS18X20_TH_REG] ) && ( tl == sp[DS18X20_TL_REG] ) ) {
		printf( "TH and TL verified" NEWLINESTR );
	} else {
		printf( "verify failed" NEWLINESTR );
	}
	th_tl_dump( sp );
}
#endif /* DS18X20_EEPROMSUPPORT */


int main( void )
{
	uint8_t nSensors, i;
	int16_t decicelsius;
	uint8_t error;
	
	setup_uart();
	
#ifndef OW_ONE_BUS
	ow_set_bus(&PIND,&PORTD,&DDRD,PD2);
#endif
	
	sei();
	
	printf( NEWLINESTR "DS18X20 1-Wire-Reader Demo by Martin Thomas" NEWLINESTR );
	printf(            "-------------------------------------------" );
	
	nSensors = search_sensors();
	uart_put_int( (int)nSensors );
	printf( " DS18X20 Sensor(s) available:" NEWLINESTR );
	
#if DS18X20_VERBOSE
	for (i = 0; i < nSensors; i++ ) {
		printf("# in Bus :");
		uart_put_int( (int)i + 1);
		printf(" : ");
		DS18X20_show_id_uart( &gSensorIDs[i][0], OW_ROMCODE_SIZE );
		printf( NEWLINESTR );
	}
#endif
		
	for ( i = 0; i < nSensors; i++ ) {
		printf( "Sensor# " );
		uart_put_int( (int)i+1 );
		printf( " is a " );
		if ( gSensorIDs[i][0] == DS18S20_FAMILY_CODE ) {
			printf( "DS18S20/DS1820" );
		} else if ( gSensorIDs[i][0] == DS1822_FAMILY_CODE ) {
			printf( "DS1822" );
		}
		else {
			printf( "DS18B20" );
		}
		printf( " which is " );
		if ( DS18X20_get_power_status( &gSensorIDs[i][0] ) == DS18X20_POWER_PARASITE ) {
			printf( "parasite" );
		} else {
			printf( "externally" ); 
		}
		printf( " powered" NEWLINESTR );
	}
	
#if DS18X20_EEPROMSUPPORT
	if ( nSensors > 0 ) {
		eeprom_test();
	}
#endif

	if ( nSensors == 1 ) {
		printf( NEWLINESTR "There is only one sensor "
		             "-> Demo of \"DS18X20_read_decicelsius_single\":" NEWLINESTR ); 
		i = gSensorIDs[0][0]; // family-code for conversion-routine
		DS18X20_start_meas( DS18X20_POWER_PARASITE, NULL );
		_delay_ms( DS18B20_TCONV_12BIT );
		DS18X20_read_decicelsius_single( i, &decicelsius );
		uart_put_temp( decicelsius );
		printf( NEWLINESTR );
	}
		
	
	for(;;) {   // main loop
	
		error = 0;

		if ( nSensors == 0 ) {
			error++;
		}

		printf( NEWLINESTR "Convert_T and Read Sensor by Sensor (reverse order)" NEWLINESTR ); 
		for ( i = nSensors; i > 0; i-- ) {
			if ( DS18X20_start_meas( DS18X20_POWER_PARASITE, 
				&gSensorIDs[i-1][0] ) == DS18X20_OK ) {
				_delay_ms( DS18B20_TCONV_12BIT );
				printf( "Sensor# " );
				uart_put_int( (int) i );
				printf(" = ");
				if ( DS18X20_read_decicelsius( &gSensorIDs[i-1][0], &decicelsius) 
				     == DS18X20_OK ) {
					uart_put_temp( decicelsius );
				} else {
					printf( "CRC Error (lost connection?)" );
					error++;
				}
				printf( NEWLINESTR );
			}
			else {
				printf( "Start meas. failed (short circuit?)" );
				error++;
			}
		}
		
		printf( NEWLINESTR "Convert_T for all Sensors and Read Sensor by Sensor" NEWLINESTR );
		if ( DS18X20_start_meas( DS18X20_POWER_PARASITE, NULL ) 
			== DS18X20_OK) {
			_delay_ms( DS18B20_TCONV_12BIT );
			for ( i = 0; i < nSensors; i++ ) {
				printf( "Sensor# " );
				uart_put_int( (int)i + 1 );
				printf(" = ");
				if ( DS18X20_read_decicelsius( &gSensorIDs[i][0], &decicelsius )
				     == DS18X20_OK ) {
					uart_put_temp( decicelsius );
				}
				else {
					printf( "CRC Error (lost connection?)" );
					error++;
				}
				printf( NEWLINESTR );
			}
#if DS18X20_MAX_RESOLUTION
			int32_t temp_eminus4;
			for ( i = 0; i < nSensors; i++ ) {
				printf( "Sensor# " );
				uart_put_int( i+1 );
				printf(" = ");
				if ( DS18X20_read_maxres( &gSensorIDs[i][0], &temp_eminus4 )
				     == DS18X20_OK ) {
					uart_put_temp_maxres( temp_eminus4 );
				}
				else {
					printf( "CRC Error (lost connection?)" );
					error++;
				}
				printf( NEWLINESTR );
			}
#endif
		}
		else {
			printf( "Start meas. failed (short circuit?)" );
			error++;
		}


#if DS18X20_VERBOSE
		// all devices:
		printf( NEWLINESTR "Verbose output" NEWLINESTR ); 
		DS18X20_start_meas( DS18X20_POWER_PARASITE, NULL );
		_delay_ms( DS18B20_TCONV_12BIT );
		DS18X20_read_meas_all_verbose();
#endif

		if ( error ) {
			printf( "*** problems - rescanning bus ..." );
			nSensors = search_sensors();
			uart_put_int( (int) nSensors );
			printf( " DS18X20 Sensor(s) available" NEWLINESTR );
			error = 0;
		}

		_delay_ms(3000); 
	}
}
#endif