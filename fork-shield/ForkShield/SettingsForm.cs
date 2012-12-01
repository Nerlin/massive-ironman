using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;


using Core;

namespace ForkShield
{
    public partial class SettingsForm : Form
    {
        const string saveSettingsHeader = "Сохранение настроек";
        const string criticalHeader = "Программой ForkShield был заблокирован подозрительный процесс.";

        const int baloonTimeout = 500;

        private Protection Protector;
        private Notification NotifyForm;

        public SettingsForm()
        {
            InitializeComponent();

            this.Protector = new Protection();
            this.Protector.ProcessDetected += new ProcessDetectedHandler(Protector_ProcessDetected);
        }

        UserResponse Protector_ProcessDetected(ProcessDetectedEventArgs args)
        {
            NotifyForm = new Notification(args.DetectedProcess, args.ProcessesCount,
                 args.DetectionType);

            if (args.DetectionType == DetectionType.Warning)
            {
                NotifyForm.BringToFront();
                NotifyForm.ShowDialog();
            }
            else if (args.DetectionType == DetectionType.Critical)
            {
                string text = string.Format("Название процесса: {0}." + Environment.NewLine +
                                            "Количество процессов: {1}.", args.DetectedProcess, args.ProcessesCount);

                this.forkShieldIcon.ShowBalloonTip(baloonTimeout, criticalHeader,
                    text, ToolTipIcon.Warning);
            }


            UserResponse result = NotifyForm.Response;
            return result;
        }

        private void ShowCriticalNotification(object notification)
        {
            ((Notification)notification).Show();
        }

        private void Handler_SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ExitIsNeeded())
            {
                e.Cancel = true;
            }
        }

        private bool ExitIsNeeded()
        {
            DialogResult dialogResult = MessageBox.Show("Вы уверены, что хотите выйти из программы?", "Выход из программы",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void LoadSettings()
        {
            LoadProtectionType();
            LoadCriticalProcessesCount();
            LoadWarningProcessesCount();
        }

        private void LoadWarningProcessesCount()
        {
            this.warningNumber.Text = this.Protector.WarningProcessesCount.ToString();
        }

        private void LoadCriticalProcessesCount()
        {
            this.criticalNumber.Text = this.Protector.CriticalProcessesCount.ToString();
        }

        private void LoadProtectionType()
        {
            switch (this.Protector.ProtectionType)
            {
                case ProtectionType.Simple:
                    this.simpleProtection.Checked = true; break;
                case ProtectionType.Critical:
                    this.criticalProtection.Checked = true; break;
                default:
                    MessageBox.Show("Обнаружена ошибка с типом защиты. Обратитесь к разработчику."); break;
            }
        }

        private void CheckCriticalProcessesCount()
        {
            try
            {
                int criticalProcessesCount = Convert.ToInt16(this.criticalNumber.Text);
            }
            catch
            {
                MessageBox.Show("Данное поле должно быть числовым.", "Ошибка ввода данных",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.criticalNumber.Text = this.Protector.CriticalProcessesCount.ToString();
            }
        }

        private void CheckWarningProcessesCount()
        {
            try
            {
                int warningProcessesCount = Convert.ToInt16(this.warningNumber.Text);
            }
            catch
            {
                MessageBox.Show("Данное поле должно быть числовым.", "Ошибка ввода данных",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.warningNumber.Text = this.Protector.WarningProcessesCount.ToString();
            }
        }

        private void Handler_Save_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            SaveProtectionType();

            bool criticalProcessesCountSaved = SaveCriticalProcessesCount();
            bool warningProcessesCountSaved  = SaveWarningProcessesCount();

            if (criticalProcessesCountSaved && warningProcessesCountSaved)
            {
                try
                {
                    this.Protector.SaveSettings();
                    MessageBox.Show("Настройки успешно сохранены.", saveSettingsHeader,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (System.IO.IOException)
                {
                    DialogResult result = MessageBox.Show("Произошла ошибка ввода\\вывода." +
                    "Целостность конфигурации нарушена или файл конфигурации занят другим процессом.",
                        saveSettingsHeader, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);

                    if (result == System.Windows.Forms.DialogResult.Retry)
                    {
                        SaveSettings();
                    }
                }
            }
            else
            {
                LoadSettings();
            }
        }

        private bool SaveWarningProcessesCount()
        {
            CheckWarningProcessesCount();
            try
            {
                this.Protector.WarningProcessesCount = int.Parse(this.warningNumber.Text);
                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка ввода предупредительного количества процессов",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool SaveCriticalProcessesCount()
        {
            CheckCriticalProcessesCount();

            try
            {
                this.Protector.CriticalProcessesCount = int.Parse(this.criticalNumber.Text);
                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка ввода критического количества процессов",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void SaveProtectionType()
        {
            if (this.criticalProtection.Checked)
            {
                this.Protector.ProtectionType = ProtectionType.Critical;
            }
            else if (this.simpleProtection.Checked)
            {
                this.Protector.ProtectionType = ProtectionType.Simple;
            }
        }

        private void Handler_WarningNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                CheckWarningProcessesCount();
            }
        }

        private void Handler_CriticalNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                CheckCriticalProcessesCount();
            }
        }

        private void Handler_CriticalNumber_Leave(object sender, EventArgs e)
        {
            CheckCriticalProcessesCount();
        }

        private void Handler_WarningNumber_Leave(object sender, EventArgs e)
        {
            CheckWarningProcessesCount();
        }

        private void Initialize(object sender, EventArgs e)
        {
            LoadSettings();

            this.Protector.StartProtection();
        }

        private void Handler_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Handler_выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Handler_открытьНастройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }


    }
}
