﻿using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Chorus.sync;
using Chorus.Utilities;
using Chorus.VcsDrivers.Mercurial;
using Palaso.Progress;

namespace Chorus.UI.Review.RevisionsInRepository
{
    public partial class RevisionsInRepositoryView : UserControl
    {
        private RevisionInRepositoryModel _model;
        private ProjectFolderConfiguration _project;
        private String _userName="anonymous";

        public RevisionsInRepositoryView(RevisionInRepositoryModel model)
        {
            this.Font = SystemFonts.MessageBoxFont;
            _model = model;
            _model.ProgressDisplay = new NullProgress();
            InitializeComponent();
            UpdateDisplay();
            _showAdvanced.Visible=false;
        }

        public ProjectFolderConfiguration ProjectFolderConfig
        {
            get { return _project; }
            set { _project = value; }
        }

        /// <summary>
        /// most client apps won't have anything to put in here, that's ok
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }


        private void UpdateDisplay()
        {
            _historyGrid.Columns[1].Visible = false; // the UI is there, but not the functionality _model.DoShowRevisionChoiceControls;
            _historyGrid.Columns[2].Visible = false;//  _model.DoShowRevisionChoiceControls;
        }
		
        private void RefreshRevisions()
        {
            _historyGrid.Rows.Clear();
        	_model.BeginGettingRevisions();
        	_rowAddingTimer.Enabled = true;
        }

		/// <summary>
		/// called when the firt batch of revisions are ready
		/// </summary>
    	private void ChooseInitialRevisions()
    	{
    		_childRevisionIndex = _parentRevisionIndex = 0;
    		if (_historyGrid.Rows.Count > 1)
    		{
    			// no: this can be very slow, so wait until they select one
    			//_historyList.Items[0].Selected = true;
    			_parentRevisionIndex = 1;
    		}
            
    		_historyGrid.Rows[_childRevisionIndex].Cells[ColumnChildRevision.Name].Value = true;
    		_historyGrid.Rows[_parentRevisionIndex].Cells[ColumnParentRevision.Name].Value = true;
    	}

    	/// <summary>
		/// Revisions take time to gether up, so the UI gets them in chunks,
		/// while we gather up more in the background
		/// </summary>
		private void OnRowAddingTimer_Tick(object sender, EventArgs e)
		{
    		bool gridWasEmpty =_historyGrid.Rows.Count < 2;
			lock(_model.DiscoveredRevisionsQueue)
			{
				_historyGrid.SuspendLayout();
				const int maxNumberToAddAtOnce = 20;
				for (int i = 0; i < maxNumberToAddAtOnce && _model.DiscoveredRevisionsQueue.Count > 0; i++)
				{
						AddRow(_model.DiscoveredRevisionsQueue.Dequeue());
				}
				_historyGrid.ResumeLayout();
			}
			if(gridWasEmpty && _historyGrid.Rows.Count >= 2)
			{
				ChooseInitialRevisions();
			}
		}

