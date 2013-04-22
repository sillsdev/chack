﻿using System.IO;
using System.Text;
using Chorus.merge;
using LibChorus.Tests.merge;
using Chorus.Utilities;
using NUnit.Framework;
using Palaso.TestUtilities;

namespace ChorusMerge.Tests
{
    /// <summary>
    /// The assembly under test doesn't actually do much, so the only tests
    /// needed here are to cover what little it does... most of the action is in libchorus
    /// </summary>
    [TestFixture]
    public class ChorusMergeTests
    {
		// The file is held onto by R#'s test taskrunner for some reason.
		//[TestFixtureTearDown]
		//public void FixtureTearDown()
		//{
		//    var file = Path.Combine(Path.GetTempPath(), "LiftMerger.FindEntryById");
		//    if (File.Exists(file))
		//        File.Delete(file);
		//}

        [Test]
        public void Main_NoConflictFileB4_ConflictsEncountered_HaveConflictFileAfter()
        {
            using (var group = new GroupOfConflictingLiftFiles())
            {
                Assert.AreEqual(0, DoMerge(group));
                Assert.IsTrue(File.Exists(group.BobTextConflictsPath));
                var text = File.ReadAllText(group.BobTextConflictsPath);
                Assert.AreNotEqual(string.Empty, text);
            }
        }

        [Test]
        public void Main_UnhandledMergeFailure_Returns1()
        {
			using (var group = new GroupOfConflictingLiftFiles())
			using (new FailureSimulator("LiftMerger.FindEntryById"))
			{
				Assert.AreEqual(1, DoMerge(group));
			}
        }

        private int DoMerge(GroupOfConflictingLiftFiles group)
        {
            MergeSituation.PushRevisionsToEnvironmentVariables("bob", "-123", "sally", "-456");
            MergeOrder.PushToEnvironmentVariables(group.Folder.Path);
            return Program.Main(new[] {group.BobFile.Path, group.AncestorFile.Path, group.SallyFile.Path});
        }

        [Test, Ignore("re-enable, after ini file fix is in.")]
        public void Main_Utf8FilePaths_FileNamesOk()
        {
            using (var e = new TemporaryFolder("ChorusMergeTest"))
            using (var p = new TemporaryFolder(e, "ไก่ projéct"))
            {
                var filePath1 = Path.Combine(p.Path, "aaa.chorusTest");
                File.WriteAllText(filePath1, @"aaa");
                var filePath2 = Path.Combine(e.Path, "aaa.chorusTest");
                File.WriteAllText(filePath2, @"aaa");
                var filePath3 = Path.Combine(e.Path, "aaa.chorusTest");
                File.WriteAllText(filePath3, @"aaa");

                var encoding = Encoding.GetEncoding(1252);
                string filePath1Cp1252 = encoding.GetString(Encoding.UTF8.GetBytes(filePath1));
                string filePath2Cp1252 = encoding.GetString(Encoding.UTF8.GetBytes(filePath2));
                string filePath3Cp1252 = encoding.GetString(Encoding.UTF8.GetBytes(filePath3));

                MergeSituation.PushRevisionsToEnvironmentVariables("bob", "-123", "sally", "-456");
                MergeOrder.PushToEnvironmentVariables(p.Path);
                var result = Program.Main(new[] { filePath1Cp1252, filePath2Cp1252, filePath3Cp1252 });
                
                Assert.That(result, Is.EqualTo(0));
            }
            
        }


    }
}
