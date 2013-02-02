using System;
using System.Windows.Forms;

using Symbol.MT2000.UserInterface;
using Symbol.MT2000.ScannerServices;

namespace SU_MT2000_SUIDScanner
{
	public partial class MainForm : Symbol.MT2000.UserInterface.BaseForm
	{

        // UI features
        private MenuDataList menu = null;
        private Timer clearTimer = null;

        // Label reading
        private delegate void ReadLabelEventDelegate(object sender, ReadLabelEventArgs e);
        private ReadLabelEventDelegate readLabelEvent = null;
        private ScannerServicesClient.ReadLabelHandler RLHandler;

        // ID processor
        public static string admit_file = null;
        public static bool admit_processed = false;
        private bool in_processing = false;
        private CardProcessor processor = null;

        private bool forceMode = false;
        private bool allAccessMode = false;
        private int numAdmitted = 0;

        ScannerServicesClient scannerServices;

        #region Constructors/Destructors
        /// <summary>
		/// initializes the form
		/// </summary>
		public MainForm()
		{
            this.scannerServices = Program.ScannerServicesClient;

            // initalize UI
			InitializeComponent();
            TitleText = "";
			LeftSoftKeyText = "Menu";
            RightSoftKeyText = "Force";

            menu = new MenuDataList();
            menu.ShowIcons = true;
            //menu.Add(new MenuDataItem("Force Admit", "force", null, Config.EditBitmap));
            menu.Add(new MenuDataItem("Save Admits", "save", null, Config.SaveBitmap));
            menu.Add(new MenuDataItem("Sales Mode", "sales", null, null));
            menu.Add(new MenuDataItem("Exit", "exit", null, Config.CloseBitmap));
            
            readLabelEvent = new ReadLabelEventDelegate(ReadLabelEventCallback);
            RLHandler = new ScannerServicesClient.ReadLabelHandler(this.ReadLabelEventCallback);

            this.LeftSoftKeyPressed += new System.EventHandler(this.LeftSoftKeyPressedHandler);
            this.RightSoftKeyPressed += new System.EventHandler(this.RightSoftKeyPressedHandler);

            // Display timer
            clearTimer = new Timer();
            clearTimer.Tick += new EventHandler(clearTimer_Tick);
            clearTimer.Interval = 5000;
            clearTimer.Enabled = false;

            // Add event handlers. 
            this.Activated += new EventHandler(FormActivated);
            this.Deactivate += new EventHandler(FormDeactivated);
		}

        private void FormActivated(object sender, EventArgs e)
        {
            // Attach the processing to the ReadLabelEvent property
            this.scannerServices.ReadLabelEvent -= RLHandler;
            this.scannerServices.ReadLabelEvent += RLHandler;

            // set scanner modes
            this.scannerServices.Attributes.System.BeepOnGoodDecode.Value = false;

            // start an asynchronous scanner read
            this.scannerServices.BeginReadLabel();

            if (admit_processed == false)
            {
                SetupAdmitList();
            }
        }

        private void FormDeactivated(object sender, EventArgs e)
        {
            clearDisplay();
            Program.ScannerServicesClient.ReadLabelEvent -= RLHandler;
            this.scannerServices.Attributes.System.BeepOnGoodDecode.Value = true;
            this.scannerServices.CancelReadLabel();
            
        }

        public static void ChangeAdmitListFile(string newFile)
        {
            admit_file = newFile;
            admit_processed = false;
        }

        protected void SetupAdmitList()
        {
            if (in_processing) return;

            in_processing = true;
            // setup which file to use
            // show selection window for file (dectivates this window)
            SelectForm sForm = new SelectForm();
            sForm.ShowDialog();

            this.ShowSpinner = true;
            processor = AdmitList.SetupProcessorFromFile(MainForm.admit_file);
            this.ShowSpinner = false;

            // alert that we are done!
            this.scannerServices.ExecuteUIFCommand(UIF_COMMAND.BC_APP_CLICK);

            this.numAdmitted = 0;
            admit_processed = true;

            in_processing = false;
        }
        #endregion

