//--------------------------------------------------------------------
// Stanford LiveEvent / LiveID scanner technology
//
// Copyright (c) 2011-2013 Stanford University
// Primary developer: Stephen Trusheim 
//        (trusheim@stanford.edu / stephen@trushe.im)
// See README for licensing information
//
// Based on Symbol Technologies ScanInventory sample application
// copyright (c) 2009 Symbol Technologies, Inc.
//--------------------------------------------------------------------
using System;
using System.Windows.Forms;
using Microsoft.Win32;

using Symbol.MT2000.Utils;
using Symbol.MT2000.UserInterface;
using Symbol.MT2000.ScannerServices;

namespace SU_MT2000_SUIDScanner
{
	static class Program
	{
		// public variables
		public static ScannerServicesClient ScannerServicesClient = null;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[MTAThread]
		static void Main()
		{
			// create the scanner services interface
			if (!ScannerServicesClient.IsServiceRunning)
			{
				MsgBox.Show(null, Properties.Resources.StrSUIDScanner, Properties.Resources.StrErrorCannotConnect);
				return;
			}
			ScannerServicesClient = new ScannerServicesClient();

            // Test connection..
			if (!ScannerServicesClient.Connect(true))
			{
				MsgBox.Show(null, Properties.Resources.StrSUIDScanner, Properties.Resources.StrErrorCannotConnect);
				return;
			}
			if (RESULTCODE.E_OK != ScannerServicesClient.SetMode(SCANNERSVC_MODE.SVC_MODE_DECODE))
			{
				MsgBox.Show(null, Properties.Resources.StrSUIDScanner, Properties.Resources.StrErrorCannotConnect);
				ScannerServicesClient.Disconnect();
				ScannerServicesClient.Dispose();
				return;
			}

            ScannerServicesClient.Disconnect();

			// run the application
			SystemMonitor.Start();
			Application.Run(new MainForm());
			SystemMonitor.Stop();

            //Inventory.Save();

			// shut down the scanner interface
			if (ScannerServicesClient != null)
			{
                ScannerServicesClient.Connect(false);
				ScannerServicesClient.Disconnect();
				ScannerServicesClient.Dispose();
				ScannerServicesClient = null;
			}
		}
	}
}