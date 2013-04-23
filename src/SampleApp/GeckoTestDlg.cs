using System;
using System.IO;
using System.Windows.Forms;
using Gecko;

namespace SampleApp
{
	public partial class GeckoTestDlg : Form
	{
	    private readonly HtmlButton _usbButton ;
	    private readonly HtmlButton _toggleInsertUsbFlashDrive;

		public GeckoTestDlg()
		{
			InitializeComponent();
			var browser = new GeckoWebBrowser {Dock = DockStyle.Fill};
			browser.Navigate("file://" + Path.Combine(Environment.CurrentDirectory, "GeckoTestDlg.htm"));
			Controls.Add(browser);
		    _usbButton = new HtmlButton(browser, "UsbButton");
            _toggleInsertUsbFlashDrive = new HtmlButton(browser, "toggleInsertUsbFlashDrive");
            _usbButton.Clicked += OnUsbClicked;
		    _toggleInsertUsbFlashDrive.Clicked += OnToggleInsertUsbFlashDriveButtonClicked;
		}

	    private void OnToggleInsertUsbFlashDriveButtonClicked(object sender, EventArgs e)
	    {
	        _usbButton.Enable = !_usbButton.Enable;
	    }

	    private void OnUsbClicked(object sender, EventArgs e)
	    {
	    }
	}
}
