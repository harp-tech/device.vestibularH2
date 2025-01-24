#ifndef _ENCODER_H_
#define _ENCODER_H_
#include <avr/io.h>

// Define if not defined
#ifndef bool
	#define bool uint8_t
#endif
#ifndef true
	#define true 1
	#define false 0
#endif

void init_quadrature_encoder (void);
int16_t get_quadrature_encoder (void);
void reset_quadrature_encoder (void);

#endif /* _ENCODER_H_ */