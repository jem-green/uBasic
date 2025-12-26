using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace uBasicForm
{
    partial class uBasicUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uBasicUI));
            consoleTextBox = new TextBox();
            consoleMenuStrip = new MenuStrip();
            fileMenuItem = new ToolStripMenuItem();
            openFileMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            recentFileToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            formatToolStripMenuItem = new ToolStripMenuItem();
            fontToolStripMenuItem = new ToolStripMenuItem();
            colorToolStripMenuItem = new ToolStripMenuItem();
            consoleMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // consoleTextBox
            // 
            consoleTextBox.Dock = DockStyle.Fill;
            consoleTextBox.Enabled = false;
            consoleTextBox.Location = new Point(0, 44);
            consoleTextBox.Margin = new Padding(4);
            consoleTextBox.Multiline = true;
            consoleTextBox.Name = "consoleTextBox";
            consoleTextBox.ReadOnly = true;
            consoleTextBox.ScrollBars = ScrollBars.Vertical;
            consoleTextBox.Size = new Size(866, 633);
            consoleTextBox.TabIndex = 0;
            consoleTextBox.Visible = false;
            consoleTextBox.KeyPress += ConsoleTextBox_KeyPress;
            // 
            // consoleMenuStrip
            // 
            consoleMenuStrip.ImageScalingSize = new Size(32, 32);
            consoleMenuStrip.Items.AddRange(new ToolStripItem[] { fileMenuItem, formatToolStripMenuItem });
            consoleMenuStrip.Location = new Point(0, 0);
            consoleMenuStrip.Name = "consoleMenuStrip";
            consoleMenuStrip.Padding = new Padding(7, 3, 0, 3);
            consoleMenuStrip.Size = new Size(866, 44);
            consoleMenuStrip.TabIndex = 1;
            consoleMenuStrip.Text = "menuStrip1";
            // 
            // fileMenuItem
            // 
            fileMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openFileMenuItem, toolStripSeparator1, recentFileToolStripMenuItem, toolStripSeparator2, exitToolStripMenuItem });
            fileMenuItem.Name = "fileMenuItem";
            fileMenuItem.Size = new Size(71, 38);
            fileMenuItem.Text = "File";
            // 
            // openFileMenuItem
            // 
            openFileMenuItem.Name = "openFileMenuItem";
            openFileMenuItem.Size = new Size(263, 44);
            openFileMenuItem.Text = "&Open";
            openFileMenuItem.Click += FileOpenMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(260, 6);
            // 
            // recentFileToolStripMenuItem
            // 
            recentFileToolStripMenuItem.Name = "recentFileToolStripMenuItem";
            recentFileToolStripMenuItem.Size = new Size(263, 44);
            recentFileToolStripMenuItem.Text = "Recent File";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(260, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(263, 44);
            exitToolStripMenuItem.Text = "&Exit";
            exitToolStripMenuItem.Click += FileExitMenuItem_Click;
            // 
            // formatToolStripMenuItem
            // 
            formatToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { fontToolStripMenuItem, colorToolStripMenuItem });
            formatToolStripMenuItem.Name = "formatToolStripMenuItem";
            formatToolStripMenuItem.Size = new Size(109, 38);
            formatToolStripMenuItem.Text = "Format";
            // 
            // fontToolStripMenuItem
            // 
            fontToolStripMenuItem.Name = "fontToolStripMenuItem";
            fontToolStripMenuItem.Size = new Size(359, 44);
            fontToolStripMenuItem.Text = "Font";
            fontToolStripMenuItem.Click += FormatFontMenuItem_Click;
            // 
            // colorToolStripMenuItem
            // 
            colorToolStripMenuItem.Name = "colorToolStripMenuItem";
            colorToolStripMenuItem.Size = new Size(359, 44);
            colorToolStripMenuItem.Text = "Color";
            colorToolStripMenuItem.Click += FormatColorMenuItem_Click;
            // 
            // uBasicUI
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(866, 677);
            Controls.Add(consoleTextBox);
            Controls.Add(consoleMenuStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = consoleMenuStrip;
            Margin = new Padding(4);
            Name = "uBasicUI";
            Text = "uBasic";
            FormClosing += ConsoleForm_FormClosing;
            Load += ConsoleForm_Load;
            consoleMenuStrip.ResumeLayout(false);
            consoleMenuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox consoleTextBox;
        private System.Windows.Forms.MenuStrip consoleMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem formatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem recentFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}

