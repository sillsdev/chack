using System;
using Gecko;

namespace SampleApp
{
    public class HtmlButton
    {
        public event EventHandler Clicked;
        private readonly GeckoWebBrowser _browser;

        public HtmlButton(GeckoWebBrowser browser, string buttonName)
        {
            _browser = browser;
			Name = buttonName;
            browser.DomClick += browser_DomClick;
        }

        void browser_DomClick(object sender, GeckoDomEventArgs e)
        {
            GeckoHtmlElement element = e.Target;
			if (element.Id == Name)
            {
                if (Clicked != null)
                {
                    Clicked(this, new EventArgs());
                }
            }
        }

		public string Name { get; private set; }

        public bool Enable
        {
            get
            {
                using (var context = new AutoJSContext(_browser.JSContext))
                {
                    string result;
                    context.EvaluateScript(
                        String.Format("document.getElementById('{0}').disabled", Name), out result);
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
                        String.Format("document.getElementById('{0}').disabled = {1}", Name, valueToSet),
                        out result);
                }
            }
        }
    }
}
