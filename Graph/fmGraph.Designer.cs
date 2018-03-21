namespace Graph
{
    partial class fmGraph
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
            this.grPoints = new System.Windows.Forms.DataGridView();
            this.muTools = new System.Windows.Forms.MenuStrip();
            this.miAddPoints = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.grPoints)).BeginInit();
            this.muTools.SuspendLayout();
            this.SuspendLayout();
            // 
            // grPoints
            // 
            this.grPoints.AllowUserToAddRows = false;
            this.grPoints.AllowUserToDeleteRows = false;
            this.grPoints.AllowUserToResizeColumns = false;
            this.grPoints.AllowUserToResizeRows = false;
            this.grPoints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grPoints.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grPoints.GridColor = System.Drawing.SystemColors.Control;
            this.grPoints.Location = new System.Drawing.Point(0, 27);
            this.grPoints.MultiSelect = false;
            this.grPoints.Name = "grPoints";
            this.grPoints.RowTemplate.Height = 23;
            this.grPoints.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grPoints.ShowEditingIcon = false;
            this.grPoints.Size = new System.Drawing.Size(128, 150);
            this.grPoints.TabIndex = 1;
            this.grPoints.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grPoints_CellEndEdit);
            this.grPoints.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.grPoints_DataError);
            // 
            // muTools
            // 
            this.muTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAddPoints});
            this.muTools.Location = new System.Drawing.Point(0, 0);
            this.muTools.Name = "muTools";
            this.muTools.Size = new System.Drawing.Size(784, 24);
            this.muTools.TabIndex = 2;
            this.muTools.Text = "menuStrip1";
            // 
            // miAddPoints
            // 
            this.miAddPoints.Name = "miAddPoints";
            this.miAddPoints.Size = new System.Drawing.Size(70, 20);
            this.miAddPoints.Text = "Add points";
            this.miAddPoints.Click += new System.EventHandler(this.miAddPoints_Click);
            // 
            // fmGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.grPoints);
            this.Controls.Add(this.muTools);
            this.MainMenuStrip = this.muTools;
            this.Name = "fmGraph";
            this.Text = "fmGraph";
            this.Shown += new System.EventHandler(this.fmGraph_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.fmGraph_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.fmGraph_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.fmGraph_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.grPoints)).EndInit();
            this.muTools.ResumeLayout(false);
            this.muTools.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView grPoints;
        private System.Windows.Forms.MenuStrip muTools;
        private System.Windows.Forms.ToolStripMenuItem miAddPoints;
    }
}

