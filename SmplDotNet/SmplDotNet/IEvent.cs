namespace SmplDotNet
{
    public interface IEvent
    {
        /// <summary>
        /// Возвращает или задает номер события.
        /// </summary>
        int Number { get; set; }

        /// <summary>
        /// Возвращает или задает время начала события.
        /// </summary>
        int StartsAt { get; set; }

        /// <summary>
        /// Возвращает или задает время, через которое возникнет событие.
        /// </summary>
        int StartsSince { get; set; }

        /// <summary>
        /// Возвращает или задает время, когда событие завершилось.
        /// </summary>
        int FinishedAt { get; set; }

        /// <summary>
        /// Возвращает или задает транзакцию, за которой зарезервировано событие.
        /// </summary>
        ITransaction Transaction { get; set; }
    }
}
