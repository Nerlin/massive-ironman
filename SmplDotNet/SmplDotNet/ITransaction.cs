namespace SmplDotNet
{
    public interface ITransaction
    {
        /// <summary>
        /// Возвращает или задает номер транзакции.
        /// </summary>
        int Number { get; set; }

        /// <summary>
        /// Возвращает или задает приоритет транзакции.
        /// </summary>
        int Priority { get; set; }

        /// <summary>
        /// Возвращает или задает статус транзакта.
        /// Следует использовать для определения типа заявки.
        /// </summary>
        object Status { get; set; }

    }
}
