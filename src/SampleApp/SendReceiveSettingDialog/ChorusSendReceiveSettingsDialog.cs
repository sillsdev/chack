using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gecko;

namespace SampleApp
{
    public partial class ChorusSendReceiveSettingsDialog : Form
    {
        private GeckoWebBrowser _browser;
		private readonly HtmlButton _internetTab;
		private readonly HtmlButton _chorusHubTab;

        public ChorusSendReceiveSettingsDialog()
        {
            InitializeComponent();
            _browser = new GeckoWebBrowser { Dock = DockStyle.Fill };
            var pathToHtml = Path.Combine(Path.Combine(Environment.CurrentDirectory, "SendReceiveSettingDialog"),
                                          "ChorusSendReceiveSettings.htm");
            _browser.Navigate("file://" + pathToHtml);
            Controls.Add(_browser);
			_internetTab = new HtmlButton(_browser, "tab-1");
			_internetTab.Clicked += OnTabClicked;
			_chorusHubTab = new HtmlButton(_browser, "tab-2");
			_chorusHubTab.Clicked += OnTabClicked;
		}

		private void OnTabClicked(object sender, EventArgs e)
		{
			SwitchTab(((HtmlButton)sender).Name);
		}

		private void SwitchTab(string button)
		{
			int activeTab, nonActiveTab;
			switch (button)
			{
				case "tab-1":
					activeTab = 1;
					nonActiveTab = 2;
					break;
				case "tab-2":
					activeTab = 2;
					nonActiveTab = 1;
					break;
			}
			_browser.Document.GetElementById(string.Format("fragment-{0}", activeTab)).SetAttribute("style", "display: block");
			_browser.Document.GetElementById(string.Format("fragment-{0}", nonActiveTab)).SetAttribute("style", "display: none");
			_browser.Document.GetElementById(string.Format("tab-{0}", activeTab)).SetAttribute("style", "border-bottom: 1px solid white");
			_browser.Document.GetElementById(string.Format("tab-{0}", activeTab)).SetAttribute("class","active-tab");
			_browser.Document.GetElementById(string.Format("tab-{0}", nonActiveTab)).SetAttribute("style","border-bottom: 1px solid lightgrey");
			_browser.Document.GetElementById(string.Format("tab-{0}", nonActiveTab)).SetAttribute("class","non-active-tab");
		}
	}
}
