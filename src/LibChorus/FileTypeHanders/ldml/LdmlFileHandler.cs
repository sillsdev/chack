﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Chorus.FileTypeHanders.xml;
using Chorus.merge;
using Chorus.merge.xml.generic;
using Chorus.VcsDrivers.Mercurial;
using Palaso.IO;
using Palaso.Progress;
using Palaso.Xml;

namespace Chorus.FileTypeHanders.ldml
{
	///<summary>
	/// Implementation of the IChorusFileTypeHandler interface to handle LDML files
	///</summary>
	public class LdmlFileHandler : IChorusFileTypeHandler
	{
		internal LdmlFileHandler()
		{}

		private const string Extension = "ldml";

		#region Implementation of IChorusFileTypeHandler

		public bool CanDiffFile(string pathToFile)
		{
			return CanValidateFile(pathToFile);
		}

		public bool CanMergeFile(string pathToFile)
		{
			return CanValidateFile(pathToFile);
		}

		public bool CanPresentFile(string pathToFile)
		{
			return CanValidateFile(pathToFile);
		}

		public bool CanValidateFile(string pathToFile)
		{
			return FileUtils.CheckValidPathname(pathToFile, Extension);
		}

		/// <summary>
		/// Do a 3-file merge, placing the result over the "ours" file and returning an error status
		/// </summary>
		/// <remarks>Implementations can exit with an exception, which the caller will catch and deal with.
		/// The must not have any UI, no interaction with the user.</remarks>
		public void Do3WayMerge(MergeOrder mergeOrder)
		{
			if (mergeOrder == null)
				throw new ArgumentNullException("mergeOrder");
			PreMergeFile(mergeOrder);

			var merger = new XmlMerger(mergeOrder.MergeSituation);
			SetupElementStrategies(merger);

			merger.EventListener = mergeOrder.EventListener;
			var result = merger.MergeFiles(mergeOrder.pathToOurs, mergeOrder.pathToTheirs, mergeOrder.pathToCommonAncestor);
			using (var writer = XmlWriter.Create(mergeOrder.pathToOurs, CanonicalXmlSettings.CreateXmlWriterSettings()))
			{
				var nameSpaceManager = new XmlNamespaceManager(new NameTable());
				nameSpaceManager.AddNamespace("palaso", "urn://palaso.org/ldmlExtensions/v1");
				nameSpaceManager.AddNamespace("fw", "urn://fieldworks.sil.org/ldmlExtensions/v1");

				var readerSettings = CanonicalXmlSettings.CreateXmlReaderSettings(ConformanceLevel.Auto);
				readerSettings.NameTable = nameSpaceManager.NameTable;
				readerSettings.XmlResolver = null;
				readerSettings.ProhibitDtd = false;
				using (var nodeReader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(result.MergedNode.OuterXml)), readerSettings))
				{
					writer.WriteNode(nodeReader, false);
				}
			}
		}

		public IEnumerable<IChangeReport> Find2WayDifferences(FileInRevision parent, FileInRevision child, HgRepository repository)
		{
			return new IChangeReport[] { new DefaultChangeReport(parent, child, "Edited") };
		}

		public IChangePresenter GetChangePresenter(IChangeReport report, HgRepository repository)
		{
			// TODO: Add better presenter.
			return report is ErrorDeterminingChangeReport
			       	? (IChangePresenter) report
			       	: new DefaultChangePresenter(report, repository);
		}

		/// <summary>
		/// return null if valid, otherwise nice verbose description of what went wrong
		/// </summary>
		public string ValidateFile(string pathToFile, IProgress progress)
		{
			try
			{
				using (var reader = XmlReader.Create(pathToFile, CanonicalXmlSettings.CreateXmlReaderSettings()))
				{
					reader.MoveToContent();
					if (reader.LocalName == "ldml")
					{
						// It would be nice, if it could really validate it.
						while (reader.Read())
						{
						}
					}
					else
					{
						throw new InvalidOperationException("Not an LDML file.");
					}
				}
			}
			catch (Exception error)
			{
				return error.Message;
			}
			return null;
		}

		/// <summary>
		/// This is like a diff, but for when the file is first checked in.  So, for example, a dictionary
		/// handler might list any the words that were already in the dictionary when it was first checked in.
		/// </summary>
		public IEnumerable<IChangeReport> DescribeInitialContents(FileInRevision fileInRevision, TempFile file)
		{
			return new IChangeReport[] { new DefaultChangeReport(fileInRevision, "Added") };
		}

		/// <summary>
		/// Get a list or one, or more, extensions this file type handler can process
		/// </summary>
		/// <returns>A collection of extensions (without leading period (.)) that can be processed.</returns>
		public IEnumerable<string> GetExtensionsOfKnownTextFileTypes()
		{
			yield return Extension;
		}

		/// <summary>
		/// Return the maximum file size that can be added to the repository.
		/// </summary>
		/// <remarks>
		/// Return UInt32.MaxValue for no limit.
		/// </remarks>
		public uint MaximumFileSize
		{
			get { return UInt32.MaxValue; }
		}

		#endregion

		private static void SetupElementStrategies(XmlMerger merger)
		{
			merger.MergeStrategies.ElementToMergeStrategyKeyMapper = new LdmlElementToMergeStrategyKeyMapper();

			merger.MergeStrategies.SetStrategy("ldml", ElementStrategy.CreateSingletonElement());
			merger.MergeStrategies.SetStrategy("identity", ElementStrategy.CreateSingletonElement());
			// Child elements of "identity".
			merger.MergeStrategies.SetStrategy("version", ElementStrategy.CreateSingletonElement());
			merger.MergeStrategies.SetStrategy("generation", ElementStrategy.CreateSingletonElement());
			merger.MergeStrategies.SetStrategy("language", ElementStrategy.CreateSingletonElement());
			merger.MergeStrategies.SetStrategy("script", ElementStrategy.CreateSingletonElement());
			merger.MergeStrategies.SetStrategy("territory", ElementStrategy.CreateSingletonElement());
			merger.MergeStrategies.SetStrategy("variant", ElementStrategy.CreateSingletonElement());
			merger.MergeStrategies.SetStrategy("collations", ElementStrategy.CreateSingletonElement());
			// Child element of collations
			var strategy = ElementStrategy.CreateSingletonElement();
			strategy.IsAtomic = true; // I (RBR) think it would be suicidal to try and merge this element.
			merger.MergeStrategies.SetStrategy("collation", strategy);
			// Special "xmlns:palaso"
			strategy = new ElementStrategy(false)
			{
				IsAtomic = true, // May not be needed...
				MergePartnerFinder = new FindByMatchingAttributeNames(new HashSet<string> { "xmlns:palaso" })
			};
			merger.MergeStrategies.SetStrategy("special_xmlns:palaso", strategy);
			// Special "xmlns:fw"
			strategy = new ElementStrategy(false)
			{
				IsAtomic = true, // Really is needed. At least it is for some child elements.
				MergePartnerFinder = new FindByMatchingAttributeNames(new HashSet<string> { "xmlns:fw" })
			};
			merger.MergeStrategies.SetStrategy("special_xmlns:fw", strategy);
		}

		/// <summary>
		/// handles that date business, so it doesn't overwhelm the poor user with conflict reports
		/// </summary>
		/// <param name="mergeOrder"></param>
		private static void PreMergeFile(MergeOrder mergeOrder)
		{
			var ourDoc = File.Exists(mergeOrder.pathToOurs) ? XDocument.Load(mergeOrder.pathToOurs) : null;
			var theirDoc = File.Exists(mergeOrder.pathToTheirs) ? XDocument.Load(mergeOrder.pathToTheirs) : null;

			if (ourDoc == null || theirDoc == null)
				return;

			// Pre-merge <generation> date attr to newest.
			string ourRawGenDate;
			var ourGenDate = GetGenDate(ourDoc, out ourRawGenDate);
			string theirRawGenDate;
			var theirGenDate = GetGenDate(theirDoc, out theirRawGenDate);
			if (ourGenDate == theirGenDate)
				return;
			if (ourGenDate < theirGenDate)
			{
				var ourData = File.ReadAllText(mergeOrder.pathToOurs).Replace(ourRawGenDate, theirRawGenDate);
				File.WriteAllText(mergeOrder.pathToOurs, ourData);
				return;
			}
			var theirData = File.ReadAllText(mergeOrder.pathToTheirs).Replace(theirRawGenDate, ourRawGenDate);
			File.WriteAllText(mergeOrder.pathToTheirs, theirData);
		}

		private static DateTime GetGenDate(XDocument doc, out string rawGenDate)
		{
			rawGenDate = doc.Root.Element("identity").Element("generation").Attribute("date").Value;
			var splitData = GetDateTimeInts(rawGenDate);
			return new DateTime(splitData[0], splitData[1], splitData[2], splitData[3], splitData[4], splitData[5]);
		}

		private static int[] GetDateTimeInts(string ldmlDate)
		{
			// date="2012-06-08T09:36:30"
			var splitData = ldmlDate.Split(new[] {"-", "T", ":"}, StringSplitOptions.RemoveEmptyEntries);
			return new[] { Int32.Parse(splitData[0]), Int32.Parse(splitData[1]), Int32.Parse(splitData[2]), Int32.Parse(splitData[3]), Int32.Parse(splitData[4]), Int32.Parse(splitData[5]) };
		}
	}
}
