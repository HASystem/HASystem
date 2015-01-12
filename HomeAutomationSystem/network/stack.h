/*
 * stack.h
 *
 * Created: 11.01.2015 01:40:09
 *  Author: Bernhard
 */ 


#ifndef STACK_H_
#define STACK_H_

#include <inttypes.h>

//const

//default Ethernet MTU = 1518
#define MTU_SIZE 1518

//typedefs
typedef uint32_t Ip;

//defined
#define create_ip(p1, p2, p3, p4) (Ip)((uint32_t)p1 << 24 | (uint32_t)p2 << 16 | (uint32_t)p3 << 8 | (uint32_t)p4)
#define HTONS16(n) (unsigned int)((((unsigned int) (n)) << 8) | (((unsigned int) (n)) >> 8))
#define HTONS32(x) ((x & 0xFF000000)>>24)+((x & 0x00FF0000)>>8)+((x & 0x0000FF00)<<8)+((x & 0x000000FF)<<24)

//methods
void setup_stack();
void eth_get_data();

#endif /* STACK_H_ */