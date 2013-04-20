namespace SmplDotNet.Realization
{
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
    }
}
