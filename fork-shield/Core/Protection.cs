using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Core.Configs;

namespace Core
{
    /// <summary>
    /// Класс обеспечивает защиту приложения и защиту системы от запуска fork-приложений.
    /// </summary>
    public class Protection
    {
        const string ConfigsFile = "data.cfg";
        readonly SettingID ProtectionTypeSettingID = new SettingID("ProtectionType");
        readonly SettingID WarningProcessesCountSettingID = new SettingID("WarningProcesseCount");
        readonly SettingID CriticalProcessesCountSettingID = new SettingID("CriticalProcessesCount");
        readonly SettingID ProcessesExclusionID = new SettingID("ProcessesExclusion");

        /// <summary>
        /// Получает или задает уровень защиты системы.
        /// </summary>
        public ProtectionType ProtectionType
        {
            get { return this.ForkProtector.ProtectionType; }
            set
            {
                this.ForkProtector.ProtectionType = value;
                this.SettingsManager.SetSetting(ProtectionTypeSettingID, this.ForkProtector.ProtectionType);
            }
        }


        /// <summary>
        /// Получает или задает количество однотипных процессов, при котором будет выдаваться предупреждение,
        /// которое сообщает о большом количестве процессов.
        /// </summary>
        public int WarningProcessesCount
        {
            get { return this.ForkProtector.WarningProcessesCount; }
            set
            {
                try
                {
                    this.ForkProtector.WarningProcessesCount = value;
                    this.SettingsManager.SetSetting(WarningProcessesCountSettingID, this.ForkProtector.WarningProcessesCount);
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
        }


        /// <summary>
        /// Получает или задает количество однотипных процессов,
        /// при котором будут выполнены критические меры, такие как блокировка и аварийное завершение процессов.
        /// </summary>
        public int CriticalProcessesCount
        {
            get { return this.ForkProtector.CriticalProcessesCount; }
            set
            {
                try
                {
                    this.ForkProtector.CriticalProcessesCount = value;
                    this.SettingsManager.SetSetting(CriticalProcessesCountSettingID,
                        this.ForkProtector.CriticalProcessesCount);
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
        }


        /// <summary>
        /// Событие обнаружения подозрительного процесса.
        /// </summary>
        public event ProcessDetectedHandler ProcessDetected
        {
            add
            {
                this.ForkProtector.ProcessDetected -= value;
                this.ForkProtector.ProcessDetected += value;
            }
            remove
            {
                this.ForkProtector.ProcessDetected -= value;
            }
        }
        /// <summary>
        /// Объект, выполняющий защиту от fork-угроз.
        /// </summary>
        ForkProtection ForkProtector;

        SettingsCore SettingsManager;
        

        public Protection()
        {
            this.ForkProtector = new ForkProtection(Core.ProtectionType.Critical);
            this.SettingsManager = new SettingsCore();
            this.InitializeSettings();
            this.InitializeProperties();
        }

        #region Settings
        /// <summary>
        /// Инициализирует менеджер настроек.
        /// </summary>
        private void InitializeSettings()
        {
            if (SettingsAreSaved())
            {
                bool settingsLoaded = LoadSettings();

                if (!settingsLoaded)
                {
                    CreateSettings();
                }
            }
            else
            {
                CreateSettings();
            }
        }

        /// <summary>
        /// Проверяет, сохранены ли настройки.
        /// </summary>
        /// <returns></returns>
        private bool SettingsAreSaved()
        {
            return File.Exists(ConfigsFile);
        }

        /// <summary>
        /// Загружает настройки из файла.
        /// </summary>
        private bool LoadSettings()
        {           
            try
            {
                using (FileStream stream = File.Open(ConfigsFile, FileMode.Open))
                {
                    this.SettingsManager.LoadSettings(stream);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Сохраняет настройки.
        /// </summary>
        ///// <exception cref="SettingsIOException"></exception>
        /// <exception cref="IOException"></exception>
        public void SaveSettings()
        {
            try
            {
                using (FileStream stream = File.Open(Protection.ConfigsFile, FileMode.OpenOrCreate))
                {
                    this.SettingsManager.SaveSettings(stream);
                }
            }
            catch (SettingsIOException settingException)
            {
                throw new IOException(settingException.Message);
            }
            catch (IOException ioException)
            {
                throw ioException;
            }
        }

        /// <summary>
        /// Создает настройки для менеджера.
        /// </summary>
        private void CreateSettings()
        {
            this.SettingsManager.CreateSettingForManager(ProtectionTypeSettingID, this.ProtectionType);
            this.SettingsManager.CreateSettingForManager(WarningProcessesCountSettingID, this.WarningProcessesCount);
            this.SettingsManager.CreateSettingForManager(CriticalProcessesCountSettingID, this.CriticalProcessesCount);
            this.SettingsManager.CreateSettingForManager(ProcessesExclusionID, this.ForkProtector.ProcessesExclusion);
        }

        #endregion

        /// <summary>
        /// Инициализирует свойства базовыми значениями.
        /// </summary>
        private void InitializeProperties()
        {
            this.ProtectionType = this.SettingsManager.GetSetting(ProtectionTypeSettingID).GetValue<ProtectionType>();
            this.CriticalProcessesCount = this.SettingsManager.GetSetting(CriticalProcessesCountSettingID).GetValue<int>();
            this.WarningProcessesCount = this.SettingsManager.GetSetting(WarningProcessesCountSettingID).GetValue<int>();

            this.ForkProtector.ProcessesExclusion = this.SettingsManager.GetSetting(ProcessesExclusionID).GetValue<List<String>>();
        }

        /// <summary>
        /// Добавляет процесс в процессы-исключения.
        /// </summary>
        /// <param name="processName">Название процесса.</param>
        public void AddExclusionProcess(String processName)
        {
            this.ForkProtector.ProcessesExclusion.Add(processName);
        }

        /// <summary>
        /// Получает список процессов-исключений.
        /// </summary>
        /// <returns></returns>
        public String[] GetExclusionProcesses()
        {
            return this.ForkProtector.ProcessesExclusion.ToArray();
        }

        /// <summary>
        /// Удаляет процесс с заданным именем из списка процессов-исключений.
        /// </summary>
        /// <param name="processName">Название процесса.</param>
        public void RemoveExclusionProcess(String processName)
        {
            if (this.ForkProtector.ProcessesExclusion.Contains(processName))
            {
                this.ForkProtector.ProcessesExclusion.Remove(processName);
            }
        }

        /// <summary>
        /// Активирует работу защиты от fork-угроз.
        /// </summary>
        public void StartProtection()
        {
            this.ForkProtector.Start();
        }

        /// <summary>
        /// Останавливает работу защиты от fork-угроз.
        /// </summary>
        public void StopProtection()
        {
            this.ForkProtector.Stop();
        }


    }
}
