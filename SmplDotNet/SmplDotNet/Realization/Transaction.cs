using System;

namespace SmplDotNet.Realization
{
    [Serializable]
    public class Transaction: ITransaction
    {
        /// <summary>
        /// Возвращает или задает номер транзакции.
        /// </summary>
        public int Number
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает приоритет транзакции.
        /// </summary>
        public int Priority
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает статус транзакта.
        /// Следует использовать для определения типа заявки.
        /// </summary>
        public object Status { get; set; }

        /// <summary>
        /// Возвращает или задает время появления транзакта.
        /// </summary>
        public int StartedAt { get; set; }

        /// <summary>
        /// Возвращает или задает длительность обработки данного транзакта.
        /// Считается без времени пробывания транзакта в очередях.
        /// </summary>
        public int Duration { get; set; }
    }
}
