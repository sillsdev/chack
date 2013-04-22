using Chorus.UI.Clone;
using NUnit.Framework;
using Palaso.TestUtilities;

namespace Chorus.Tests.UI.Clone
{
	[TestFixture]
	public class TargetFolderControlTests
	{
		///--------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the attempt to display to the user that they entered bad path info does not crash while trying
		/// to use that bad info to construct the error message.
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void UpdateDisplay_BadModelDoesNotThrow()
		{
			using (var testFolder = new TemporaryFolder("clonetest"))
			{
				var model = new GetCloneFromInternetModel(testFolder.Path);
				model.AccountName = "account";
				model.Password = "password";
				model.ProjectId = "id";
				model.LocalFolderName = "Some<Folder";
				var ctrl = new TargetFolderControl(model);
				Assert.DoesNotThrow(() => { ctrl._localFolderName.Text = "Some<Folders"; });
			}
		}
	}
}
