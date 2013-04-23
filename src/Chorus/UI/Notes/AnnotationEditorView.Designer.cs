﻿namespace Chorus.UI.Notes
{
    partial class AnnotationEditorView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._resolvedCheckBox = new System.Windows.Forms.CheckBox();
            this._addButton = new System.Windows.Forms.Button();
            this._newMessage = new System.Windows.Forms.TextBox();
            this._annotationLogo = new System.Windows.Forms.PictureBox();
            this._existingMessagesDisplay = new Gecko.GeckoWebBrowser();
            this._closeButton = new System.Windows.Forms.Button();
            this._annotationLabel = new System.Windows.Forms.LinkLabel();
            this._addNewMessageLabel = new Chorus.UI.BetterLabel();
            ((System.ComponentModel.ISupportInitialize)(this._annotationLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // _resolvedCheckBox
            // 
            this._resolvedCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._resolvedCheckBox.AutoSize = true;
            this._resolvedCheckBox.Image = global::Chorus.Properties.Resources.check12x12;
            this._resolvedCheckBox.Location = new System.Drawing.Point(4, 387);
            this._resolvedCheckBox.Name = "_resolvedCheckBox";
            this._resolvedCheckBox.Size = new System.Drawing.Size(83, 17);
            this._resolvedCheckBox.TabIndex = 2;
            this._resolvedCheckBox.Text = "&Resolved";
            this._resolvedCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this._resolvedCheckBox.UseVisualStyleBackColor = true;
            this._resolvedCheckBox.CheckedChanged += new System.EventHandler(this.OnResolvedCheckBox_CheckedChanged);
            // 
            // _addButton
            // 
            this._addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._addButton.Location = new System.Drawing.Point(255, 325);
            this._addButton.Name = "_addButton";
            this._addButton.Size = new System.Drawing.Size(63, 45);
            this._addButton.TabIndex = 1;
            this._addButton.Text = "&Add";
            this._addButton.UseVisualStyleBackColor = true;
            this._addButton.Click += new System.EventHandler(this._addButton_Click);
            // 
            // _newMessage
            // 
            this._newMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._newMessage.Location = new System.Drawing.Point(4, 325);
            this._newMessage.Multiline = true;
            this._newMessage.Name = "_newMessage";
            this._newMessage.Size = new System.Drawing.Size(248, 45);
            this._newMessage.TabIndex = 0;
            this._newMessage.TextChanged += new System.EventHandler(this._newMessage_TextChanged);
            this._newMessage.Enter += new System.EventHandler(this._newMessage_Enter);
            // 
            // _annotationLogo
            // 
            this._annotationLogo.Image = global::Chorus.Properties.Resources.NewNote16x16;
            this._annotationLogo.Location = new System.Drawing.Point(5, 8);
            this._annotationLogo.Name = "_annotationLogo";
            this._annotationLogo.Size = new System.Drawing.Size(32, 32);
            this._annotationLogo.TabIndex = 1;
            this._annotationLogo.TabStop = false;
            this._annotationLogo.Paint += new System.Windows.Forms.PaintEventHandler(this._annotationLogo_Paint);
            this._annotationLogo.DoubleClick += new System.EventHandler(this._annotationLogo_DoubleClick);
            // 
            // _existingMessagesDisplay
            // 
			// GECKOFX: is this needed?
            //this._existingMessagesDisplay.AllowWebBrowserDrop = false;
            this._existingMessagesDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._existingMessagesDisplay.Location = new System.Drawing.Point(5, 47);
            this._existingMessagesDisplay.MinimumSize = new System.Drawing.Size(20, 20);
            this._existingMessagesDisplay.Name = "_existingMessagesDisplay";
            this._existingMessagesDisplay.Size = new System.Drawing.Size(313, 250);
            this._existingMessagesDisplay.TabIndex = 9;
			// GECKOFX: is this needed?
            //this._existingMessagesDisplay.WebBrowserShortcutsEnabled = false;
			// GECKOFX: does this do what is needed?
            //this._existingMessagesDisplay.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this._existingMessagesDisplay_DocumentCompleted);
			this._existingMessagesDisplay.DocumentCompleted += new System.EventHandler(this._existingMessagesDisplay_DocumentCompleted);
			// GECKOFX: does this do what is needed?
            //this._existingMessagesDisplay.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this._existingMessagesDisplay_Navigating);
			this._existingMessagesDisplay.Navigating += new System.EventHandler<Gecko.GeckoNavigatingEventArgs>(this._existingMessagesDisplay_Navigating);
            // 
            // _closeButton
            // 
            this._closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._closeButton.Location = new System.Drawing.Point(189, 376);
            this._closeButton.Name = "_closeButton";
            this._closeButton.Size = new System.Drawing.Size(129, 36);
            this._closeButton.TabIndex = 1;
            this._closeButton.Text = "Add && &OK";
            this._closeButton.UseVisualStyleBackColor = true;
            this._closeButton.Click += new System.EventHandler(this._closeButton_Click);
            // 
            // _annotationLabel
            // 
            this._annotationLabel.AutoSize = true;
            this._annotationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._annotationLabel.LinkColor = System.Drawing.Color.Black;
            this._annotationLabel.Location = new System.Drawing.Point(43, 8);
            this._annotationLabel.Name = "_annotationLabel";
            this._annotationLabel.Size = new System.Drawing.Size(129, 24);
            this._annotationLabel.TabIndex = 10;
            this._annotationLabel.TabStop = true;
            this._annotationLabel.Text = "Target of Note";
            this._annotationLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._annotationLabel_LinkClicked);
            // 
            // _addNewMessageLabel
            // 
            this._addNewMessageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._addNewMessageLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._addNewMessageLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._addNewMessageLabel.Location = new System.Drawing.Point(4, 298);
            this._addNewMessageLabel.Multiline = true;
            this._addNewMessageLabel.Name = "_addNewMessageLabel";
            this._addNewMessageLabel.ReadOnly = true;
            this._addNewMessageLabel.Size = new System.Drawing.Size(293, 20);
            this._addNewMessageLabel.TabIndex = 5;
            this._addNewMessageLabel.TabStop = false;
            this._addNewMessageLabel.Text = "Add new message:";
            // 
            // AnnotationEditorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._annotationLabel);
            this.Controls.Add(this._existingMessagesDisplay);
            this.Controls.Add(this._newMessage);
            this.Controls.Add(this._closeButton);
            this.Controls.Add(this._addButton);
            this.Controls.Add(this._addNewMessageLabel);
            this.Controls.Add(this._resolvedCheckBox);
            this.Controls.Add(this._annotationLogo);
            this.Name = "AnnotationEditorView";
            this.Size = new System.Drawing.Size(321, 415);
            this.Load += new System.EventHandler(this.AnnotationView_Load);
            ((System.ComponentModel.ISupportInitialize)(this._annotationLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox _annotationLogo;
        private System.Windows.Forms.CheckBox _resolvedCheckBox;
        private BetterLabel _addNewMessageLabel;
        private System.Windows.Forms.Button _addButton;
        private System.Windows.Forms.TextBox _newMessage;
        private Gecko.GeckoWebBrowser _existingMessagesDisplay;
        private System.Windows.Forms.Button _closeButton;
        private System.Windows.Forms.LinkLabel _annotationLabel;
    }
}