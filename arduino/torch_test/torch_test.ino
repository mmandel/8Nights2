#include <Adafruit_NeoPixel.h>
#ifdef __AVR__
  #include <avr/power.h>
#endif

// We'll use SoftwareSerial to communicate with the XBee:
#include <SoftwareSerial.h>
// XBee's DOUT (TX) is connected to pin 2 (Arduino's Software RX)
// XBee's DIN (RX) is connected to pin 3 (Arduino's Software TX)
SoftwareSerial XBee(2, 3); // RX, TX

//DARE: set this!
#define NEOPIXEL_PIN 6

// Parameter 1 = number of pixels in strip
// Parameter 2 = Arduino pin number (most are valid)
// Parameter 3 = pixel type flags, add together as needed:
//   NEO_KHZ800  800 KHz bitstream (most NeoPixel products w/WS2812 LEDs)
//   NEO_KHZ400  400 KHz (classic 'v1' (not v2) FLORA pixels, WS2811 drivers)
//   NEO_GRB     Pixels are wired for GRB bitstream (most NeoPixel products)
//   NEO_RGB     Pixels are wired for RGB bitstream (v1 FLORA pixels, not v2)
Adafruit_NeoPixel strip = Adafruit_NeoPixel(30, NEOPIXEL_PIN, NEO_GRB + NEO_KHZ800);

// IMPORTANT: To reduce NeoPixel burnout risk, add 1000 uF capacitor across
// pixel power leads, add 300 - 500 Ohm resistor on first pixel's data input
// and minimize distance between Arduino and first pixel.  Avoid connecting
// on a live circuit...if you must, connect GND first.

bool _isTorchOn = false;
bool _isMagicOn = false;

void setup() {
  // This is for Trinket 5V 16MHz, you can remove these three lines if you are not using a Trinket
  #if defined (__AVR_ATtiny85__)
    if (F_CPU == 16000000) clock_prescale_set(clock_div_1);
  #endif
  // End of trinket special code


  strip.begin();
  strip.show(); // Initialize all pixels to 'off'

  _isTorchOn = false;
  _isMagicOn = false;

  // Set up both ports at 9600 baud. This value is most important
  // for the XBee. Make sure the baud rate matches the config
  // setting of your XBee.
  XBee.begin(9600);
  Serial.begin(9600);  
}

void loop() 
{
  //read from serial port to get instructions from game, as sent over XBee
  if (XBee.available())
  { 
    char c = XBee.read();
    if(c == '0') //turn off torch
    {
      _isTorchOn = false;
      _isMagicOn = false;
    }
    //else if((c >= 'A') && (c <= 'H')) //torch on with magic
    else if(c == 'A')
    {
      _isTorchOn = true;
      _isMagicOn = true;
    }
    //else if((c >= '1') && (c <= '9')) //torch on, no magic
    else if(c == '1')
    {
      _isTorchOn = true;
      _isMagicOn = false;
    }

    Serial.write(c); //so we can see it on the Arduino serial monitor
  }  

  if(_isTorchOn)
  {
    //turn all the pixels on
    for (int i=0; i < strip.numPixels(); i++) 
    {
      if(_isMagicOn)  
        strip.setPixelColor(i, strip.Color(0, 255, 0));  //green for magic
      else
        strip.setPixelColor(i, strip.Color(255, 0, 0));  //red for no magic
    } 
  }
  else
  {
    //turn all the pixels off
    for (int i=0; i < strip.numPixels(); i++) 
    {
      strip.setPixelColor(i, 0);      
    }  
  }

  strip.show();
}


