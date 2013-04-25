using System;
using System.IO;
using System.Windows.Forms;
using Gecko;

namespace SampleApp
{
	public partial class GeckoTestDlg : Form
	{
	    private readonly HtmlButton _usbButton;
		private readonly HtmlButton _hubButton;
	    private readonly HtmlButton _toggleInsertUsbFlashDrive;
		private GeckoWebBrowser browser;
		private bool _internetVisible = true;
		private bool _internetLayedOut = true;
		private string _internetLabelText = "";
		private Timer _timer;

		public GeckoTestDlg()
		{
			InitializeComponent();
			browser = new GeckoWebBrowser {Dock = DockStyle.Fill};
			browser.Navigate("file://" + Path.Combine(Environment.CurrentDirectory, "GeckoTestDlg.htm"));
			Controls.Add(browser);
			_usbButton = new HtmlButton(browser, "UsbButton");
			_hubButton = new HtmlButton(browser, "ChorusHubButton");
            _toggleInsertUsbFlashDrive = new HtmlButton(browser, "toggleInsertUsbFlashDrive");
            _usbButton.Clicked += OnUsbClicked;
			_hubButton.Clicked += OnHubClicked;
		    _toggleInsertUsbFlashDrive.Clicked += OnToggleInsertUsbFlashDriveButtonClicked;
			_timer = new Timer();
			_timer.Tick += OnTimerTick;
			_timer.Interval = 5000;
			_timer.Start();
		}

		/// <summary>
		/// I wrote this to show the diference between setting the display and the visibility settings.
		/// Also it should be noted that this will override any style settings that are put up at the document level but only for
		/// the styles used here. (I noticed some oddities for the way elements internal to the paragraph dealt with the style
		/// GeckoFx may not handle cascading style sheets perfectly when you are messing with the DOM. Or else I don't understand
		/// how it should work completely)
		/// </summary>
		private void AdjustInternetSection()
		{
			var internetSection = browser.Document.GetElementById("internet");
			internetSection.SetAttribute("style", String.Format("display : {0}; visibility : {1}",
			                                                    _internetLayedOut ? "inline" : "none",
			                                                    _internetVisible ? "visible" : "hidden"));
			var internetLabel = browser.Document.GetElementById("internet-label");
			internetLabel.TextContent = _internetLabelText;
		}

		private void OnToggleInsertUsbFlashDriveButtonClicked(object sender, EventArgs e)
	    {
	        _usbButton.Enable = !_usbButton.Enable;
	    }

	    private void OnUsbClicked(object sender, EventArgs e)
	    {
			_internetLayedOut = !_internetLayedOut;
			_internetLabelText = "USB clicking happened last.";
			AdjustInternetSection();
	    }

		private void OnHubClicked(object sender, EventArgs e)
		{
			_internetVisible = !_internetVisible;
			_internetLabelText = "Hub clicking happened last.";
			AdjustInternetSection();
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			_timer.Stop();
			_timer.Tick -= OnTimerTick;
			var hubLabel = browser.Document.GetElementById("hub-label");
			hubLabel.TextContent = "Hello hub";
			hubLabel.SetAttribute("style", "font-style:italic");
		}
	}
}
