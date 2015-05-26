#include <Adafruit_NeoPixel.h>
#include <avr/power.h>
#include "LedControl.h"
#include "Arduino.h"
#include <avr/pgmspace.h>
#define PIN 6

struct ScreenItem {
public:
	byte Intensity;
	ScreenItem(){
		this->Intensity = 7;
	}
};

// Connected modules
int ENABLEDMODULES = 2;
int RGBLEDCOUNT = 16;
bool RGBLEDREVERSE = true;
/*
 * We use pins 12,11 and 10 on the Arduino for the SPI interface
 * Pin 12 is connected to the DATA IN-pin of the first MAX7221
 * Pin 11 is connected to the CLK-pin of the first MAX7221
 * Pin 10 is connected to the LOAD(/CS)-pin of the first MAX7221
 * There will only be a single MAX7221 attached to the arduino
 */
LedControl lc = LedControl(12, 11, 10, 2);

// Parameter 1 = number of pixels in strip
// Parameter 2 = Arduino pin number (most are valid)
// Parameter 3 = pixel type flags, add together as needed:
//   NEO_KHZ800  800 KHz bitstream (most NeoPixel products w/WS2812 LEDs)
//   NEO_KHZ400  400 KHz (classic 'v1' (not v2) FLORA pixels, WS2811 drivers)
//   NEO_GRB     Pixels are wired for GRB bitstream (most NeoPixel products)
//   NEO_RGB     Pixels are wired for RGB bitstream (v1 FLORA pixels, not v2)
Adafruit_NeoPixel strip = Adafruit_NeoPixel(RGBLEDCOUNT, PIN, NEO_GRB + NEO_KHZ800);


ScreenItem screen1;
ScreenItem screen2;
ScreenItem screen3;
ScreenItem screen4;

// Screen referencing
ScreenItem * screens [] = { &screen1, &screen2, &screen3, &screen4 };
// IMPORTANT: To reduce NeoPixel burnout risk, add 1000 uF capacitor across
// pixel power leads, add 300 - 500 Ohm resistor on first pixel's data input
// and minimize distance between Arduino and first pixel.  Avoid connecting
// on a live circuit...if you must, connect GND first.

int i;
void setup() {

	Serial.begin(19200);

	strip.begin();
	strip.show();

	//we have already set the number of devices when we created the LedControl
	int devices = lc.getDeviceCount();

	for (int address = 0; address < devices; address++) {
		/*The MAX72XX is in power-saving mode on startup*/
		lc.shutdown(address, false);
		/* Set the brightness to a medium values */
		lc.setIntensity(address, 16);
		/* and clear the display */
		lc.clearDisplay(address);
		/*lc.setRow(0, 0, 0b00000001);
		lc.setRow(0, 1, 0b00000011);
		lc.setRow(0, 2, 0b00000111);
		lc.setRow(0, 3, 0b00001111);
		lc.setRow(0, 4, 0b00011111);
		lc.setRow(0, 5, 0b00111111);
		lc.setRow(0, 6, 0b01111111);
		lc.setRow(0, 7, 0b11111111);*/
	}
}

// Reverse the order of bits in a byte. 
// I.e. MSB is swapped with LSB, etc. 
byte Bit_Reverse(byte x)
{
	x = ((x >> 1) & 0x55) | ((x << 1) & 0xaa);
	x = ((x >> 2) & 0x33) | ((x << 2) & 0xcc);
	x = ((x >> 4) & 0x0f) | ((x << 4) & 0xf0);
	return (x >> 1) | ((x & 1) << 7);
}

