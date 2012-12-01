using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace Core.Configs
{
    /// <summary>
    /// Исключение, возникающее когда не удается найти настройку.
    /// </summary>
    class SettingNotFoundException : Exception
    {
        const string baseMessage = "Настройка {0} не найдена.";
        const string nameNotice  = "Название настройки: {0}.";

        public SettingNotFoundException(SettingID name) :
            base(String.Format(baseMessage, name.Value)) { }
        public SettingNotFoundException(SettingID name, string message) :
            base(String.Format(message + Environment.NewLine + nameNotice, name.Value)) { } 
    }

    /// <summary>
    /// Исключение, возникающее при попытке записать в настройку значение неверного типа.
    /// </summary>
    class SettingValueTypeMismatchException : Exception
    {
        const string baseMessage = "Значение с заданным типом для настройки не допустимо.";
        const string nameNotice  = "Название настройки: {0}.";
        const string typesMessage = "Тип настройки: {0}. Тип значения: {1}.";

        /// <summary>
        /// Идентификатор настройки.
        /// </summary>
        public SettingID SettingID
        {
            get;
            private set;
        }

        /// <summary>
        /// Тип значения настройки.
        /// </summary>
        public Type SettingType
        {
            get;
            private set;
        }

        /// <summary>
        /// Тип записываемого значения.
        /// </summary>
        public Type ValueType
        {
            get;
            private set;
        }

        public SettingValueTypeMismatchException():
            base(baseMessage) { }

        public SettingValueTypeMismatchException(SettingID name):
            base(string.Format(baseMessage + Environment.NewLine + nameNotice, name.Value)) 
        { 
            this.SettingID = name;
        }

        public SettingValueTypeMismatchException(Type settingType, Type valueType):
            base(string.Format(baseMessage + Environment.NewLine + typesMessage, settingType, valueType))
        {
            this.SettingType = settingType;
            this.ValueType = valueType;
        }

        public SettingValueTypeMismatchException(SettingID name, Type settingType, Type valueType):
            base(string.Format(baseMessage + Environment.NewLine + nameNotice, name.Value) +
                 Environment.NewLine +
                 string.Format(typesMessage, settingType, valueType)) 
        {
            this.SettingID = name;
            this.SettingType = settingType;
            this.ValueType = valueType;
        }
    }

    /// <summary>
    /// Исключение, возникающее при неверном сохранении или загрузке настроек из потока.
    /// </summary>
    class SettingsIOException : Exception
    {
        const string baseMessage = "Возникла ошибка сохранения/загрузки настроек в поток.";

        public SettingsIOException():
            base(baseMessage) { }

        public SettingsIOException(string message):
            base(message) { }
    }
    /// <summary>
    /// Данный класс описывает взаимодействие с настройками игры, их сохранение и загрузку, и т.д.
    /// Для взаимодействия с настройками клиент определяет тип настройки.
    /// </summary>
    [Serializable]
    class SettingsCore
    {
        /// <summary>
        /// Список настроек.
        /// </summary>
        Dictionary<SettingID, object> Settings
        {
            get;
            set;
        }

        public SettingsCore()
        {
            this.Settings = new Dictionary<SettingID, object>();
        }
        /// <summary>
        /// Сбрасывает значение настроек на значения по-умолчанию.
        /// </summary>
        public void ResetSettings()
        {
            foreach (var id in this.Settings.Keys)
            {
                Setting setting = (Setting)this.Settings[id];
                setting.ResetValue();
            }
        }
        /// <summary>
        /// Сохраняет все настройки.
        /// </summary>
        public void SaveSettings(Stream stream) 
        {
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(stream, this.Settings);
            }
            catch
            {
                throw new SettingsIOException();
            }
        }

        /// <summary>
        /// Загружает все настройки.
        /// </summary>
        public void LoadSettings(Stream stream) 
        {
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                this.Settings = (Dictionary<SettingID, object>)formatter.Deserialize(stream);
            }
            catch
            {
                throw new SettingsIOException();
            }
        }
        /// <summary>
        /// Добавляет новую настройку к менеджеру.
        /// </summary>
        /// <param name="setting"></param>
        public void AddSetting(Setting setting)
        {            
            this.Settings.Add(setting.Name, setting);
        }

        /// <summary>
        /// Создает настройку для данного менеджера.
        /// </summary>
        /// <typeparam name="T">Тип значения настройки.</typeparam>
        /// <param name="settingName">Идентификатор настройки.</param>
        /// <param name="settingValue">Значение настройки.</param>
        public Setting CreateSettingForManager(SettingID settingName, Object settingValue)
        {
            Setting result = new Setting(settingName, settingValue);
            this.AddSetting(result);

            return result;
        }

        /// <summary>
        /// Создает настройку для данного менеджера.
        /// </summary>
        /// <typeparam name="T">Тип значения настройки.</typeparam>
        /// <param name="settingName">Идентификатор настройки.</param>
        /// <param name="settingValue">Значение настройки.</param>
        public Setting CreateSettingForManager(string settingName, Object settingValue)
        {
            Setting result = new Setting(settingName, settingValue);
            this.AddSetting(result);

            return result;
        }

        /// <summary>
        /// Получает настройку по ее идентификатору. 
        /// </summary>
        /// <param name="name">Идентификатор настройки.</param>
        /// <returns>Объект настройки.</returns>
        public Setting GetSetting(SettingID name)
        {       
            if (this.ContainsID(name))
            {
                return this.GetSettingByID(name);
            }
            else
            {
                throw new SettingNotFoundException(name);
            }
        }

        private bool ContainsID(SettingID name)
        {
            bool found = false;
            foreach (var settingID in this.Settings.Keys)
            {
                if (settingID.Value.Equals(name.Value))
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        private Setting GetSettingByID(SettingID name)
        {
            foreach (var settingID in this.Settings.Keys)
            {
                if (settingID.Value.Equals(name.Value))
                {
                    return (Setting)this.Settings[settingID];
                }
            }

            throw new SettingNotFoundException(name);
        }
        
        // NOTE : Нарушение концепции инкапсуляции.
        ///// <summary>
        ///// Получает список всех настроек игры.
        ///// </summary>
        ///// <returns>Список настроек игры.</returns>
        //public IEnumerable<Setting<T>> GetSettings()
        //{
        //    return this.Settings.Values;
        //}

        /// <summary>
        /// Устанавливает настройку с заданным именем в заданное значение.
        /// </summary>
        /// <param name="name">Имя настройки.</param>
        /// <param name="value">Значение настройки.</param>
        /// <exception cref="SettingValueTypeMismatchException">Исключение,
        /// возникающее при попытке записать значение неверного типа в настройку.</exception>
        public void SetSetting(SettingID settingID, Object value) 
        {
            Setting setting = GetSetting(settingID);
            setting.SetValue(value);
        }
        
        /// <summary>
        /// Удаляет настройку по ее имени.
        /// </summary>
        /// <param name="name"></param>
        public void DeleteSetting<T>(SettingID name)
        {
            Setting removeableSetting = GetSetting(name);
            this.Settings.Remove(removeableSetting.Name);
        }
    }
}
