﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Chorus.notes;
using Palaso.Progress;

namespace Chorus.UI.Notes.Browser
{
    public partial class NotesInProjectView : UserControl
    {
        public delegate NotesInProjectView Factory(IProgress progress);//autofac uses this
        private NotesInProjectViewModel _model;

        public NotesInProjectView(NotesInProjectViewModel model)
        {
            this.Font = SystemFonts.MessageBoxFont;
            model.ReloadMessages += new EventHandler(OnReloadMessages);
            _model = model;
            //       _model.ProgressDisplay = new NullProgress();
            InitializeComponent();
            _messageListView.SmallImageList = AnnotationClassFactoryUI.CreateImageListContainingAnnotationImages();
            showResolvedNotesMenuItem.Checked = _model.ShowClosedNotes;
	        showQuestionsMenuItem.Checked = !_model.HideQuestions;
	        showMergeNotifcationsMenuItem.Checked = !model.HideNotifications;
	        showMergeConflictsMenuItem.Checked = !model.HideCriticalConflicts;
            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Enabled = true;
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            if(Visible)
            {
                _model.CheckIfWeNeedToReload();
            }
        }


        void OnReloadMessages(object sender, EventArgs e)
        {
            ListMessage previousItem = null;
            int previousIndex = -1;
            if (_messageListView.SelectedIndices.Count > 0)
            {
                previousItem = _messageListView.SelectedItems[0].Tag as ListMessage;
                previousIndex = _messageListView.SelectedIndices[0];
            }

            Cursor.Current = Cursors.WaitCursor;
            _messageListView.SuspendLayout();
            List<ListViewItem> rows = new List<ListViewItem>();
            
            foreach (var item in _model.GetMessages())
            {
                rows.Add(item.GetListViewItem(_model.DisplaySettings));
			}
			_messageListView.Items.Clear(); // Don't even think of moving this before the loop, as the items are doubled for reasons unknown.
            _messageListView.Items.AddRange(rows.ToArray());
            _messageListView.ResumeLayout();
            Cursor.Current = Cursors.Default;

            //restore the previous selection
            if (previousItem !=null)
            {
                bool gotIt = false;
                foreach (ListViewItem listViewItem in _messageListView.Items)
                {
                    if (((ListMessage)(listViewItem.Tag)).ParentAnnotation.Guid == previousItem.ParentAnnotation.Guid)
                    {
                        listViewItem.Selected = true;
                        gotIt = true;
                        break;
                    }
                }
                if (_messageListView.Items.Count > 0 && !gotIt)
                {
                    // Likely we hid the item that was previously selected.
                    // Select something, preferably the item at the same position.
                    if (previousIndex < 0)
                        _messageListView.Items[0].Selected = true;
                    else if (_messageListView.Items.Count > previousIndex) // usual case, if we deleted one thing and not the last
                        _messageListView.Items[previousIndex].Selected = true;
                    else
                        _messageListView.Items[_messageListView.Items.Count - 1].Selected = true; // closest to original index
                }
            }
	        filterStateLabel.Text = _model.FilterStateMessage;
            //enhance...we could, if the message is not found, go looking for the owning annotation. But since
            //you can't currently delete a message, that wouldn't have any advantage yet.

            //this leads to hiding the annotationview when nothing is actually selected anymore (because of searching)
            OnSelectedIndexChanged(null, null);
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (_messageListView.SelectedItems.Count == 0)
            {
                _model.SelectedMessageChanged(null);
            }
            else
            {
                _model.SelectedMessageChanged(_messageListView.SelectedItems[0].Tag as ListMessage);
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            //OnReloadMessages(null,null);
            _model.CheckIfWeNeedToReload();
        }

        private void searchBox1_SearchTextChanged(object sender, EventArgs e)
        {
            _model.SearchTextChanged(sender as string);
        }

        private void NotesInProjectView_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
                _model.CheckIfWeNeedToReload();
            else
            {
                //Enhance: this is never called (e.g. we get a call when we become visible, but not invisible)
                _model.GoodTimeToSave();
            }
        }

        private void _filterCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void showClosedNotesMenuItem_Click(object sender, EventArgs e)
        {
            _model.ShowClosedNotes = ((ToolStripMenuItem)sender).Checked;
        }

		private void showQuestionsMenuItem_Click(object sender, EventArgs e)
		{
			_model.HideQuestions = !((ToolStripMenuItem)sender).Checked;
		}

		private void showMergeNotificationsMenuItem_Click(object sender, EventArgs e)
		{
			_model.HideNotifications = !((ToolStripMenuItem)sender).Checked;
		}

		private void showMergeConflictsMenuItem_Click(object sender, EventArgs e)
		{
			_model.HideCriticalConflicts = !((ToolStripMenuItem)sender).Checked;
		}
    }
}