        #region Scanning Actions
        /// <summary>
        /// receives and sends the scanner data
        /// </summary>
        private void ReadLabelEventCallback(object sender, ReadLabelEventArgs e)
        {
            // invoke this method if required
            if (this.InvokeRequired)
            {
                this.Invoke(readLabelEvent, sender, e);
                return;
            }

            if (MainForm.admit_processed == false)
            {
                return;
            }

            if (e.Result == RESULTCODE.E_OK)
            {
                string barcode_id = e.LabelData.Text;
                AdmitInfo admitInfo = processor.TryAdmit(barcode_id);

                if (admitInfo.status == AdmitStatus.OKAY)
                {
                    this.scannerServices.ExecuteUIFCommand(UIF_COMMAND.BC_SHORT_HI2);
                    LEDManager.ShowGreen();
                    this.DisplayAccess(admitInfo.message.big_message, admitInfo.message.small_message);
                    this.numAdmitted += 1;
                }
                else if (admitInfo.status == AdmitStatus.REPEAT)
                {
                    this.scannerServices.ExecuteUIFCommand(UIF_COMMAND.BC_LO_HI_LO);
                    LEDManager.ShowRed();
                    this.DisplayDeny(admitInfo.message.big_message, admitInfo.message.small_message);
                }
                else
                {
                    this.scannerServices.ExecuteUIFCommand(UIF_COMMAND.BC_SLOW_WARB);
                    LEDManager.ShowRed();
                    this.DisplayDeny(admitInfo.message.big_message,admitInfo.message.small_message);
                }

                // set timeouts
                this.clearTimer.Interval = 5000;
                this.clearTimer.Enabled = true;
            }
            else
            {
                MsgBox.Error(this, "Barcode could not be scanned.");
            }

            // start another read
            this.scannerServices.BeginReadLabel();
        }

        private void DisplayAccess(string bigMessage, string smallMessage)
        {
            this.PleaseScanLabel.Visible = false;
            this.numIn.Visible = false;
            if (bigMessage == "GO")
            {
                this.BigLabel.Visible = false;
                this.GoLabel.Visible = true;
            }
            else
            {
                this.GoLabel.Visible = false;
                this.BigLabel.Text = bigMessage;
                this.BigLabel.Visible = true;
            }

            this.SmallInfo.Text = smallMessage;
            this.SmallInfo.Visible = true;
            this.BackColor = System.Drawing.Color.Green;
        }

        private void DisplayDeny(string big_message, string small_message)
        {
            this.PleaseScanLabel.Visible = false;
            this.GoLabel.Visible = false;
            this.numIn.Visible = false;
            this.BigLabel.Text = big_message;
            this.BigLabel.Visible = true;

            this.SmallInfo.Text = small_message;
            this.SmallInfo.Visible = true;
            this.BackColor = System.Drawing.Color.Red;
        }

        private void clearDisplay()
        {
            this.PleaseScanLabel.Visible = true;
            this.numIn.Visible = true;
            this.numIn.Text = this.numAdmitted.ToString();

            this.GoLabel.Visible = false;
            this.BigLabel.Visible = false;
            this.SmallInfo.Visible = false;
            this.BackColor = System.Drawing.Color.White;
        }

        /// <summary>
        /// clears the barcode
        /// </summary>
        private void clearTimer_Tick(object sender, EventArgs e)
        {
            PleaseScanLabel.Text = "Scan a SUID";
            clearTimer.Enabled = false;
            clearDisplay();
        }
        #endregion

        #region Other interaction
        private void LeftSoftKeyPressedHandler(object sender, EventArgs e) 
        {
            menu.SelectedItemIndex = 0;
            this.HighlightLeftSoftKey = true;
            MenuDataItem menuItem = PopupMenu.Show(this, menu);
            this.HighlightLeftSoftKey = false;
            if (menuItem != null)
            {
                // save the inventory
                if (menuItem.Command == "save")
                {
                    Save();
                }
                else if (menuItem.Command == "sales")
                {
                    this.allAccessMode = !this.allAccessMode;
                    ToggleForceMode();
                }
                else if (menuItem.Command == "exit")
                {
                    Save();
                    clearDisplay();
                    Close();
                }
            }
        }

        private void Save()
        {
            this.ShowSpinner = true;
            bool ok = true; // used to be the .Save() action
            //TODO: fix and add back in if necessary
            this.ShowSpinner = false;
            if (ok)
            {
                MsgBox.Show(this, "Saved", "Saved admit list");
            }
            else
            {
                MsgBox.Error(this, "Could not save admit list");
            }
        }

        private void RightSoftKeyPressedHandler(object sender, EventArgs e)
        {
            ToggleForceMode();
        }

        private void ToggleForceMode()
        {
            if (!forceMode)
            {
                forceMode = true;
                this.HighlightRightSoftKey = true;
            }
            else if (forceMode && !allAccessMode) 
            {
                forceMode = false;
                this.HighlightRightSoftKey = false;
            }
        }
        #endregion
    }
}
