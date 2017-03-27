namespace KinectTest
{
    partial class Form1
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
            this.midSpineX = new System.Windows.Forms.TextBox();
            this.midSpineY = new System.Windows.Forms.TextBox();
            this.midSpineZ = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lhz = new System.Windows.Forms.TextBox();
            this.lhy = new System.Windows.Forms.TextBox();
            this.lhx = new System.Windows.Forms.TextBox();
            this.palmDownRadio = new System.Windows.Forms.RadioButton();
            this.palmUpRadio = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // midSpineX
            // 
            this.midSpineX.BackColor = System.Drawing.SystemColors.Window;
            this.midSpineX.Location = new System.Drawing.Point(766, 96);
            this.midSpineX.Name = "midSpineX";
            this.midSpineX.Size = new System.Drawing.Size(154, 22);
            this.midSpineX.TabIndex = 0;
            // 
            // midSpineY
            // 
            this.midSpineY.Location = new System.Drawing.Point(766, 153);
            this.midSpineY.Name = "midSpineY";
            this.midSpineY.Size = new System.Drawing.Size(154, 22);
            this.midSpineY.TabIndex = 1;
            // 
            // midSpineZ
            // 
            this.midSpineZ.Location = new System.Drawing.Point(766, 213);
            this.midSpineZ.Name = "midSpineZ";
            this.midSpineZ.Size = new System.Drawing.Size(154, 22);
            this.midSpineZ.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(581, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "RightHandX";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(581, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "RightHandY";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(581, 213);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "RightHandZ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(54, 196);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "LeftHandZ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(54, 134);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "LeftHandY";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(54, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 17);
            this.label6.TabIndex = 9;
            this.label6.Text = "LeftHandX";
            // 
            // lhz
            // 
            this.lhz.Location = new System.Drawing.Point(239, 196);
            this.lhz.Name = "lhz";
            this.lhz.Size = new System.Drawing.Size(154, 22);
            this.lhz.TabIndex = 8;
            // 
            // lhy
            // 
            this.lhy.Location = new System.Drawing.Point(239, 136);
            this.lhy.Name = "lhy";
            this.lhy.Size = new System.Drawing.Size(154, 22);
            this.lhy.TabIndex = 7;
            // 
            // lhx
            // 
            this.lhx.BackColor = System.Drawing.SystemColors.Window;
            this.lhx.Location = new System.Drawing.Point(239, 79);
            this.lhx.Name = "lhx";
            this.lhx.Size = new System.Drawing.Size(154, 22);
            this.lhx.TabIndex = 6;
            // 
            // palmDownRadio
            // 
            this.palmDownRadio.AutoSize = true;
            this.palmDownRadio.Location = new System.Drawing.Point(115, 280);
            this.palmDownRadio.Name = "palmDownRadio";
            this.palmDownRadio.Size = new System.Drawing.Size(94, 21);
            this.palmDownRadio.TabIndex = 12;
            this.palmDownRadio.TabStop = true;
            this.palmDownRadio.Text = "palmDown";
            this.palmDownRadio.UseVisualStyleBackColor = true;
            this.palmDownRadio.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // palmUpRadio
            // 
            this.palmUpRadio.AutoSize = true;
            this.palmUpRadio.Location = new System.Drawing.Point(268, 280);
            this.palmUpRadio.Name = "palmUpRadio";
            this.palmUpRadio.Size = new System.Drawing.Size(77, 21);
            this.palmUpRadio.TabIndex = 13;
            this.palmUpRadio.TabStop = true;
            this.palmUpRadio.Text = "palmUp";
            this.palmUpRadio.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.palmUpRadio);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.palmDownRadio);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lhx);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lhy);
            this.groupBox1.Controls.Add(this.lhz);
            this.groupBox1.Location = new System.Drawing.Point(26, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(439, 321);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(584, 292);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(81, 17);
            this.statusLabel.TabIndex = 15;
            this.statusLabel.Text = "Drone is off";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 335);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.midSpineZ);
            this.Controls.Add(this.midSpineY);
            this.Controls.Add(this.midSpineX);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox midSpineX;
        private System.Windows.Forms.TextBox midSpineY;
        private System.Windows.Forms.TextBox midSpineZ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox lhz;
        private System.Windows.Forms.TextBox lhy;
        private System.Windows.Forms.TextBox lhx;
        private System.Windows.Forms.RadioButton palmDownRadio;
        private System.Windows.Forms.RadioButton palmUpRadio;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label statusLabel;
    }
}

