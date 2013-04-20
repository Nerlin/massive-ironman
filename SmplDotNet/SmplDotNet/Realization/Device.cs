using System;

namespace SmplDotNet.Realization
{
    public class Device : IDevice
    {
        /// <summary>
        /// Возвращает или задает номер устройства.
        /// </summary>
        public int Number
        { 
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает название устройства.
        /// </summary>
        public string Name 
        {
            get;
            set;
        }

        private bool reserved_;

        /// <summary>
        /// Возвращает статус данного устройства.
        /// </summary>
        public bool Reserved
        {
            get { return reserved_; }
        }

        private ITransaction transaction_;

        /// <summary>
        /// Возвращает текущую транзакцию, которая находится на обработке в данном устройстве.
        /// </summary>
        public ITransaction CurrentTransaction
        {
            get { return transaction_; }
        }

        /// <summary>
        /// Возвращает или задает объект моделирования, в котором используется данное устройство.
        /// </summary>
        public IModeling Modeling
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает время обработки транзакта данным устройством.
        /// </summary>
        public int HandlingTime
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает время, когда транзакт будет обработан.
        /// </summary>
        public int ReleaseTime 
        {
            get
            {
                return this.LastReservationTime + this.HandlingTime;
            }
        }


        /// <summary>
        /// Возвращает время, когда в последний раз было зарезервировано устройство.
        /// </summary>
        public int LastReservationTime
        {
            get;
            set;
        }

        private int handledTransactionsCount;

        /// <summary>
        /// Возвращает количество обработанных транзакций.
        /// </summary>
        public int HandledTransactionsCount
        {
            get { return handledTransactionsCount; }
        }

        /// <summary>
        /// Возвращает или задает очередь для данного устройства.
        /// </summary>
        public IQueue Queue { get; set; }

        /// <summary>
        /// Событие, происходящее когда заявка приходит на обслуживание.
        /// </summary>
        public event EventHandler OnStartHandling;

        /// <summary>
        /// Событие, происходящее когда заявка прошла обслуживание.
        /// </summary>
        public event EventHandler OnRelease;


        public Device(IModeling modeling)
        {
            this.Modeling = modeling;
            this.Reset();
            this.OnRelease += Device_OnRelease;
        }

        void Device_OnRelease(object sender, EventArgs e)
        {
            if (this.Queue.Count != 0)
                this.Reserve(this.Queue.Dequeue());
        }

        /// <summary>
        /// Резервирует указанное устройство за заданным транзактом.
        /// </summary>
        /// <param name="transaction"></param>
        public void Reserve(ITransaction transaction)
        {
            if (!this.Reserved)
            {
                if (transaction != null)
                {
                    transaction_ = transaction;
                    reserved_ = true;

                    this.LastReservationTime = this.Modeling.Time;
                    if (this.OnStartHandling != null)
                    {
                        this.OnStartHandling(this, new EventArgs());
                    }
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            else
            {
                if (this.Queue != null)
                    this.Queue.Enqueue(transaction);
                else
                    throw new InvalidOperationException("Устройство занято и не задана очередь, в которую может поступить транзакт.");
            }
        }

        /// <summary>
        /// Освобождает данное устройство.
        /// </summary>
        public void Release()
        {
            handledTransactionsCount++;
            transaction_ = null;
            reserved_ = false;

            if (this.OnRelease != null)
                this.OnRelease(this, new EventArgs());
        }

        /// <summary>
        /// Сбрасывает статистику устройства.
        /// </summary>
        public void Reset()
        {
            handledTransactionsCount = 0;
            transaction_ = null;
            reserved_ = false;
        }
    }
}
