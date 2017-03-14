

// include the library code:
#include <LiquidCrystal.h>

// initialize the library with the numbers of the interface pins
LiquidCrystal lcd(12, 11, 5, 4, 3, 2);

void setup() {
  Serial.begin(9600);
  lcd.begin(16, 2);
}

void loop() {
  while (Serial.available() > 0) {
    lcd.clear();
    lcd.setCursor(0,0);
    String top = Serial.readStringUntil('-');
    String bottom = Serial.readStringUntil('-');
    lcd.print(top);
    lcd.setCursor(0,1);
    lcd.print(bottom);
  }
  
}

