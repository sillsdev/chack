﻿using System;
using System.Collections.Generic;
using System.IO;
using Chorus.VcsDrivers.Mercurial;
using Nini.Ini;
using Palaso.IO;

namespace LibChorus.Tests.VcsDrivers.Mercurial
{
	public class MercurialIniForTests : IDisposable
	{
#if !MONO
		private readonly string _mercurialIniFilePath;
		private readonly string _mercurialIniBackupFilePath;
#endif

		public MercurialIniForTests()
		{
#if !MONO
			_mercurialIniFilePath = Path.Combine(Chorus.MercurialLocation.PathToMercurialFolder, "mercurial.ini");
			_mercurialIniBackupFilePath = _mercurialIniFilePath + ".bak";
			File.Copy(_mercurialIniFilePath, _mercurialIniBackupFilePath, true);
			UpdateExtensions();
#endif
		}

		public void Dispose()
		{
#if !MONO
			File.Copy(_mercurialIniBackupFilePath, _mercurialIniFilePath, true);
			File.Delete(_mercurialIniBackupFilePath);
#endif
		}

		private static void UpdateExtensions()
		{
			var extensions = new Dictionary<string, string>();
#if !MONO
			extensions.Add("hgext.win32text", ""); //for converting line endings on windows machines
#endif
			extensions.Add("hgext.graphlog", ""); //for more easily readable diagnostic logs
			extensions.Add("convert", ""); //for catastrophic repair in case of repo corruption
#if !MONO
			string fixUtfFolder = FileLocator.GetDirectoryDistributedWithApplication(false, "MercurialExtensions", "fixutf8");
			if (!string.IsNullOrEmpty(fixUtfFolder))
				extensions.Add("fixutf8", Path.Combine(fixUtfFolder, "fixutf8.py"));
#endif
			var doc = HgRepository.GetMercurialConfigInMercurialFolder();
			SetExtensions(doc, extensions);
			doc.SaveAndThrowIfCannot();
		}

		private static void SetExtensions(IniDocument doc, IEnumerable<KeyValuePair<string, string>> extensionDeclarations)
		{
			var section = doc.Sections.GetOrCreate("extensions");
			foreach (var pair in extensionDeclarations)
			{
				section.Set(pair.Key, pair.Value);
			}
		}

	}

	public class MercurialIniHider : IDisposable
	{
		private readonly string _mercurialIniFilePath;
		private readonly string _mercurialIniBackupFilePath;

		public MercurialIniHider()
		{
#if MONO
			var home = Environment.GetEnvironmentVariable("HOME");
            if (home == null)
            {
                throw new ApplicationException("The HOME environment variable is not set.");
            }
            _mercurialIniFilePath = Path.Combine(home, ".hgrc");
#else
			_mercurialIniFilePath = Path.Combine(Chorus.MercurialLocation.PathToMercurialFolder, "mercurial.ini");
#endif
			_mercurialIniBackupFilePath = _mercurialIniFilePath + ".bak";
			File.Copy(_mercurialIniFilePath, _mercurialIniBackupFilePath, true);
		}

		public void Dispose()
		{
			File.Copy(_mercurialIniBackupFilePath, _mercurialIniFilePath, true);
			File.Delete(_mercurialIniBackupFilePath);
		}
	}
}