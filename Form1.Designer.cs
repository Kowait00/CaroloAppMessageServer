namespace CaroloAppMessageServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.senderInterfaceComboBox = new System.Windows.Forms.ComboBox();
            this.senderInterfaceHeadingLabel = new System.Windows.Forms.Label();
            this.senderInterfaceWarningLabel = new System.Windows.Forms.Label();
            this.dummyDataCheckBox = new System.Windows.Forms.CheckBox();
            this.dataReceivedOutputLabel = new System.Windows.Forms.Label();
            this.dataReceivedOutputTextBox = new System.Windows.Forms.RichTextBox();
            this.runServerCheckbox = new System.Windows.Forms.CheckBox();
            this.receiverPortDescriptionLabel = new System.Windows.Forms.Label();
            this.receiverPortInputTextBox = new System.Windows.Forms.TextBox();
            this.receiverPortWarningLabel = new System.Windows.Forms.Label();
            this.setupGroupBox = new System.Windows.Forms.GroupBox();
            this.setupGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(338, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Network interface to receive UDP packets on:";
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(9, 48);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(640, 24);
            this.comboBox1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(6, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(437, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Selected network interface is not supported. Please try another one.";
            this.label2.Visible = false;
            // 
            // senderInterfaceComboBox
            // 
            this.senderInterfaceComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.senderInterfaceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.senderInterfaceComboBox.FormattingEnabled = true;
            this.senderInterfaceComboBox.Items.AddRange(new object[] {
            "Interface 1",
            "Interface 2"});
            this.senderInterfaceComboBox.Location = new System.Drawing.Point(9, 162);
            this.senderInterfaceComboBox.Name = "senderInterfaceComboBox";
            this.senderInterfaceComboBox.Size = new System.Drawing.Size(640, 24);
            this.senderInterfaceComboBox.TabIndex = 4;
            this.senderInterfaceComboBox.DropDown += new System.EventHandler(this.senderInterfaceComboBox_DropDown);
            // 
            // senderInterfaceHeadingLabel
            // 
            this.senderInterfaceHeadingLabel.AutoSize = true;
            this.senderInterfaceHeadingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.senderInterfaceHeadingLabel.Location = new System.Drawing.Point(6, 142);
            this.senderInterfaceHeadingLabel.Name = "senderInterfaceHeadingLabel";
            this.senderInterfaceHeadingLabel.Size = new System.Drawing.Size(375, 17);
            this.senderInterfaceHeadingLabel.TabIndex = 5;
            this.senderInterfaceHeadingLabel.Text = "Network interface to send the UDP packages from:";
            // 
            // senderInterfaceWarningLabel
            // 
            this.senderInterfaceWarningLabel.AutoSize = true;
            this.senderInterfaceWarningLabel.ForeColor = System.Drawing.Color.Red;
            this.senderInterfaceWarningLabel.Location = new System.Drawing.Point(6, 189);
            this.senderInterfaceWarningLabel.Name = "senderInterfaceWarningLabel";
            this.senderInterfaceWarningLabel.Size = new System.Drawing.Size(437, 17);
            this.senderInterfaceWarningLabel.TabIndex = 6;
            this.senderInterfaceWarningLabel.Text = "Selected network interface is not supported. Please try another one.";
            this.senderInterfaceWarningLabel.Visible = false;
            // 
            // dummyDataCheckBox
            // 
            this.dummyDataCheckBox.AutoSize = true;
            this.dummyDataCheckBox.Location = new System.Drawing.Point(9, 211);
            this.dummyDataCheckBox.Name = "dummyDataCheckBox";
            this.dummyDataCheckBox.Size = new System.Drawing.Size(307, 21);
            this.dummyDataCheckBox.TabIndex = 7;
            this.dummyDataCheckBox.Text = "Generate dummy data on loopback address";
            this.dummyDataCheckBox.UseVisualStyleBackColor = true;
            // 
            // dataReceivedOutputLabel
            // 
            this.dataReceivedOutputLabel.AutoSize = true;
            this.dataReceivedOutputLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataReceivedOutputLabel.Location = new System.Drawing.Point(12, 321);
            this.dataReceivedOutputLabel.Name = "dataReceivedOutputLabel";
            this.dataReceivedOutputLabel.Size = new System.Drawing.Size(114, 17);
            this.dataReceivedOutputLabel.TabIndex = 9;
            this.dataReceivedOutputLabel.Text = "Data received:";
            // 
            // dataReceivedOutputTextBox
            // 
            this.dataReceivedOutputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataReceivedOutputTextBox.Location = new System.Drawing.Point(12, 341);
            this.dataReceivedOutputTextBox.Name = "dataReceivedOutputTextBox";
            this.dataReceivedOutputTextBox.ReadOnly = true;
            this.dataReceivedOutputTextBox.Size = new System.Drawing.Size(658, 200);
            this.dataReceivedOutputTextBox.TabIndex = 10;
            this.dataReceivedOutputTextBox.Text = "";
            this.dataReceivedOutputTextBox.TextChanged += new System.EventHandler(this.dataReceivedOutputTextBox_TextChanged);
            // 
            // runServerCheckbox
            // 
            this.runServerCheckbox.Appearance = System.Windows.Forms.Appearance.Button;
            this.runServerCheckbox.AutoSize = true;
            this.runServerCheckbox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.runServerCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runServerCheckbox.Location = new System.Drawing.Point(15, 273);
            this.runServerCheckbox.MinimumSize = new System.Drawing.Size(200, 35);
            this.runServerCheckbox.Name = "runServerCheckbox";
            this.runServerCheckbox.Size = new System.Drawing.Size(200, 35);
            this.runServerCheckbox.TabIndex = 12;
            this.runServerCheckbox.Text = "Start Server";
            this.runServerCheckbox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.runServerCheckbox.UseVisualStyleBackColor = true;
            this.runServerCheckbox.CheckedChanged += new System.EventHandler(this.runServerCheckbox_CheckedChanged);
            // 
            // receiverPortDescriptionLabel
            // 
            this.receiverPortDescriptionLabel.AutoSize = true;
            this.receiverPortDescriptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.receiverPortDescriptionLabel.Location = new System.Drawing.Point(6, 99);
            this.receiverPortDescriptionLabel.Name = "receiverPortDescriptionLabel";
            this.receiverPortDescriptionLabel.Size = new System.Drawing.Size(241, 17);
            this.receiverPortDescriptionLabel.TabIndex = 13;
            this.receiverPortDescriptionLabel.Text = "Port to receive UDP packets on:";
            // 
            // receiverPortInputTextBox
            // 
            this.receiverPortInputTextBox.Location = new System.Drawing.Point(253, 94);
            this.receiverPortInputTextBox.MaxLength = 5;
            this.receiverPortInputTextBox.Name = "receiverPortInputTextBox";
            this.receiverPortInputTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.receiverPortInputTextBox.Size = new System.Drawing.Size(46, 22);
            this.receiverPortInputTextBox.TabIndex = 14;
            this.receiverPortInputTextBox.Text = "8626";
            this.receiverPortInputTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.receiverPortInputTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.receiverPortInputTextBox_KeyPress);
            // 
            // receiverPortWarningLabel
            // 
            this.receiverPortWarningLabel.AutoSize = true;
            this.receiverPortWarningLabel.ForeColor = System.Drawing.Color.Red;
            this.receiverPortWarningLabel.Location = new System.Drawing.Point(305, 99);
            this.receiverPortWarningLabel.Name = "receiverPortWarningLabel";
            this.receiverPortWarningLabel.Size = new System.Drawing.Size(104, 17);
            this.receiverPortWarningLabel.TabIndex = 15;
            this.receiverPortWarningLabel.Text = "Not a valid port";
            this.receiverPortWarningLabel.Visible = false;
            // 
            // setupGroupBox
            // 
            this.setupGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.setupGroupBox.Controls.Add(this.label1);
            this.setupGroupBox.Controls.Add(this.receiverPortWarningLabel);
            this.setupGroupBox.Controls.Add(this.comboBox1);
            this.setupGroupBox.Controls.Add(this.receiverPortInputTextBox);
            this.setupGroupBox.Controls.Add(this.label2);
            this.setupGroupBox.Controls.Add(this.receiverPortDescriptionLabel);
            this.setupGroupBox.Controls.Add(this.senderInterfaceComboBox);
            this.setupGroupBox.Controls.Add(this.senderInterfaceHeadingLabel);
            this.setupGroupBox.Controls.Add(this.senderInterfaceWarningLabel);
            this.setupGroupBox.Controls.Add(this.dummyDataCheckBox);
            this.setupGroupBox.Location = new System.Drawing.Point(15, 12);
            this.setupGroupBox.Name = "setupGroupBox";
            this.setupGroupBox.Size = new System.Drawing.Size(655, 246);
            this.setupGroupBox.TabIndex = 16;
            this.setupGroupBox.TabStop = false;
            this.setupGroupBox.Text = "Setup";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 553);
            this.Controls.Add(this.setupGroupBox);
            this.Controls.Add(this.runServerCheckbox);
            this.Controls.Add(this.dataReceivedOutputTextBox);
            this.Controls.Add(this.dataReceivedOutputLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(550, 450);
            this.Name = "Form1";
            this.Text = "Carolo App Message Server";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.setupGroupBox.ResumeLayout(false);
            this.setupGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox senderInterfaceComboBox;
        private System.Windows.Forms.Label senderInterfaceHeadingLabel;
        private System.Windows.Forms.Label senderInterfaceWarningLabel;
        private System.Windows.Forms.CheckBox dummyDataCheckBox;
        private System.Windows.Forms.Label dataReceivedOutputLabel;
        private System.Windows.Forms.RichTextBox dataReceivedOutputTextBox;
        private System.Windows.Forms.CheckBox runServerCheckbox;
        private System.Windows.Forms.Label receiverPortDescriptionLabel;
        private System.Windows.Forms.TextBox receiverPortInputTextBox;
        private System.Windows.Forms.Label receiverPortWarningLabel;
        private System.Windows.Forms.GroupBox setupGroupBox;
    }
}