    	private void AddRow(Revision rev)
    	{
    		var dateString = rev.DateString;
    		DateTime when;

    		//TODO: this is all a guess and a mess
    		//I haven't figured out how/why hg uses this strange date format,
    		//nor if it is going to do it on all machines, or will someday 
    		//change.
    		if (DateTime.TryParseExact(dateString, "ddd MMM dd HH':'mm':'ss yyyy zzz",
    		                           null, DateTimeStyles.AssumeUniversal, out when))
    		{
    			when = when.ToLocalTime();
    			dateString = when.ToShortDateString()+" "+when.ToShortTimeString();
    		}

    		object image;
            if (rev.Summary.ToLower().Contains("conflict"))
                image = HistoryRowIcons.Warning;
            else if (rev.Parents.Count > 1)
                image = HistoryRowIcons.Merge;
            else
            {
                var colonLocation = rev.Summary.IndexOf(':');
                string appName = rev.Summary;
                if (colonLocation > 0)
                {
                    appName = appName.Substring(0, colonLocation);
                }
                var bracketLocation = appName.IndexOf(']');
                if (bracketLocation > 0)
                {
                    appName = appName.Substring(0, bracketLocation);
                }
                appName = appName.Trim(new char[] { '[', '+' }); // there was a bug in chorus that introduced the +
                //temp hack... the app has now been fixed to not include this
                appName = appName.Replace("0.5", "");

                switch (appName.Trim())
                {
                    case "WeSay":
                        image = HistoryRowIcons.WeSay;
                        break;
                    case "WeSay Configuration Tool":
                        image = HistoryRowIcons.WeSayConfiguration;
                        break;
                    case "FieldWorks":
                        image = HistoryRowIcons.FieldWorks;
                        break;
                    case "Bloom":
                        image = HistoryRowIcons.Bloom16x16;
                        break;
                    default:
                        image = HistoryRowIcons.GenericCheckin;
                        break;
                }

            }
    		int nIndex = _historyGrid.Rows.Add(new [] { image, false, false, dateString, rev.UserId, GetDescriptionForListView(rev) });
    		var row = _historyGrid.Rows[nIndex];
    		row.Tag = rev;
    		row.Cells[0].ToolTipText = rev.Number.LocalRevisionNumber + ": " + rev.Number.Hash;
    	}

    	private string GetDescriptionForListView(Revision rev)
        {
            var s = rev.Summary.Substring(rev.Summary.IndexOf(']')+1).Trim();
            if(s=="auto")
                return string.Empty;
            return s;
        }

        private void StartRefreshTimer(object sender, EventArgs e)
        {
            timer1.Enabled = this.Visible;
//            if (Visible && _historyList.Items.Count == 0)
//            {
//                Refresh(null, null);
//            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                if (_model.GetNeedRefresh())
                {
                    RefreshRevisions();
                    //enhance, check after app itself is reactivated, or work in background.  It  is noticeably slow.
                    timer1.Enabled = false;//don't check again so long as we're visible
                }
            }
        }
       private void OnRefresh(object sender, EventArgs e)
        {
           RefreshRevisions();
        }

       private int _childRevisionIndex;
       private int _parentRevisionIndex;
       private void OnHistoryGrid_CellClick(object sender, DataGridViewCellEventArgs e)
       {
           if(!_model.DoShowRevisionChoiceControls)
           {
               if(_historyGrid.SelectedRows.Count==1)
               {
                   Revision revision = _historyGrid.SelectedRows[0].Tag as Revision;
                   _model.SelectedRevisionChanged(revision);
               }
               return;
           }
 /*          // make sure we have something reasonable
           if (((e.ColumnIndex < ColumnParentRevision.Index) || (e.ColumnIndex > ColumnChildRevision.Index))
               || (e.RowIndex < 0) || (e.RowIndex > _historyGrid.Rows.Count))
               return;
           
           if (e.ColumnIndex == ColumnParentRevision.Index)
           {
               _historyGrid.Rows[_parentRevisionIndex].Cells[ColumnParentRevision.Index].Value = false;
               _parentRevisionIndex = e.RowIndex;
           }
           else if (e.ColumnIndex == ColumnChildRevision.Index)
           {
               _historyGrid.Rows[_childRevisionIndex].Cells[ColumnChildRevision.Index].Value = false;
               _childRevisionIndex = e.RowIndex;
           }

  * 
  * REVIW: I (JH) can't see how this ever worked.  It appears to only send a single revision, the partent, and ignores the childRevisionIndex.
           Revision rev = _historyGrid.Rows[_parentRevisionIndex].Tag as Revision;
           _model.SelectedRevisionChanged(rev);
  */
       }

       private void OnShowAdvanced_CheckedChanged(object sender, EventArgs e)
       {
           _model.DoShowRevisionChoiceControls = _showAdvanced.Checked;
           UpdateDisplay();
       }


    }
}