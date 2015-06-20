#include <avr/pgmspace.h>
#include <EEPROM.h>
#include <TM1638.h>
//#include <InvertedTM1638.h>
#include <SPI.h>
#include "Arduino.h"
#include <avr/pgmspace.h>

struct ScreenItem {
public:

	TM1638 * Screen;
	byte Buttons;
	byte Oldbuttons;
	byte Intensity;

	ScreenItem(TM1638 * module) : Screen(module){
		this->Buttons = 0;
		this->Oldbuttons = 0;
		this->Intensity = 7;
	}
};

// Connected modules
int ENABLEDMODULES = 4;

// Modules common pins
#define DIO 8
#define CLK 7

// 1st module strobe pin
#define STB1 9
// 2nd screen strobe pin
#define STB2 10
// 3nd screen strobe pin
#define STB3 11
// 4rd screen strobe pin
#define STB4 12
TM1638 module1(DIO, CLK, STB1, false);
ScreenItem screen1(&module1);

TM1638 module2(DIO, CLK, STB2, false);
ScreenItem screen2(&module2);

TM1638 module3(DIO, CLK, STB3, false);
ScreenItem screen3(&module3);

TM1638 module4(DIO, CLK, STB4, false);
ScreenItem screen4(&module4);

// Screen referencing
ScreenItem * screens[] = { &screen1, &screen2, &screen3, &screen4 };

int i;
byte displayValues[] = { 1, 2, 4, 8, 16, 32, 64, 128 };

void setup()
{
	Serial.begin(19200);
	for (i = 0; i < ENABLEDMODULES; i++)
	{
		screens[i]->Screen->setupDisplay(true, 7);
		screens[i]->Screen->clearDisplay();
	}
}

void SetDisplayFromSerial(TM1638 * screen)
{
	// Digits
	{
		for (i = 0; i < 8; i++){
			displayValues[i] = Serial.read();
		}
		screen->setDisplay(displayValues);
	}

	// Leds
	for (i = 0; i < 8; i++){
		char state = (char)Serial.read();
		if (state == 'G'){
			screen->setLED(TM1638_COLOR_GREEN, i);
		}
		else if (state == 'R'){
			screen->setLED(TM1638_COLOR_RED, i);
		}
		else{
			screen->setLED(TM1638_COLOR_NONE, i);
		}
	}
}

void sendButtonState(){

	bool sendButtons = false;

	for (i = 0; i < ENABLEDMODULES; i++){
		screens[i]->Buttons = screens[i]->Screen->getButtons();
		if (screens[i]->Buttons != screens[i]->Oldbuttons){
			sendButtons = true;
		}
		screens[i]->Oldbuttons = screens[i]->Buttons;
	}

	if (sendButtons){
		for (i = 0; i < ENABLEDMODULES; i++){
			Serial.write(screens[i]->Buttons);
		}
		Serial.flush();
	}
}

void loop() {
	// Wait for data
	if (Serial.available() >= 1){
		// Read command
		char opt = Serial.read();

		// Hello command
		if (opt == '1'){
			delay(10);
			Serial.print('a');
			Serial.flush();
		}

		//  Module count command
		if (opt == '2'){
			Serial.print((byte)ENABLEDMODULES);
			Serial.flush();
		}

		// Write data
		if (opt == '3'){
			for (int j = 0; j < ENABLEDMODULES; j++){
				while (Serial.available() < 17) {
				}

				// Wait for display data
				int newIntensity = Serial.read();
				if (newIntensity != screens[j]->Intensity){
					screens[j]->Screen->setupDisplay(true, newIntensity);
					screens[j]->Intensity = newIntensity;
				}

				SetDisplayFromSerial(screens[j]->Screen);
			}
		}
	}
	sendButtonState();
}