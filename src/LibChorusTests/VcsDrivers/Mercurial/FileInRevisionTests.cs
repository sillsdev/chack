using System.IO;
using Chorus.VcsDrivers.Mercurial;
using NUnit.Framework;
using System.Linq;
using Palaso.IO;
using Palaso.Progress;
using Palaso.TestUtilities;

namespace LibChorus.Tests.VcsDrivers.Mercurial
{
    [TestFixture]
    public class FilesInRevisionTests
    {

        private ConsoleProgress _progress;

    	[SetUp]
        public void Setup()
        {
            _progress = new ConsoleProgress();
        }

    	/// <summary>
        /// What's special here is that, with mercurial, if the revision has no
        /// parent, we don't ask for a range
        /// e.g. --rev 34:35, nor --rev 35:35.  We have to ask for --rev 35
        /// </summary>
        [Test]
        public void GetFilesInRevision_OnlyOneRevisionInRepo_GivesAllFiles()
        {
			using (var testRoot = new TemporaryFolder("ChorusRetrieveTest"))
            using (var f = new TempFileFromFolder(testRoot))
                {
                    File.WriteAllText(f.Path, "one");

                    HgRepository.CreateRepositoryInExistingDir(testRoot.Path,_progress);
                    var repo = new HgRepository(testRoot.Path, _progress);

                    repo.AddAndCheckinFile(f.Path);
                    repo.Commit(true, "initial");
                    var revisions = repo.GetAllRevisions();
                    Assert.AreEqual(1, revisions.Count);
                    var files = repo.GetFilesInRevision(revisions[0]);
                    Assert.AreEqual(1, files.Count());
                    Assert.AreEqual(f.Path, files.First().FullPath);
                }
        }

        [Test]
        public void GetFilesInRevision_MultipleRevisionsInRepo_GivesCorrectFiles()
        {
			using (var testRoot = new TemporaryFolder("ChorusRetrieveTest"))
            {
                var temp = testRoot.Combine("filename with spaces");
                File.WriteAllText(temp, "one");
                using (var f = TempFile.TrackExisting(temp))
                {
                    HgRepository.CreateRepositoryInExistingDir(testRoot.Path,_progress);
                    var repo = new HgRepository(testRoot.Path, _progress);

                    repo.AddAndCheckinFile(f.Path);
                    repo.Commit(true, "initial");
                    File.WriteAllText(f.Path, "one two");
                    repo.Commit(true, "second");

                    var revisions = repo.GetAllRevisions();
                    Assert.AreEqual(2, revisions.Count);
                    var files = repo.GetFilesInRevision(revisions[0]);
                    Assert.AreEqual(1, files.Count());
                    Assert.AreEqual(f.Path, files.First().FullPath);
                }
            }
        }

        /// <summary>
        /// what this is really testing is if we calculated the correct revision
        /// range string when there was a merge so that the parent is not necessarily just
        /// the rev # minus 1
        /// </summary>
        [Test, Ignore("I don't know how to write this test, at the moment")]
        public void GetFilesInRevision_RevisionsParentNumberIsNotJustNumberMinus1_GivesCorrectFiles()
        {
            
        }
    }
}