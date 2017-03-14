using System;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace PriceCheckerVGH
{
    class Program
    {
        static void Main(string[] args)
        {
            string lastScan=null;
            Console.WriteLine("Enter Y and enter at any time to exit scanner.");
            
            
            SerialPort scanner = new SerialPort("COM7");
            scanner.DataBits = 8;
            Console.WriteLine("Connecting to COM...");
            scanner.ReadTimeout = 30000;

            Core coreProcess = new Core();
            try
            {
                scanner.Open();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


            Boolean statusflag = true;
            if (scanner.IsOpen)
            {
                Console.WriteLine("Connected to COM port.");
            }
            while (statusflag)
            {
                var input = Console.ReadLine();

                if (input.Length < 5)
                {
                    statusflag = false;
                }
                else if(lastScan == input)
                {
                    var gameCallStatus = coreProcess.writeGame();
                    if (gameCallStatus > 0)
                    {
                        scanner.WriteLine(coreProcess.gTitle + "-Added to .csv");
                    }
                    lastScan = null;
                }
                else
                {
                    coreProcess.getPrice(input).Wait();
                    var gameCallStatus = coreProcess.flag;
                    if (gameCallStatus == "OK")
                    {
                        if (coreProcess.cost == null)
                        {
                            scanner.Write("No price found.");
                        }
                        else
                        {
                            scanner.WriteLine(coreProcess.gTitle + "-" + coreProcess.cost);
                            lastScan = input;
                            Console.WriteLine("Game found in db");
                        }
                    }
                }

            }

            return; 
        }

    }
}

