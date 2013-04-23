﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Chorus.VcsDrivers.Mercurial;
using Gecko;
using System.Runtime.InteropServices;

namespace Chorus
{
    static class Program
    {
                // dummy function to dlopen geckofix
        [DllImport("geckofix.so")]
        static extern void DummyFunction();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SetUpErrorHandling();
			
			// Set up Xpcom for geckofx
#if MONO	
#if false // TODO: renable this - when making geckofx work in Chorus
			DummyFunction();

			if (!Environment.GetEnvironmentVariable("LD_LIBRARY_PATH").Contains("/usr/lib/firefox/"))
				throw new ApplicationException(String.Format("LD_LIBRARY_PATH must contain {0}", "/usr/lib/firefox/"));

			Xpcom.Initialize("/usr/lib/firefox/");
			GeckoPreferences.User["gfx.font_rendering.graphite.enabled"] = true;
#endif
#else
			Xpcom.Initialize(XULRunnerLocator.GetXULRunnerLocation());
#endif
			Application.ApplicationExit += (sender, e) => 
			{
        		Xpcom.Shutdown();
			};

		//	throw new ApplicationException("test");

            //is mercurial set up?
            var s = HgRepository.GetEnvironmentReadinessMessage("en");
            if (!string.IsNullOrEmpty(s))
            {
                MessageBox.Show(s, "Chorus", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            //did they give us a path on the command line?
            string pathToRepository =null;
            if(args.Length > 0)
            {
                pathToRepository = args[0];
            }

            if (string.IsNullOrEmpty(pathToRepository) || !Directory.Exists(pathToRepository))
            {
                //do we have a valid path from last time?
                pathToRepository = Properties.Settings.Default.PathToRepository;
            }

            if (string.IsNullOrEmpty(pathToRepository) || !Directory.Exists(pathToRepository))
            {
                //can they find a repository for us?
                pathToRepository = Runner.BrowseForRepository();
            }
            if (string.IsNullOrEmpty(pathToRepository) || !Directory.Exists(pathToRepository))
            {
                return; //give up
            }


            Properties.Settings.Default.PathToRepository = pathToRepository;
            Properties.Settings.Default.Save();
            new Runner().Run(pathToRepository, new Arguments(args));

            Properties.Settings.Default.Save();
            Application.Exit ();
        }

        private static void SetUpErrorHandling()
        {
            try
            {
//				Palaso.Reporting.ErrorReport.AddProperty("EmailAddress", "issues@wesay.org");
//				Palaso.Reporting.ErrorReport.AddStandardProperties();
//				Palaso.Reporting.ExceptionHandler.Init();

            /* until we decide to require palaso.dll, we can at least make use of it if it happens
             * to be there (as it is with WeSay)
             */
                Assembly asm = Assembly.LoadFrom("Palaso.dll");
                Type errorReportType = asm.GetType("Palaso.Reporting.ErrorReport");
                PropertyInfo emailAddress = errorReportType.GetProperty("EmailAddress");
                emailAddress.SetValue(null,"issues@wesay.org",null);
                errorReportType.GetMethod("AddStandardProperties").Invoke(null, null);
                asm.GetType("Palaso.Reporting.ExceptionHandler").GetMethod("Init").Invoke(null, null);
            }
            catch(Exception)
            {
                //ah well
            }
        }


        internal class Runner
        {
            public void Run(string pathToRepository, Arguments arguments)
            {

                BrowseForRepositoryEvent browseForRepositoryEvent = new BrowseForRepositoryEvent();
                browseForRepositoryEvent.Subscribe(BrowseForRepository);
                using (var bootStrapper = new BootStrapper(pathToRepository))
                {
                    Application.Run(bootStrapper.CreateShell(browseForRepositoryEvent, arguments));
                }
            }

            public  void BrowseForRepository(string dummy)
            {
                var s = BrowseForRepository();
                if (!string.IsNullOrEmpty(s) && Directory.Exists(s))
                {
                    //NB: if this was run from a Visual Studio debug session, these settings
                    //are going to be saved in a different place, so on
                    //restart, we won't really open the one we wanted.
                    //We'll instead open the last project that was opened when
                    //running outside of Visual Studio.
                    Properties.Settings.Default.PathToRepository = s;
                    Properties.Settings.Default.Save(); 
                    Application.Restart();
                }
            }

            public static string BrowseForRepository()
            {
                var dlg = new FolderBrowserDialog();
                dlg.Description = "Select a chorus-enabled project to open:";
                dlg.ShowNewFolderButton = false;
                if (DialogResult.OK != dlg.ShowDialog())
                    return null;
                return dlg.SelectedPath;
            }
        }
    }
}