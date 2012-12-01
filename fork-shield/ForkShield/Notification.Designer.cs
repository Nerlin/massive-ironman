namespace ForkShield
{
    partial class Notification
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.processCount = new System.Windows.Forms.Label();
            this.processNameLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.notificationField = new System.Windows.Forms.Label();
            this.header = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.allow = new System.Windows.Forms.Button();
            this.block = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.processCount);
            this.groupBox1.Controls.Add(this.processNameLabel);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.notificationField);
            this.groupBox1.Controls.Add(this.header);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Location = new System.Drawing.Point(-12, -15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(337, 267);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // processCount
            // 
            this.processCount.AutoSize = true;
            this.processCount.Location = new System.Drawing.Point(161, 205);
            this.processCount.Name = "processCount";
            this.processCount.Size = new System.Drawing.Size(78, 13);
            this.processCount.TabIndex = 6;
            this.processCount.Text = "processCount";
            // 
            // processNameLabel
            // 
            this.processNameLabel.AutoSize = true;
            this.processNameLabel.Location = new System.Drawing.Point(161, 178);
            this.processNameLabel.Name = "processNameLabel";
            this.processNameLabel.Size = new System.Drawing.Size(75, 13);
            this.processNameLabel.TabIndex = 5;
            this.processNameLabel.Text = "processName";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 205);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Количество процессов:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 178);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Название процесса:";
            // 
            // notificationField
            // 
            this.notificationField.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.notificationField.Location = new System.Drawing.Point(157, 63);
            this.notificationField.Name = "notificationField";
            this.notificationField.Size = new System.Drawing.Size(160, 59);
            this.notificationField.TabIndex = 2;
            this.notificationField.Text = "Программой был обнаружен подозрительный процесс.";
            // 
            // header
            // 
            this.header.AutoSize = true;
            this.header.Font = new System.Drawing.Font("Segoe UI Light", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.header.Location = new System.Drawing.Point(156, 28);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(94, 25);
            this.header.TabIndex = 1;
            this.header.Text = "ForkShield";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::ForkShield.Properties.Resources.Shield;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(25, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(125, 125);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // allow
            // 
            this.allow.Location = new System.Drawing.Point(117, 258);
            this.allow.Name = "allow";
            this.allow.Size = new System.Drawing.Size(75, 23);
            this.allow.TabIndex = 1;
            this.allow.Text = "Разрешить";
            this.allow.UseVisualStyleBackColor = true;
            this.allow.Click += new System.EventHandler(this.Handler_Allow_Click);
            // 
            // block
            // 
            this.block.Location = new System.Drawing.Point(198, 258);
            this.block.Name = "block";
            this.block.Size = new System.Drawing.Size(106, 23);
            this.block.TabIndex = 2;
            this.block.Text = "Заблокировать";
            this.block.UseVisualStyleBackColor = true;
            this.block.Click += new System.EventHandler(this.Handler_Block_Click);
            // 
            // Notification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(316, 285);
            this.Controls.Add(this.block);
            this.Controls.Add(this.allow);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Notification";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ForkShield - Уведомление";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Notification_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button allow;
        private System.Windows.Forms.Button block;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label processCount;
        private System.Windows.Forms.Label processNameLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label notificationField;
        private System.Windows.Forms.Label header;
    }
}