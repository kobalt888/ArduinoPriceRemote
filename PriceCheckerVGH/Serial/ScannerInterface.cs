using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace PriceCheckerVGH
{
    public class ScannerInterface : SerialPort
    {
        public ScannerInterface()
        {
            PortName = ("COM7");
            DataBits = 8;
            Console.WriteLine("Connecting to COM...");
            ReadTimeout = 30000;

            try
            {
                Open();
            }

            catch (Exception ex)
            {
                
                Console.WriteLine(ex);
            }

            if (IsOpen)
            {
                Console.WriteLine("Connected to COM port.");
            }
        }

        public void writeToLcd(string topRow, string bottomRow)
        {
            WriteLine(topRow+"-"+bottomRow);
            return;
        }

    }
}
