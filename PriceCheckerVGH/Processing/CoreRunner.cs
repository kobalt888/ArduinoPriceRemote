using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PriceCheckerVGH
{
    class CoreRunner
    {
        ScannerInterface arduino = new ScannerInterface();
        Core coreProcess = new Core();// object has all functions to get game data from upc, and write to csv
        string lastScan = null;
        public int addGame()
        {
            
            var input = Console.ReadLine();
            
            if (input.Length == 0)
            {
                return -1; //-1 will stop program
            }
            else if (lastScan == input)
            {
                var gameCallStatus = coreProcess.writeGame();
                arduino.writeToLcd(coreProcess.gameData.title, "added to .csv");
                lastScan = null;
                return 0;
            }
            else
            {
                coreProcess.getGame(input).Wait();
                try
                {
                    arduino.writeToLcd(coreProcess.gameData.title, coreProcess.gameData.price);
                    Clipboard.SetText(coreProcess.gameData.upc);
                    lastScan = input;//For the double scan system of adding the game to .csv
                }
                catch (Exception e)
                {                   
                    try
                    {
                        Console.WriteLine("No price for game found.");
                        arduino.writeToLcd(coreProcess.gameData.title, "No price found.");
                    }
                    catch(Exception e1)
                    {
                        Console.WriteLine("Heck, the game wasnt even found!");
                        arduino.writeToLcd("No UPC found","in database");
                    }
                    lastScan = null;
                }                
                return 0;
            }
        }
    }
}
