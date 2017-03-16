using GSValidator.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PriceCheckerVGH.Forms
{
    
    public partial class GameView : Form
    {
        List<Game> loadedGames = new List<Game>();
        CoreRunner runner = new CoreRunner();

        private string filePath;
        public GameView()
        {
            InitializeComponent();
            ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Invoker csvSelect = new Invoker();
                csvSelect.Invoke();
                filePath = csvSelect.InvokeDialog.FileName;
                var csvFile = File.ReadAllLines(filePath).Select(a => a.Split(','));
                var count = 0;
                foreach (var value in csvFile)
                {
                    if (count>0)//So we skip the first line of csv that has formatting for readability of data
                    {
                        var temp = value[0].PadRight(15);
                        listBox1.Items.Add(temp + value[1]);
                        Game iteratedGame = new Game();
                        iteratedGame.console = value[0];
                        iteratedGame.title = value[1];
                        iteratedGame.price = value[2];
                        iteratedGame.upc = value[3];

                        loadedGames.Add(iteratedGame);
                    }
                    count++;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GameDataBox.Clear();
            GameDataBox.Text = (loadedGames[this.listBox1.SelectedIndex].console + "\n");
            GameDataBox.AppendText(loadedGames[this.listBox1.SelectedIndex].title + "\n");
            GameDataBox.AppendText(loadedGames[this.listBox1.SelectedIndex].price + "\n");
            GameDataBox.AppendText(loadedGames[this.listBox1.SelectedIndex].upc + "\n");
            Clipboard.SetText(loadedGames[listBox1.SelectedIndex].upc);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.SelectedIndex += 1;
        }

        private void GameDataBox_TextChanged(object sender, EventArgs e)
        {
            Clipboard.SetText(loadedGames[listBox1.SelectedIndex].upc);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            
        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (!runner.arduino.IsOpen)
            {
                runner.connect();
                SerialStatusBox.Text = runner.arduino.status;
                button1.Text = "Scan";
            }
            else
            {
                UPCDialog upcWindow = new UPCDialog(runner);
                upcWindow.ShowDialog();
            }
            
        }

        
    }
}

