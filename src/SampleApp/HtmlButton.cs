using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gecko;

namespace SampleApp
{
    public class HtmlButton
    {
        private readonly string _buttonName;
        public event EventHandler Clicked;
        private readonly GeckoWebBrowser _browser;

        public HtmlButton(GeckoWebBrowser browser, string buttonName)
        {
            _browser = browser;
            _buttonName = buttonName;
            browser.DomClick += browser_DomClick;
        }

        void browser_DomClick(object sender, GeckoDomEventArgs e)
        {
            GeckoHtmlElement element = e.Target;
            if (element.Id == _buttonName)
            {
                if (Clicked != null)
                {
                    Clicked(this, new EventArgs());
                }
            }
        }

        public bool Enable
        {
            get
            {
                using (var context = new AutoJSContext(_browser.JSContext))
                {
                    string result;
                    context.EvaluateScript(
                        String.Format("document.getElementById('{0}').disabled", _buttonName), out result);
                    return result == "true" ? false : true;
                }
            }
            set
            {
                using (var context = new AutoJSContext(_browser.JSContext))
                {
                    string result;
                    var valueToSet = value ? "false" : "true";
                    context.EvaluateScript(
                        String.Format("document.getElementById('{0}').disabled = {1}", _buttonName, valueToSet),
                        out result);
                }
            }
        }
    }
}
