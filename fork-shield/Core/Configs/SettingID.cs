using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Configs
{
    /// <summary>
    /// Идентификатор настройки.
    /// </summary>
    /// 
    [Serializable]
    public class SettingID
    {
        /// <summary>
        /// Значение идентификатор.
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        public SettingID(string value)
        {
            this.Value = value;
        }
    }
}
