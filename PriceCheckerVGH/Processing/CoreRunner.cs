using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PriceCheckerVGH
{
    public class CoreRunner
    {
        public ScannerInterface arduino = new ScannerInterface();
        Core coreProcess = new Core();// object has all functions to get game data from upc, and write to csv
        string lastScan = null;
        public string status = "Initialized";
        public async Task<int> addGame(string upc)
        {


            if (upc.Length == 0)
            {
                return -1; //-1 will stop program
            }
            else if (lastScan == upc)
            {
                var gameCallStatus = coreProcess.writeGame();
                arduino.writeToLcd(coreProcess.gameData.title, " added to .csv");
                status = coreProcess.gameData.title + " added to .csv"; 
                lastScan = null;
                return 0;
            }
            else
            {
                await Task.Run(() => coreProcess.getGame(upc));
                try
                {
                    arduino.writeToLcd(coreProcess.gameData.title, coreProcess.gameData.price);
                    Clipboard.SetText(coreProcess.gameData.upc);
                    lastScan = upc;//For the double scan system of adding the game to .csv
                    status = coreProcess.gameData.title + " "+ coreProcess.gameData.price;
                }
                catch (Exception e)
                {
                    try
                    {
                        arduino.writeToLcd(coreProcess.gameData.title, "No price found.");
                        status = coreProcess.gameData.title + " No price found.";
                    }
                    catch (Exception e1)
                    {                       
                        arduino.writeToLcd("No UPC found", "in database");
                        status = "No UPC found in database";
                    }
                    lastScan = null;
                }
                return 0;
            }
        }
        public void connect()
        {
            arduino.connect();
        }
    }
}
