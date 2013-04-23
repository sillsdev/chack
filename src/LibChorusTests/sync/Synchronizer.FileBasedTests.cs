﻿using System.IO;
using Chorus.sync;
using Chorus.VcsDrivers;
using Chorus.VcsDrivers.Mercurial;
using LibChorus.TestUtilities;
using NUnit.Framework;
using System.Linq;
using Palaso.Progress;

namespace LibChorus.Tests.sync
{
    /// <summary>
    /// Review: what's the unifying idea here?  
    /// </summary>
    [TestFixture]
	[Category("Sync")]
    public class Synchronizer_FileBasedTests
    {
        private ProjectFolderConfiguration _project;
        private IProgress _progress;
        private string _pathToTestRoot;
        private string _pathToProjectRoot;
        private Synchronizer _synchronizer;
        private string _pathToBackupFolder;
        private DirectoryRepositorySource _directorySource;

        [SetUp]
        public void Setup()
        {
            _progress = new ConsoleProgress();
            _pathToTestRoot = Path.Combine(Path.GetTempPath(), "ChorusTest");
            if (Directory.Exists(_pathToTestRoot))
                Directory.Delete(_pathToTestRoot, true);
            Directory.CreateDirectory(_pathToTestRoot);

            //nb: the ".2" here is significant; there was an issue where anything after a "." got stripped
            _pathToProjectRoot = Path.Combine(_pathToTestRoot, "foo project.2");
            Directory.CreateDirectory(_pathToProjectRoot);

            string pathToText = WriteTestFile("version one");

            RepositorySetup.MakeRepositoryForTest(_pathToProjectRoot, "bob",_progress);
            _project = new ProjectFolderConfiguration(_pathToProjectRoot);
            _project.IncludePatterns.Add(pathToText);
            _project.FolderPath = _pathToProjectRoot;


            _synchronizer = Synchronizer.FromProjectConfiguration(_project, _progress);
            _pathToBackupFolder = Path.Combine(_pathToTestRoot, "backup");
            Directory.CreateDirectory(_pathToBackupFolder);
            _directorySource = new DirectoryRepositorySource("SD Backup Card", Path.Combine(_pathToBackupFolder,RepositoryAddress.ProjectNameVariable), false);

        }

		[TearDown]
		public void Teardown()
		{
			Directory.Delete(_pathToTestRoot, true);
			// No. It goes away when _pathToTestRoot goes away.
			//Directory.Delete(_pathToProjectRoot, true);
		}

        private string WriteTestFile(string contents)
        {
            string pathToText = Path.Combine(_pathToProjectRoot, "foo.txt");
            File.WriteAllText(pathToText, contents);
            return pathToText;
        }


		[Test, Ignore("Doesn't really test a repository on another computer. Cf. Synchronizer.SendToOneOther that fails on the Update attempt on a shared network folder.")]
        public void SyncNow_BackupAlreadySetUp_GetsSync()
        {             
            SyncOptions options = new SyncOptions();
            _synchronizer.SyncNow(options);
            string projectDirOnBackup = Path.Combine(_pathToBackupFolder, "foo project.2");
            //_synchronizer.MakeClone(projectDirOnBackup, true);
            HgHighLevel.MakeCloneFromLocalToLocal(_synchronizer.Repository.PathToRepo, projectDirOnBackup, true, _progress);

            string contents = File.ReadAllText(Path.Combine(projectDirOnBackup, "foo.txt"));
            Assert.AreEqual("version one", contents);
            WriteTestFile("version two");

            options.RepositorySourcesToTry.Add(_directorySource);
            _synchronizer.SyncNow(options);
            contents = File.ReadAllText(Path.Combine(projectDirOnBackup, "foo.txt"));
            Assert.AreEqual("version two", contents);
        }

        [Test]
        public void SyncNow_FileMissing_GetsRemoved()
        {
            SyncOptions options = new SyncOptions();
            _synchronizer.SyncNow(options);

            string path = Path.Combine(_pathToProjectRoot, "foo.txt");
            Assert.IsTrue(File.Exists(path));
            _synchronizer.SyncNow(options);
            File.Delete(path);
            _synchronizer.SyncNow(options);

            Assert.IsFalse(File.Exists(path));
        }

        /// <summary>
        /// Here, we're testing the scenario where the user specifies a backup location, like an sd card at z:\
        /// </summary>
        [Test]
        public void SyncNow_NotSetupBefore_GetsClone()
        {
            SyncOptions options = new SyncOptions();
            options.RepositorySourcesToTry.Add(_directorySource);

           // WriteTestFile("version two");
            
            Assert.IsTrue(_synchronizer.SyncNow(options).Succeeded);
            string dir = Path.Combine(_pathToBackupFolder, "foo project.2");
            Assert.IsTrue(Directory.Exists(dir));
        }

        [Test]
        public void SyncNow_SetsUpHgrc()
        {
            using (var setup = new RepositorySetup("Dan"))
            {
               Assert.Contains("hgext.graphlog", setup.Repository.GetEnabledExtension().ToArray());
               Assert.Contains("convert", setup.Repository.GetEnabledExtension().ToArray());
            }
        }

    }
}
