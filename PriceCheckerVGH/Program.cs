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
            Console.WriteLine("Enter Y and enter at any time to exit scanner.");

            SerialPort scanner = new SerialPort("COM7");
            scanner.DataBits = 8;
            Console.WriteLine("Connecting to COM...");
            scanner.ReadTimeout = 30000;
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
                else
                {
                    Core coreProcess = new Core();
                    coreProcess.getPrice(input).Wait();
                    scanner.WriteLine(coreProcess.cost);
                }

            }

            return; 
        }

    }
}

