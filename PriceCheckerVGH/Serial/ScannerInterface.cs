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
        public string status="null";
        public ScannerInterface()
        {
            
        }

        public void writeToLcd(string topRow, string bottomRow)
        {
            WriteLine(topRow+"-"+bottomRow);
            return;
        }

        public void connect()
        {
            if(IsOpen)
            {
                return;
            }
            PortName = ("COM7");
            DataBits = 8;

            status = "Connecting to COM...";
            ReadTimeout = 30000;

            try
            {
                Open();
            }

            catch (Exception ex)
            {
                status = ("Failed to connect to device.");
            }

            if (IsOpen)
            {
                status = ("Connected to COM port.");
            }
        }
    }
}
