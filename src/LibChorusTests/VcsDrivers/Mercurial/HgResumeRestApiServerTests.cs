﻿using Chorus.VcsDrivers.Mercurial;
using NUnit.Framework;

namespace LibChorus.Tests.VcsDrivers.Mercurial
{
    [TestFixture]
    class HgResumeRestApiServerTests
    {
        [Test]
        public void Constructor_languageDepotUrl_IdentityAndProjectIdSetCorrectly()
        {
            var api = new HgResumeRestApiServer("http://hg-private.languagedepot.org/kyu-dictionary");
            Assert.That(api.Host, Is.EqualTo("hg-private.languagedepot.org"));
            Assert.That(api.ProjectId, Is.EqualTo("kyu-dictionary"));
        }

        [Test]
        public void Constructor_languageForgeUrl_IdentityAndProjectIdSetCorrectly()
        {
            var api = new HgResumeRestApiServer("http://hg.languageforge.com/projects/kyu-dictionary");
            Assert.That(api.Host, Is.EqualTo("hg.languageforge.com"));
            Assert.That(api.ProjectId, Is.EqualTo("kyu-dictionary"));
        }

        [Test]
        //colon is probably bogus for a username, but tests escaping well enough
        public void Username_IsEscapedInUri_IsUnescaped()
        {
            var api = new HgResumeRestApiServer("http://user%3aname:password@resumable.languageforge.com/projects/kyu-dictionary");
            Assert.That(api.UserName, Is.EqualTo("user:name"));
        }

        [Test]
        public void Password_IsEscapedInUri_IsUnescaped()
        {
            var api = new HgResumeRestApiServer("http://username:pass%3aword@resumable.languageforge.com/projects/kyu-dictionary");
            Assert.That(api.Password, Is.EqualTo("pass:word"));
        }
    }
}
