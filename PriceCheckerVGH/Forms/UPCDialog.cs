using PriceCheckerVGH;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSValidator.Forms
{
    public partial class UPCDialog : Form
    {
        CoreRunner runnerRef;
        public UPCDialog(CoreRunner runner)
        {
            InitializeComponent();
            runnerRef = runner;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void UPCDialog_Load(object sender, EventArgs e)
        {

        }

       

        private void UPCBox_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private async void UPCBox_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var upc = UPCBox.Text;
                UPCBox.Clear();
                await runnerRef.addGame(upc);
                label2.Text = runnerRef.status;
            }
        }

        private void UPCBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
