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

#define NUM_PIXELS 30

// Parameter 1 = number of pixels in strip
// Parameter 2 = Arduino pin number (most are valid)
// Parameter 3 = pixel type flags, add together as needed:
//   NEO_KHZ800  800 KHz bitstream (most NeoPixel products w/WS2812 LEDs)
//   NEO_KHZ400  400 KHz (classic 'v1' (not v2) FLORA pixels, WS2811 drivers)
//   NEO_GRB     Pixels are wired for GRB bitstream (most NeoPixel products)
//   NEO_RGB     Pixels are wired for RGB bitstream (v1 FLORA pixels, not v2)
Adafruit_NeoPixel strip = Adafruit_NeoPixel(NUM_PIXELS, NEOPIXEL_PIN, NEO_GRB + NEO_KHZ800);

// IMPORTANT: To reduce NeoPixel burnout risk, add 1000 uF capacitor across
// pixel power leads, add 300 - 500 Ohm resistor on first pixel's data input
// and minimize distance between Arduino and first pixel.  Avoid connecting
// on a live circuit...if you must, connect GND first.

bool _isTorchOn = false;
bool _isMagicOn = false;

//magic scrolling state
int _magicPixelOffset = 0;
int _lastMagicOffetMS = 0;
int _magicScrollDelay = 75; //increase this to slow down scrolling of pixels in magic mode

//special state change scrolling
bool _isSpecialTransish = false;
int  _specialEndMS = 0;
int _specialPixelOffset = 0;
int _lastSpecialOffetMS = 0;
int _specialScrollDelay = 25; //increase this to slow down scrolling of pixels for special state changes

//flicker behavior params
int _flickerMinDelay = 75; //min time we wait between changing the brightness
int _flickerMaxDelay = 120; //max time we wait between changing the brightness
int _flickerMinPct = 80; //min percent of brightness to apply to color (0 to 100)
int _flickerMaxPct = 100;  //max percent of brightness to apply to color (0 to 100)

//flicker runtime state
int _lastFlickerTime = 0;
int _nextFlickerDelay = 80;
int _curFlickerPct = 0;

struct TorchColor
{
  int red;
  int blue;
  int green;
};

struct TorchColor _colorProgresh[8] = { 
                                 {255, 44, 181},
                                 {255, 126, 0 },
                                 {79, 106, 255},
                                 {255, 239, 37},
                                 {18, 208, 255},
                                 {255, 33, 0},      
                                 {174, 44, 255},
                                 {107, 241, 33},                                                                                                                                                                                                
                                     };


TorchColor _curColor;

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
  _isSpecialTransish = false;
  _lastMagicOffetMS = 0;
  _curColor = _colorProgresh[0];

  // Set up both ports at 9600 baud. This value is most important
  // for the XBee. Make sure the baud rate matches the config
  // setting of your XBee.
  XBee.begin(9600);
  Serial.begin(9600);  
}

void loop() 
{
  bool wasTorchOn = _isTorchOn;
  bool hadMagic = _isMagicOn;
  
  //read from serial port to get instructions from game, as sent over XBee
  if (XBee.available())
  { 
    char c = XBee.read();
    if(c == '0') //turn off torch
    {
      _isTorchOn = false;
      _isMagicOn = false;
    }
    else if((c >= 'A') && (c <= 'H')) //torch on with magic
    {
      _isTorchOn = true;
      _isMagicOn = true;

      int colorIdx = ((int)c - (int)'A');
      _curColor = _colorProgresh[colorIdx];
    }
    else if((c >= '1') && (c <= '9')) //torch on, no magic
    {
      _isTorchOn = true;
      _isMagicOn = false;
      int colorIdx = ((int)c - (int)'1');
      _curColor = _colorProgresh[colorIdx];      
    }

    Serial.write(c); //so we can see it on the Arduino serial monitor
  }  

  //process transitions
  /*if(!wasTorchOn && _isTorchOn) //torch just went on!
  {
    _isSpecialTransish = true;
    _specialEndMS = millis() + 1000;
  }
  else if(!hadMagic && _isMagicOn) //magic just went on
  {
    _isSpecialTransish = true;
    _specialEndMS = millis() + 500;    
  }*/

  //deal with computing flicker
  if(millis() - _lastFlickerTime >= _nextFlickerDelay)
  {
    _nextFlickerDelay = random(_flickerMinDelay, _flickerMaxDelay);
    _lastFlickerTime = millis();
    _curFlickerPct = random(_flickerMinPct, _flickerMaxPct);
  }

  if(_isTorchOn)
  {
    if(_isSpecialTransish)
    {
      //scroll
      long elapsed = (millis() - _lastSpecialOffetMS);
      if(elapsed >= _specialScrollDelay)
      {
        _specialPixelOffset = (_specialPixelOffset + 1) % strip.numPixels();
        _lastSpecialOffetMS = millis();
      }

      //rainbow pixels
      for (int i=0; i < strip.numPixels(); i++) 
      {
        //apply scroll offset
        int pixelIdx = (i + _specialPixelOffset) % strip.numPixels();

        TorchColor pixelColor = _colorProgresh[pixelIdx % 8];
        strip.setPixelColor(pixelIdx, strip.Color(pixelColor.red, pixelColor.blue, pixelColor.green)); 
      }

      //done?
      if(millis() >= _specialEndMS)
        _isSpecialTransish = false;
    }
    else if(_isMagicOn)
    {
      //increment scroll offset every so often
      long elapsed = (millis() - _lastMagicOffetMS);
      if(elapsed >= _magicScrollDelay)
      {
        _magicPixelOffset = (_magicPixelOffset + 1) % strip.numPixels();
        _lastMagicOffetMS = millis();
      }
      
      //scroll the pixels
      for (int i=0; i < strip.numPixels(); i++) 
      {
        //apply scroll offset
        int pixelIdx = (i + _magicPixelOffset) % strip.numPixels();
        
        if((pixelIdx % 3) == 0) //every 3 gets the color
           strip.setPixelColor(pixelIdx, strip.Color(TakePercentOf(_curFlickerPct, _curColor.red), TakePercentOf(_curFlickerPct, _curColor.green),TakePercentOf(_curFlickerPct, _curColor.blue))); 
        else
           strip.setPixelColor(pixelIdx, 0); //off
      }
    }
    else
    {
      //turn on a solid color
      for (int i=0; i < strip.numPixels(); i++) 
      {  
        strip.setPixelColor(i, strip.Color(TakePercentOf(_curFlickerPct, _curColor.red), TakePercentOf(_curFlickerPct, _curColor.green),TakePercentOf(_curFlickerPct, _curColor.blue)));  
      } 
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

int TakePercentOf(int percent, int value)
{
  return ((value * percent * 655) / 65536);
}


