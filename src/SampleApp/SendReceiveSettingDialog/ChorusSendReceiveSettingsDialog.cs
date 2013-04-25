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

        public ChorusSendReceiveSettingsDialog()
        {
            InitializeComponent();
            _browser = new GeckoWebBrowser { Dock = DockStyle.Fill };
            var pathToHtml = Path.Combine(Path.Combine(Environment.CurrentDirectory, "SendReceiveSettingDialog"),
                                          "ChorusSendReceiveSettings.htm");
            _browser.Navigate("file://" + pathToHtml);
            Controls.Add(_browser);
        }
    }
}
