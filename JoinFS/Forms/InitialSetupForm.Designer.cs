namespace JoinFS
{
    partial class InitialSetupForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitialSetupForm));
            this.Text_Nickname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Button_OK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.Label_SimBriefUsername = new System.Windows.Forms.Label();
            this.Text_SimBriefUsername = new System.Windows.Forms.TextBox();
            this.Label_FolderPrompt = new System.Windows.Forms.Label();
            this.Text_Folder = new System.Windows.Forms.TextBox();
            this.Button_Browse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // Text_Nickname
            //
            resources.ApplyResources(this.Text_Nickname, "Text_Nickname");
            this.Text_Nickname.Name = "Text_Nickname";
            //
            // label1
            //
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            //
            // Button_OK
            //
            resources.ApplyResources(this.Button_OK, "Button_OK");
            this.Button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Button_OK.Name = "Button_OK";
            this.Button_OK.UseVisualStyleBackColor = true;
            this.Button_OK.Click += new System.EventHandler(this.Button_OK_Click);
            //
            // label2
            //
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            //
            // Label_SimBriefUsername
            //
            resources.ApplyResources(this.Label_SimBriefUsername, "Label_SimBriefUsername");
            this.Label_SimBriefUsername.Name = "Label_SimBriefUsername";
            //
            // Text_SimBriefUsername
            //
            resources.ApplyResources(this.Text_SimBriefUsername, "Text_SimBriefUsername");
            this.Text_SimBriefUsername.Name = "Text_SimBriefUsername";
            //
            // Label_FolderPrompt
            //
            resources.ApplyResources(this.Label_FolderPrompt, "Label_FolderPrompt");
            this.Label_FolderPrompt.Name = "Label_FolderPrompt";
            //
            // Text_Folder
            //
            resources.ApplyResources(this.Text_Folder, "Text_Folder");
            this.Text_Folder.Name = "Text_Folder";
            //
            // Button_Browse
            //
            resources.ApplyResources(this.Button_Browse, "Button_Browse");
            this.Button_Browse.Name = "Button_Browse";
            this.Button_Browse.UseVisualStyleBackColor = true;
            this.Button_Browse.Click += new System.EventHandler(this.Button_Browse_Click);
            //
            // InitialSetupForm
            //
            this.AcceptButton = this.Button_OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Button_Browse);
            this.Controls.Add(this.Text_Folder);
            this.Controls.Add(this.Label_FolderPrompt);
            this.Controls.Add(this.Text_SimBriefUsername);
            this.Controls.Add(this.Label_SimBriefUsername);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Button_OK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Text_Nickname);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InitialSetupForm";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Text_Nickname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Button_OK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Label_SimBriefUsername;
        private System.Windows.Forms.TextBox Text_SimBriefUsername;
        private System.Windows.Forms.Label Label_FolderPrompt;
        private System.Windows.Forms.TextBox Text_Folder;
        private System.Windows.Forms.Button Button_Browse;
    }
}
