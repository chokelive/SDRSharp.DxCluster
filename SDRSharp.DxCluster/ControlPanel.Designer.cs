namespace SDRSharp.DxCluster
{
    partial class ControlPanel
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
            lbl_cluter_status = new System.Windows.Forms.Label();
            txt_callsign = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            lbl_version = new System.Windows.Forms.Label();
            chk_enable = new System.Windows.Forms.CheckBox();
            SuspendLayout();
            // 
            // lbl_cluter_status
            // 
            lbl_cluter_status.AutoSize = true;
            lbl_cluter_status.Location = new System.Drawing.Point(0, 66);
            lbl_cluter_status.Name = "lbl_cluter_status";
            lbl_cluter_status.Size = new System.Drawing.Size(90, 15);
            lbl_cluter_status.TabIndex = 0;
            lbl_cluter_status.Text = "DxClusterStatus";
            // 
            // txt_callsign
            // 
            txt_callsign.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txt_callsign.Location = new System.Drawing.Point(6, 30);
            txt_callsign.Name = "txt_callsign";
            txt_callsign.Size = new System.Drawing.Size(73, 22);
            txt_callsign.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(77, 15);
            label1.TabIndex = 3;
            label1.Text = "your callsign:";
            // 
            // lbl_version
            // 
            lbl_version.AutoSize = true;
            lbl_version.Location = new System.Drawing.Point(3, 94);
            lbl_version.Name = "lbl_version";
            lbl_version.Size = new System.Drawing.Size(45, 15);
            lbl_version.TabIndex = 5;
            lbl_version.Text = "version";
            // 
            // chk_enable
            // 
            chk_enable.AutoSize = true;
            chk_enable.Location = new System.Drawing.Point(86, 33);
            chk_enable.Name = "chk_enable";
            chk_enable.Size = new System.Drawing.Size(61, 19);
            chk_enable.TabIndex = 6;
            chk_enable.Text = "Enable";
            chk_enable.UseVisualStyleBackColor = true;
            chk_enable.CheckedChanged += chk_enable_CheckedChanged;
            // 
            // ControlPanel
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(chk_enable);
            Controls.Add(lbl_version);
            Controls.Add(label1);
            Controls.Add(txt_callsign);
            Controls.Add(lbl_cluter_status);
            Name = "ControlPanel";
            Size = new System.Drawing.Size(150, 124);
            Load += ControlPanel_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lbl_cluter_status;
        private System.Windows.Forms.TextBox txt_callsign;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_version;
        private System.Windows.Forms.CheckBox chk_enable;
    }
}
