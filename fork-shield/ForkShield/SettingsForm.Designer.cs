namespace ForkShield
{
    partial class SettingsForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.criticalNumber = new System.Windows.Forms.TextBox();
            this.warningNumber = new System.Windows.Forms.TextBox();
            this.protectionType = new System.Windows.Forms.GroupBox();
            this.criticalProtection = new System.Windows.Forms.RadioButton();
            this.simpleProtection = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.save = new System.Windows.Forms.Button();
            this.exit = new System.Windows.Forms.Button();
            this.mainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.forkShieldIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.iconMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.открытьНастройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.protectionType.SuspendLayout();
            this.iconMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox1.Controls.Add(this.criticalNumber);
            this.groupBox1.Controls.Add(this.warningNumber);
            this.groupBox1.Controls.Add(this.protectionType);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(-6, -15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(394, 300);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // criticalNumber
            // 
            this.criticalNumber.Location = new System.Drawing.Point(294, 250);
            this.criticalNumber.Name = "criticalNumber";
            this.criticalNumber.Size = new System.Drawing.Size(82, 22);
            this.criticalNumber.TabIndex = 5;
            this.criticalNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Handler_CriticalNumber_KeyDown);
            this.criticalNumber.Leave += new System.EventHandler(this.Handler_CriticalNumber_Leave);
            // 
            // warningNumber
            // 
            this.warningNumber.Location = new System.Drawing.Point(294, 214);
            this.warningNumber.Name = "warningNumber";
            this.warningNumber.Size = new System.Drawing.Size(82, 22);
            this.warningNumber.TabIndex = 4;
            this.warningNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Handler_WarningNumber_KeyDown);
            this.warningNumber.Leave += new System.EventHandler(this.Handler_WarningNumber_Leave);
            // 
            // protectionType
            // 
            this.protectionType.Controls.Add(this.criticalProtection);
            this.protectionType.Controls.Add(this.simpleProtection);
            this.protectionType.Location = new System.Drawing.Point(24, 73);
            this.protectionType.Name = "protectionType";
            this.protectionType.Size = new System.Drawing.Size(352, 115);
            this.protectionType.TabIndex = 3;
            this.protectionType.TabStop = false;
            this.protectionType.Text = "Уровень защиты:";
            // 
            // criticalProtection
            // 
            this.criticalProtection.AutoSize = true;
            this.criticalProtection.Checked = true;
            this.criticalProtection.Location = new System.Drawing.Point(17, 76);
            this.criticalProtection.Name = "criticalProtection";
            this.criticalProtection.Size = new System.Drawing.Size(182, 17);
            this.criticalProtection.TabIndex = 1;
            this.criticalProtection.TabStop = true;
            this.criticalProtection.Text = "Критический (рекомендуемый)";
            this.mainToolTip.SetToolTip(this.criticalProtection, "В критическом режиме приложение автоматически будет блокировать \r\nвсе подозритель" +
        "ные процессы при обнаружении угрозы.\r\n\r\nДанный режим является рекомендуемым.");
            this.criticalProtection.UseVisualStyleBackColor = true;
            // 
            // simpleProtection
            // 
            this.simpleProtection.Location = new System.Drawing.Point(17, 33);
            this.simpleProtection.Name = "simpleProtection";
            this.simpleProtection.Size = new System.Drawing.Size(78, 17);
            this.simpleProtection.TabIndex = 0;
            this.simpleProtection.Text = "Обычный";
            this.mainToolTip.SetToolTip(this.simpleProtection, resources.GetString("simpleProtection.ToolTip"));
            this.simpleProtection.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 253);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(273, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Критическое количество однотипных процессов:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(18, 209);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(214, 27);
            this.label2.TabIndex = 1;
            this.label2.Text = "Выдавать предупреждение при следующем количестве процессов:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Light", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(19, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Настройки";
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(218, 291);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(75, 23);
            this.save.TabIndex = 1;
            this.save.Text = "Сохранить";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.Handler_Save_Click);
            // 
            // exit
            // 
            this.exit.Location = new System.Drawing.Point(299, 291);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(75, 23);
            this.exit.TabIndex = 2;
            this.exit.Text = "Выйти";
            this.exit.UseVisualStyleBackColor = true;
            this.exit.Click += new System.EventHandler(this.Handler_Exit_Click);
            // 
            // mainToolTip
            // 
            this.mainToolTip.AutoPopDelay = 5000;
            this.mainToolTip.InitialDelay = 100;
            this.mainToolTip.ReshowDelay = 100;
            // 
            // forkShieldIcon
            // 
            this.forkShieldIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.forkShieldIcon.ContextMenuStrip = this.iconMenu;
            this.forkShieldIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("forkShieldIcon.Icon")));
            this.forkShieldIcon.Text = "ForkShield";
            this.forkShieldIcon.Visible = true;
            // 
            // iconMenu
            // 
            this.iconMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открытьНастройкиToolStripMenuItem,
            this.toolStripMenuItem1,
            this.выходToolStripMenuItem});
            this.iconMenu.Name = "iconMenu";
            this.iconMenu.Size = new System.Drawing.Size(193, 76);
            // 
            // открытьНастройкиToolStripMenuItem
            // 
            this.открытьНастройкиToolStripMenuItem.Name = "открытьНастройкиToolStripMenuItem";
            this.открытьНастройкиToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.открытьНастройкиToolStripMenuItem.Text = "Открыть настройки";
            this.открытьНастройкиToolStripMenuItem.Click += new System.EventHandler(this.Handler_открытьНастройкиToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(189, 6);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.выходToolStripMenuItem.Text = "Выход из программы";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.Handler_выходToolStripMenuItem_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(253)))));
            this.ClientSize = new System.Drawing.Size(382, 320);
            this.Controls.Add(this.exit);
            this.Controls.Add(this.save);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(388, 348);
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ForkShield - Настройки";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Handler_SettingsForm_FormClosing);
            this.Load += new System.EventHandler(this.Initialize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.protectionType.ResumeLayout(false);
            this.protectionType.PerformLayout();
            this.iconMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox criticalNumber;
        private System.Windows.Forms.TextBox warningNumber;
        private System.Windows.Forms.GroupBox protectionType;
        private System.Windows.Forms.RadioButton criticalProtection;
        private System.Windows.Forms.RadioButton simpleProtection;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button exit;
        private System.Windows.Forms.ToolTip mainToolTip;
        private System.Windows.Forms.NotifyIcon forkShieldIcon;
        private System.Windows.Forms.ContextMenuStrip iconMenu;
        private System.Windows.Forms.ToolStripMenuItem открытьНастройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
    }
}

