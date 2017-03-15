using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PriceCheckerVGH
{
    public class Invoker
    {
        public OpenFileDialog InvokeDialog;
        private Thread InvokeThread;
        private DialogResult InvokeResult;

        public Invoker()
        {
            InvokeDialog = new OpenFileDialog();
            InvokeDialog.Title = "Select a .CSV";
            InvokeDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            InvokeDialog.Filter = "CSV Files (*.csv)|*.csv";
            InvokeDialog.RestoreDirectory = true;
            InvokeThread = new Thread(new ThreadStart(InvokeMethod));
            InvokeThread.SetApartmentState(ApartmentState.STA);
            InvokeResult = DialogResult.None;
        }

        public DialogResult Invoke()
        {
            InvokeThread.Start();
            InvokeThread.Join();
            return InvokeResult;
        }

        private void InvokeMethod()
        {
            InvokeResult = InvokeDialog.ShowDialog();
        }
    }
}
