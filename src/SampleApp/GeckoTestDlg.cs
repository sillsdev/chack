using System;
using System.IO;
using System.Windows.Forms;
using Gecko;

namespace SampleApp
{
	public partial class GeckoTestDlg : Form
	{
		public GeckoTestDlg()
		{
			InitializeComponent();
			var browser = new GeckoWebBrowser {Dock = DockStyle.Fill};
			browser.Navigate("file://" + Path.Combine(Environment.CurrentDirectory, "GeckoTestDlg.htm"));
			Controls.Add(browser);
			browser.DomClick += browser_DomClick;
		}

		void browser_DomClick(object sender, GeckoDomEventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}
