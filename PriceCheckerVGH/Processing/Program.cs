using PriceCheckerVGH.Forms;
using System;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PriceCheckerVGH
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            bool statusflag = true;
            Console.WriteLine("###############      GS Validator       #################");
            Console.WriteLine("##### Hit enter at any time to begin updating prices ####");
            Console.WriteLine("#########################################################\n\n");


            string programMode="add"; //default program to adding games mode at start
            CoreRunner runner = new CoreRunner(); //runs the functions in core.cs as prescribed by ^string programMode
            

            //Main program loop

            while (statusflag)
            {
                if (programMode == "add")
                {
                    var result = runner.addGame();
                    if (result == -1)
                    {
                        programMode = "enter";
                        ListViewDataManager manager = new ListViewDataManager();
                        Console.WriteLine("Press enter again to resume game adding.");
                    }
                }
                else if (programMode == "enter")
                {
                    var result = runner.addGame();
                    if (result == -1)
                    {
                        programMode = "add";
                        Console.WriteLine("Hit enter at any time to begin updating prices");
                    }
                }
            }


            return; 
        }
    }
}

