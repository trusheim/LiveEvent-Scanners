using System;

using System.Collections.Generic;
using System.Text;
using Symbol.MT2000.ScannerServices;
using System.Windows.Forms;

namespace SU_MT2000_SUIDScanner
{
    /// <summary>
    /// LEDManager allows easy manipulation of the green/red state of the MT2070 LED. The key feature
    /// is being able to turn on the LED for a specified color for a specified period of time.
    /// 
    /// The green LED sometimes turns itself off - this seems to be an undocumented feature of the MT2070
    /// interface.
    /// </summary>
    class LEDManager
    {
        #region Singleton setup
        private static LEDManager instance;
        public static LEDManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LEDManager();
                }
                return instance;
            }
        }
        #endregion

        #region Public methods
        public static void ShowRed() {
            ShowRed(-1);
        }
        public static void ShowRed(int milliseconds) {
            LEDManager thisManager = Instance;
            if (milliseconds > 0)
            {
                thisManager.SetTime(milliseconds);
            }
            thisManager.SetRedLED(true);
        }

        public static void ShowGreen()
        {
            ShowGreen(-1);
        }
        public static void ShowGreen(int milliseconds)
        {
            LEDManager thisManager = Instance;
            if (milliseconds > 0)
            {
                thisManager.SetTime(milliseconds);
            }
            thisManager.SetGreenLED(true);
        }
        #endregion

        #region Instance methods
        protected ScannerServicesClient scannerServices;
        protected bool greenLEDOn = false;
        protected bool redLEDOn = false;
        protected Timer clearLEDTimer = null;

        /// <summary>
        /// Singleton instance constructor
        /// </summary>
        protected LEDManager()
        {
            this.scannerServices = Program.ScannerServicesClient;

            if (!this.scannerServices.IsConnected)
            {
                throw new Exception("Cannot connect to scanner services, failed!");
            }

            clearLEDTimer = new Timer();
            clearLEDTimer.Tick += new EventHandler(clearLEDTimer_Tick);
            clearLEDTimer.Interval = 5000;
            clearLEDTimer.Enabled = false;
        }

        ~LEDManager()
        {
            this.scannerServices.ExecuteUIFCommand(UIF_COMMAND.SVC_UIF_GREEN_LED_OFF);
            this.scannerServices.ExecuteUIFCommand(UIF_COMMAND.SVC_UIF_RED_LED_OFF);
        }

        public void clearLEDTimer_Tick(object sender, EventArgs e)
        {
            clearLEDTimer.Enabled = false;
            SetGreenLED(false);
            SetRedLED(false);
        }

        public void SetGreenLED(bool newState)
        {
            clearLEDTimer.Enabled = false;
            // it seems that the green LED automatically turns off?

            //if (this.greenLEDOn == newState)
            //{
            //    return;
            //}
            if (newState)
            {
                this.scannerServices.ExecuteUIFCommand(UIF_COMMAND.SVC_UIF_GREEN_LED_ON);
                if (this.redLEDOn)
                {
                    this.scannerServices.ExecuteUIFCommand(UIF_COMMAND.SVC_UIF_RED_LED_OFF);
                    this.redLEDOn = false;
                }
            }
            else
            {
                this.scannerServices.ExecuteUIFCommand(UIF_COMMAND.SVC_UIF_GREEN_LED_OFF);
            }

            this.greenLEDOn = newState;
            
            if (newState) this.clearLEDTimer.Enabled = true;
        }

        public void SetRedLED(bool newState)
        {
            clearLEDTimer.Enabled = false;

            if (newState)
            {
                this.scannerServices.ExecuteUIFCommand(UIF_COMMAND.SVC_UIF_RED_LED_ON);
                if (this.greenLEDOn)
                {
                    this.scannerServices.ExecuteUIFCommand(UIF_COMMAND.SVC_UIF_GREEN_LED_OFF);
                    this.greenLEDOn = false;
                }
            }
            else
            {
                this.scannerServices.ExecuteUIFCommand(UIF_COMMAND.SVC_UIF_RED_LED_OFF);
            }

            this.redLEDOn = newState;

            if (newState) this.clearLEDTimer.Enabled = true;
        }

        public void SetTime(int milliseconds)
        {
            this.clearLEDTimer.Interval = milliseconds;
        }
        #endregion
    }
}
