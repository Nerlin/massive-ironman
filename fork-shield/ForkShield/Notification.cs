using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Core;

namespace ForkShield
{
    public partial class Notification : Form
    {
        private DetectionType DetectionType;
        private String ProcessName;
        private int ProcessCount;


        const string warningText = "Программой был обнаружен подозрительный процесс.";
        const string criticalText = "Программой был заблокирован подозрительный процесс.";

        /// <summary>
        /// Реакция пользователя на уведомление.
        /// </summary>
        public UserResponse Response
        {
            get;
            private set;
        }

        public Notification(String processName, int processCount, DetectionType detectionType)
        {
            this.ProcessName = processName;
            this.DetectionType = detectionType;
            this.ProcessCount = processCount;

            this.Response = UserResponse.NoResponse;

            InitializeComponent();
            SetPosition();

            RefreshComponents();
        }

        private void SetPosition()
        {
            int toolbarHeight = Screen.PrimaryScreen.Bounds.Height - Screen.PrimaryScreen.WorkingArea.Height;
            int x = Screen.PrimaryScreen.Bounds.Width - this.Size.Width;
            int y = Screen.PrimaryScreen.Bounds.Height - this.Size.Height - toolbarHeight;
            this.Location = new Point(x, y);
        }

        private void RefreshComponents()
        {
            this.processNameLabel.Text = this.ProcessName;
            this.processCount.Text = this.ProcessCount.ToString();

            if (this.DetectionType == Core.DetectionType.Warning)
            {
                this.notificationField.Text = warningText;
            }
            else if (this.DetectionType == Core.DetectionType.Critical)
            {
                this.notificationField.Text = criticalText;
            }

            SetButtonVisibility();
        }

        private void SetButtonVisibility()
        {
            this.allow.Visible = this.DetectionType == Core.DetectionType.Warning;
            this.block.Visible = this.DetectionType == Core.DetectionType.Warning;
        }

        private void Handler_Allow_Click(object sender, EventArgs e)
        {
            this.Response = UserResponse.Skip;
            this.Close();
        }

        private void Handler_Block_Click(object sender, EventArgs e)
        {
            this.Response = UserResponse.Kill;
            this.Close();
        }

        private void Notification_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Response == UserResponse.NoResponse)
            {
                if (this.DetectionType == Core.DetectionType.Critical)
                {
                    this.Response = UserResponse.OK;
                }
                else if (this.DetectionType == Core.DetectionType.Warning)
                {
                    e.Cancel = true;
                }
            }
        }


        
    }
}
