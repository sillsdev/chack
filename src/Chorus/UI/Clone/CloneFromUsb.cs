﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Palaso.UsbDrive;
using Chorus.VcsDrivers.Mercurial;
using Palaso.IO;
using Palaso.Progress;

namespace Chorus.UI.Clone
{
    /// <summary>
    /// Use this class to make an initial clone from a USB drive or Internet repository.
	/// Note, most clients can instead use the GetCloneFromUsbDialog in Chorus.exe.
    /// </summary>
    public class CloneFromUsb
    {
        public CloneFromUsb()
        {
            DriveInfoRetriever = new RetrieveUsbDriveInfo();
        }

        /// <summary>
        /// Use this to insert an artificial drive info system for unit tests
        /// </summary>
        public IRetrieveUsbDriveInfo DriveInfoRetriever { get; set; }

        /// <summary>
        /// Use this to inject a custom filter, so that the only projects that can be chosen are ones
        /// you application is prepared to open.  The delegate is given the path to each mercurial project.
        /// </summary>
		public Func<string, bool> ProjectFilter = GetSharedProject.DefaultProjectFilter;


        public bool GetHaveOneOrMoreUsbDrives()
        {
            return DriveInfoRetriever.GetDrives().Count > 0;
        }

        ///<summary>
        /// Retrieves from all USB drives all the Mercurial Repositories
        ///</summary>
        ///<returns></returns>
        public IEnumerable<string> GetDirectoriesWithMecurialRepos()
        {
            foreach (var drive in DriveInfoRetriever.GetDrives())
            {
                string[] directories = new string[0];
                try
                { // this is all complicated because the yield can't be inside the try/catch
					directories = DirectoryUtilities.GetSafeDirectories(drive.RootDirectory.FullName);
                }
                catch (Exception error)
                {
                    MessageBox.Show(
                        string.Format("Error while looking at USB flash drive.  The drive root was {0}. The error was: {1}",
                                      drive.RootDirectory.FullName, error.Message), "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                foreach (var dir in directories)
                {
                    if (Directory.Exists(Path.Combine(dir, ".hg")) && ProjectFilter(dir))
                    {
                        yield return dir;
                    }
					// As of 12 December, 2011, JohnH and I decided to remove the search at the second level.
					// Seems that will work, but then the next attempt to sync, will not be able to find the second level repo.
					//else //we'll look just at the next level down
					//{
					//    string[] subdirs = new string[0];
					//    try
					//    {    // this is all complicated because the yield can't be inside the try/catch
					//        subdirs = DirectoryUtilities.GetSafeDirectories(dir);
					//    }
					//    catch (Exception /*error*/) // Mono: The unused variable 'error' causes a compiler crash under mono 2.4, 2.10 CP 2011-10
					//    {
					//        //turns out that WIndows Backup directories can trigger this, so I'm going to just skip it. Wish we had some unobtrusive log to write to.
					//        //ErrorReport.NotifyUserOfProblem(error,"Error while looking at usb drive.  The drive root was {0}, the directory was {1}.",  drive.RootDirectory.FullName, dir );
					//    }
					//    foreach (var subdir in subdirs)
					//    {
					//        if (Directory.Exists(Path.Combine(subdir, ".hg")) && ProjectFilter(subdir))
					//        {
					//            yield return subdir;
					//        }
					//    }
					//}
                }
            }
        }

		///<summary>
		/// Makes a Mercurial clone of a repository from sourcePath to parentDirectoryToPutCloneIn
		///</summary>
		///<param name="sourcePath">Existing Hg repo</param>
		///<param name="parentDirectoryToPutCloneIn">Target folder for new clone</param>
		///<param name="progress">Progress indicator object</param>
		///<returns>Directory that clone was actually placed in (allows for renaming to avoid duplicates)</returns>
		public string MakeClone(string sourcePath, string parentDirectoryToPutCloneIn, IProgress progress)
		{
			return HgHighLevel.MakeCloneFromLocalToLocal(sourcePath,
			                                             Path.Combine(parentDirectoryToPutCloneIn, Path.GetFileName(sourcePath)),
														 true,
														 progress);
		}
	}
}