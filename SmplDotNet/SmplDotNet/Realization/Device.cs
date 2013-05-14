using System;
using System.Collections.Generic;
using System.Linq;

namespace SmplDotNet.Realization
{
    [Serializable]
    public class Device : IDevice
    {
        private readonly List<Memory> memories;
        private Memory currentMemory;

        /// <summary>
        /// Возвращает или задает номер устройства.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Возвращает или задает название устройства.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Возвращает статус данного устройства.
        /// </summary>
        public bool Reserved { get; private set; }

        /// <summary>
        /// Возвращает текущую транзакцию, которая находится на обработке в данном устройстве.
        /// </summary>
        public ITransaction CurrentTransaction
        {
            get
            {
                return currentMemory.Transaction;
            }
        }

        /// <summary>
        /// Возвращает или задает объект моделирования, в котором используется данное устройство.
        /// </summary>
        public IModeling Modeling { get; set; }

        /// <summary>
        /// Возвращает или задает время обработки транзакта данным устройством.
        /// </summary>
        public int HandlingTime { get; set; }

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
        public int LastReservationTime { get;set; }

        /// <summary>
        /// Возвращает количество полученных на обработку заявок.
        /// </summary>
        public int ReceivedTransactionsCount { get; private set; }

        /// <summary>
        /// Возвращает количество обработанных транзакций.
        /// </summary>
        public int HandledTransactionsCount { get; private set; }

        /// <summary>
        /// Возвращает эффективное время работы устройства.
        /// </summary>
        public int EffecientTime
        {
            get 
            {
                return this.Modeling.EndsAt - memories.Sum(memory => memory.EndTime - memory.StartTime);
            }
        }

        /// <summary>
        /// Возвращает среднее время обработки транзакта в устройстве.
        /// </summary>
        public int AverageHandlingTime
        {
            get { return (int) memories.Average(memory => memory.EndTime - memory.StartTime); }
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
            this.memories = new List<Memory>();
            this.currentMemory = new Memory();

            this.Modeling = modeling;
            this.Queue = new Queue(modeling);
            this.Reset();
            this.OnRelease += Device_OnRelease;
        }

        void Device_OnRelease(object sender, EventArgs e)
        {
            if (this.Queue.Count != 0)
                this.Reserve(this.Queue.Dequeue());
        }

        /// <summary>
        /// Резервирует указанное устройство за заданным транзактом и помещает событие об окончании обработки в модель.
        /// </summary>
        /// <param name="transaction"></param>
        public void Reserve(ITransaction transaction)
        {
            this.ReceivedTransactionsCount++;

            if (!this.Reserved)
            {
                if (transaction != null)
                {
                    this.currentMemory = new Memory {StartTime = this.Modeling.Time, Transaction = transaction};
                    this.Reserved = true;

                    this.LastReservationTime = this.Modeling.Time;
                    if (this.OnStartHandling != null)
                        this.OnStartHandling(this, new EventArgs());
                    
                    this.Modeling.Schedule(new Event { StartsSince = this.HandlingTime , Transaction = transaction});
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
            this.currentMemory.EndTime = this.Modeling.Time;
            this.memories.Add(this.currentMemory);

            this.HandledTransactionsCount++;
            this.Reserved = false;

            if (this.OnRelease != null)
                this.OnRelease(this, new EventArgs());
        }

        /// <summary>
        /// Сбрасывает статистику устройства.
        /// </summary>
        public void Reset()
        {
            this.ReceivedTransactionsCount = 0;
            this.HandledTransactionsCount = 0;
            this.Reserved = false;
            this.LastReservationTime = 0;

            this.memories.Clear();
            this.currentMemory = new Memory();

            this.Queue.Reset();
        }
    }
}
