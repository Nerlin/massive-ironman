using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Configs
{
    /// <summary>
    /// Класс настройки игры.
    /// </summary>
    /// Анонимные типы не используются для того, чтобы упростить доступ к настройкам.
    [Serializable]
    public class Setting
    {
        /// <summary>
        /// Название настройки.
        /// </summary>
        public SettingID Name
        {
            get;
            set;
        }

        /// <summary>
        /// Значение настройки.
        /// </summary>
        private Object Value
        {
            get;
            set;
        }

        /// <summary>
        /// Значение настройки по-умолчанию.
        /// </summary>
        private Object DefaultValue
        {
            get;
            set;
        }

        
        public Setting(string name, object value)
        {
            this.Name = new SettingID(name);
            this.Value = value;
            this.DefaultValue = value;
        }
        public Setting(SettingID id, object value)
        {
            this.Name = id;
            this.Value = value;
            this.DefaultValue = value;
        }

        public object GetDefaultValue()
        {
            return this.DefaultValue;
        }

        public object GetValue()
        {
            return this.Value;
        }

        public T GetValue<T>()
        {
            return (T)this.Value;
        }

        public void SetValue(Object value)
        {
            Type valueType = value.GetType();
            Type settingType = this.Value.GetType();

            if (TypesIsMatch(valueType, settingType))
            {
                this.Value = value;
            }
            else
            {
                throw new SettingValueTypeMismatchException(this.Name, settingType, valueType);
            }
        }

        public void SetValue<T>(T value)
        {
            Type valueType = value.GetType();
            Type settingType = this.Value.GetType();

            if (TypesIsMatch(valueType, settingType))
            {
                this.Value = value;
            }
        }

        private static bool TypesIsMatch(Type valueType, Type settingType)
        {
            return valueType.Equals(settingType);
        }

        public void ResetValue()
        {
            this.Value = this.DefaultValue;
        }

        public override string ToString()
        {
            return string.Format("Setting: {0} : {1}", this.Name, this.Value);
        }

    }
}
