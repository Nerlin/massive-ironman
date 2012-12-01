using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    /// <summary>
    /// Множество определяет уровни защиты системы от fork-угроз.
    /// </summary>
    public enum ProtectionType
    {
        /// <summary>
        /// Неверное значение.
        /// </summary>
        Invalid,
        /// <summary>
        /// Обычный уровень защиты.
        /// </summary>
        Simple,
        /// <summary>
        /// Критический уровень защиты.
        /// </summary>
        Critical
    }
}
