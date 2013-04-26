using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Gecko;

namespace SampleApp
{
	public partial class GeckoTestDlg : Form
	{
	    private readonly HtmlButton _usbButton;
		private readonly HtmlButton _hubButton;
        private readonly HtmlButton _internetButton;
        private readonly HtmlButton _settingButton;
        private readonly HtmlButton _graphiteButton;
	    private readonly HtmlButton _toggleInsertUsbFlashDrive;
		private GeckoWebBrowser _browser;
		private bool _internetVisible = true;
		private bool _internetLayedOut = true;
		private string _internetLabelText = "";
		private Timer _timer;

		public GeckoTestDlg()
		{
			InitializeComponent();
			_browser = new GeckoWebBrowser {Dock = DockStyle.Fill};
			_browser.Navigate("file://" + Path.Combine(Environment.CurrentDirectory, "GeckoTestDlg.htm"));
			Controls.Add(_browser);
			_usbButton = new HtmlButton(_browser, "UsbButton");
			_hubButton = new HtmlButton(_browser, "ChorusHubButton");
			_internetButton = new HtmlButton(_browser, "InternetButton");
		    _settingButton = new HtmlButton(_browser, "settingsButton");
		    _graphiteButton = new HtmlButton(_browser, "enableGraphiteButton");
            _toggleInsertUsbFlashDrive = new HtmlButton(_browser, "toggleInsertUsbFlashDrive");
            _usbButton.Clicked += OnUsbClicked;
			_hubButton.Clicked += OnHubClicked;
		    _settingButton.Clicked += OnSettingsClicked;
			_internetButton.Clicked += OnInternetClicked;
		    _graphiteButton.Clicked += OnGraphiteButtonClicked;
		    _toggleInsertUsbFlashDrive.Clicked += OnToggleInsertUsbFlashDriveButtonClicked;
			_timer = new Timer();
			_timer.Tick += OnTimerTick;
			_timer.Interval = 5000;
			_timer.Start();
            Size = new Size(640, 480);
		}

	    private void OnGraphiteButtonClicked(object sender, EventArgs e)
	    {
	        GraphiteEnabled = !GraphiteEnabled;
	    }

	    private bool GraphiteEnabled
	    {
	        get { return (bool) GeckoPreferences.User["gfx.font_rendering.graphite.enabled"]; }
            set { GeckoPreferences.User["gfx.font_rendering.graphite.enabled"] = value; }
	    }

	    private void OnSettingsClicked(object sender, EventArgs e)
        {
            var chorusSendReceiveSettingsDialog = new ChorusSendReceiveSettingsDialog();
            chorusSendReceiveSettingsDialog.Size = new Size(550, 530);
            chorusSendReceiveSettingsDialog.Show();
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
			var internetSection = _browser.Document.GetElementById("internet");
			internetSection.SetAttribute("style", String.Format("display : {0}; visibility : {1}",
			                                                    _internetLayedOut ? "inline" : "none",
			                                                    _internetVisible ? "visible" : "hidden"));
			var internetLabel = _browser.Document.GetElementById("internet-label");
			internetLabel.TextContent = _internetLabelText;
		}

		private void OnToggleInsertUsbFlashDriveButtonClicked(object sender, EventArgs e)
	    {
	        _usbButton.Enable = !_usbButton.Enable;
			_browser.Document.GetElementById("usb-label").TextContent = "Found at z:/";
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

		private void OnInternetClicked(object sender, EventArgs e)
		{
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			_timer.Stop();
			_timer.Tick -= OnTimerTick;
			var hubLabel = _browser.Document.GetElementById("hub-label");
			hubLabel.TextContent = "Hello hub";
			hubLabel.SetAttribute("style", "font-style:italic");
		}
	}
}
