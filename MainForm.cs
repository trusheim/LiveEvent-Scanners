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

        // ID processor
        private CardProcessor processor = null;
        private ScanLog scanLog = null;

        private bool forceMode = false;
        private bool allAccessMode = false;

        ScannerServicesClient scannerServices;

		/// <summary>
		/// initializes the form
		/// </summary>
		public MainForm()
		{
            this.scannerServices = Program.ScannerServicesClient;

            // ID processor framework
            try
            {
                processor = new CardProcessor();
                AdmitList.SetupProcessorFromFile("lol", processor);
            }
            catch (Exception e)
            {
                MsgBox.Error(null, e.Message);
                this.Close();
                return;
            }

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
            menu.Add(new MenuDataItem("Etc", "etc", null, Config.CloseBitmap));
            
            readLabelEvent = new ReadLabelEventDelegate(ReadLabelEventCallback);

            // Display timer
            clearTimer = new Timer();
            clearTimer.Tick += new EventHandler(clearTimer_Tick);
            clearTimer.Interval = 5000;
            clearTimer.Enabled = false;

            // Add event handlers. 
            this.Activated += new EventHandler(FormActivated);
            this.Deactivate += new EventHandler(FormDeactivated);

            this.LeftSoftKeyPressed += new System.EventHandler(this.LeftSoftKeyPressedHandler);
            this.RightSoftKeyPressed += new System.EventHandler(this.RightSoftKeyPressedHandler);
            this.scannerServices.ExecuteUIFCommand(UIF_COMMAND.BC_APP_CLICK);
		}

        private void FormActivated(object sender, EventArgs e)
        {

            if (!Program.ScannerServicesClient.Connect(true))
            {
                MsgBox.Show(null, Properties.Resources.StrSUIDScanner,Properties.Resources.StrErrorCannotConnect);
                this.Close();
            }
            if (RESULTCODE.E_OK != Program.ScannerServicesClient.SetMode(SCANNERSVC_MODE.SVC_MODE_DECODE))
            {
                MsgBox.Show(null, Properties.Resources.StrSUIDScanner, Properties.Resources.StrErrorCannotConnect);
                Program.ScannerServicesClient.Disconnect();
                Program.ScannerServicesClient.Dispose();
                this.Close();
            }

            // Attach it to the ReadLabelEvent property
            this.scannerServices.ReadLabelEvent += new ScannerServicesClient.ReadLabelHandler(this.ReadLabelEventCallback);

            // set scanner modes
            this.scannerServices.Attributes.System.BeepOnGoodDecode.Value = false;

            // start an asynchronous scanner read
            this.scannerServices.BeginReadLabel();
        }

        private void FormDeactivated(object sender, EventArgs e)
        {
            clearDisplay();
            Program.ScannerServicesClient.ReadLabelEvent -= new ScannerServicesClient.ReadLabelHandler(this.ReadLabelEventCallback);
            this.scannerServices.Attributes.System.BeepOnGoodDecode.Value = true;
            Program.ScannerServicesClient.Disconnect();
        }

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

            if (e.Result == RESULTCODE.E_OK)
            {
                string barcode_id = e.LabelData.Text;
                AdmitInfo admitInfo = processor.TryAdmit(barcode_id);

                if (admitInfo.status == AdmitStatus.OKAY)
                {
                    this.scannerServices.ExecuteUIFCommand(UIF_COMMAND.BC_SHORT_HI2);
                    LEDManager.ShowGreen();
                    this.DisplayAccess(admitInfo.message.big_message, admitInfo.message.small_message);
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
            Program.ScannerServicesClient.BeginReadLabel();
        }

        private void DisplayAccess(string bigMessage, string smallMessage)
        {
            this.PleaseScanLabel.Visible = false;
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
            this.BigLabel.Text = big_message;
            this.BigLabel.Visible = true;

            this.SmallInfo.Text = small_message;
            this.SmallInfo.Visible = true;
            this.BackColor = System.Drawing.Color.Red;
        }

        private void clearDisplay()
        {
            this.PleaseScanLabel.Visible = true;
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

                else if (menuItem.Command == "etc")
                {
                    BlankForm f = new BlankForm();
                    f.Show();
                }
            }
        }

        private void Save()
        {
            this.ShowSpinner = true;
            bool ok = false; // used to be the .Save() action
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

	}
}