byte displayValues [] = { 1, 2, 4, 8, 16, 32, 64, 128 };
void SetDisplayFromSerial(int idx, bool readLeds)
{
	// Digits
	{
		for (i = 0; i < 8; i++){
			while (Serial.available() < 1) {
				//delayMicroseconds(10);
			}
			displayValues[i] = Bit_Reverse((char) readByte());// //Serial.read());
		}

		if (readLeds || checkCrc()){
			for (i = 0; i < 8; i++){
				lc.setRow(idx, 7 - i, displayValues[i]);
			}
		}
	}

	// Leds
	if (readLeds){
		for (i = 0; i < 8; i++){

			while (Serial.available() < 1) {
			}

			char state = (char) Serial.read();

			int tmppos = (((idx - ENABLEDMODULES + 1)*-1) * 8) + i;
			int pos = 0;
			if (tmppos < 3)
				pos = 0;
			else if (tmppos > 13)
				pos = 16;
			else
				pos = (tmppos - 3) / 10.0 * 16.0;

			if (state == 'R' || state == 'G')
				strip.setPixelColor(idx * 8 + (7 - i), pos, 0, 16 - pos);

			//if (state == 'R'){
			//	strip.setPixelColor(idx * 8 + (7 - i), 0, 10, 0);
			//	//screen->setLED(TM1638_COLOR_GREEN, i);
			//}
			//else if (state == 'G'){
			//	strip.setPixelColor(idx * 8 + (7 - i), 10, 0, 0);
			//}
			else{
				strip.setPixelColor(idx * 8 + (7 - i), 0, 0, 0);
			}
			strip.show();
		}
	}
}

byte crc = 0;
byte current = 0;

inline byte readByte(){
	current = Serial.read();
	crc = (byte) ((int) crc + (int) current);
	return current;
}

boolean res = 0;
inline boolean checkCrc(){
	while (Serial.available() < 1) {
		//delayMicroseconds(10);
	}
	current = Serial.read();
	res = crc == current;
	crc = 0;
	return res;
}

void loop() {
	// Wait for data
	if (Serial.available() >= 1){
		// Read command
		char opt = Serial.read();

		// Hello command
		if (opt == '1'){
			delay(10);
			Serial.print('b');
			Serial.flush();
		}

		//  Module count command
		if (opt == '2'){
			Serial.print((byte) ENABLEDMODULES);
			Serial.flush();
		}

		//  RGBLED count command
		if (opt == '4'){
			Serial.write((byte) RGBLEDCOUNT);
			Serial.flush();
		}


		// Write data
		if (opt == '3'){
			for (uint8_t j = 0; j < ENABLEDMODULES; j++){
				while (Serial.available() < 1) {
				}

				// Wait for display data
				int newIntensity = Serial.read();
				/*if (newIntensity != screens[j]->Intensity){
					lc.setIntensity(j, newIntensity);
					screens[j]->Intensity = newIntensity;
					}*/

				SetDisplayFromSerial(j, true);
			}
		}

		// Write data
		if (opt == '5'){
			for (uint8_t j = 0; j < ENABLEDMODULES; j++){
				while (Serial.available() < 1) {
				}

				// Wait for display data
				int newIntensity = readByte();//Serial.read();
				/*if (newIntensity != screens[j]->Intensity){
				lc.setIntensity(j, newIntensity);
				screens[j]->Intensity = newIntensity;
				}*/

				SetDisplayFromSerial(j, false);
			}
		}

		// Write data
		if (opt == '6'){
			for (uint8_t j = 0; j < RGBLEDCOUNT; j++){
				while (Serial.available() < 3) {
				}
				uint8_t r = readByte();//Serial.read();
				uint8_t g = readByte();//Serial.read();
				uint8_t b = readByte();//Serial.read();

				strip.setPixelColor(RGBLEDCOUNT - j - 1, r, g, b);
			}

			if (checkCrc()){
				strip.show();
			}
		}

		// Set baudrate
		if (opt == '8'){
			while (Serial.available() < 1) {
				//delayMicroseconds(10);
			}
			int br = Serial.read();
			if (br == 1) Serial.begin(300);
			if (br == 2) Serial.begin(1200);
			if (br == 3) Serial.begin(2400);
			if (br == 4) Serial.begin(4800);
			if (br == 5) Serial.begin(9600);
			if (br == 6) Serial.begin(14400);
			if (br == 7) Serial.begin(19200);
			if (br == 8) Serial.begin(28800);
			if (br == 9) Serial.begin(38400);
			if (br == 10) Serial.begin(57600);
			if (br == 11) Serial.begin(115200);
			if (br == 12) Serial.begin(230400);
			if (br == 13) Serial.begin(250000);
			if (br == 14) Serial.begin(1000000);
			if (br == 15) Serial.begin(2000000);
		}
	}

}














