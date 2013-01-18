using System;
using System.Windows.Forms;

using Symbol.MT2000.UserInterface;
using Symbol.MT2000.ScannerServices;

namespace SU_MT2000_SUIDScanner
{
    public partial class SelectForm : Symbol.MT2000.UserInterface.ListForm
    {
        /// <summary>
        /// initializes the form
        /// </summary>
        public SelectForm()
        {
            // initialize the controls 
            InitializeComponent();
            Application.DoEvents();

            // create the screens
            Screens.Add("select", new SelectScreen(this));


            // Add event handlers. 
            this.Activated += new EventHandler(FormActivated);
            this.Deactivate += new EventHandler(FormDeactivated);


            // display the main screen
            PushScreen(Screens["select"]);
        }
        private void FormActivated(object sender, EventArgs e)
        {
        }
        private void FormDeactivated(object sender, EventArgs e)
        {
        }

    }
}
