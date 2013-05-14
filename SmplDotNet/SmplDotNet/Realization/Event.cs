using System;

namespace SmplDotNet.Realization
{
    [Serializable]
    public class Event : IEvent
    {
        /// <summary>
        /// Возвращает или задает номер события.
        /// </summary>
        public int Number
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает время начала события.
        /// </summary>
        public int StartsAt
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает время, через которое возникнет событие.
        /// </summary>
        public int StartsSince
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает время, когда событие завершилось.
        /// </summary>
        public int FinishedAt
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает транзакцию, за которой зарезервировано событие.
        /// </summary>
        public ITransaction Transaction
        {
            get;
            set;
        }

        public Event()
        {
            this.Transaction = new Transaction();
        }

        public Event(ITransaction transaction)
        {
            this.Transaction = transaction;
        }
    }
}
