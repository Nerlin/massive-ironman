using System;
using System.Collections.Generic;
using System.Linq;

namespace SmplDotNet.Realization
{
    [Serializable]
    public class Queue : IQueue
    {
        private readonly List<Memory> memories;
        private readonly Queue<Memory> events;

        /// <summary>
        /// Возвращает или задает название очереди.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает вместимость очереди.
        /// </summary>
        public int Capacity
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает количество элементов в очереди.
        /// </summary>
        public int Count
        {
            get
            {
                return events.Count;
            }
        }

        /// <summary>
        /// Возвращает количество элементов, которые покинули очередь.
        /// </summary>
        public int Left { get; private set; }

        /// <summary>
        /// Возвращает количество поступивших транзактов в очередь.
        /// </summary>
        public int TotalAmount { get; private set; }

        /// <summary>
        /// Возвращает или задает время последнего освобождения очереди.
        /// </summary>
        public int LastDequeue { get; set; }

        /// <summary>
        /// Возвращает среднее время пребывания транзакта в очереди.
        /// </summary>
        public int AverageTime
        {
            get
            {
                return (int) memories.Average(memory => memory.EndTime - memory.StartTime);
            }
        }

        /// <summary>
        /// Возвращает среднее количество элементов в очереди.
        /// </summary>
        public int AverageAmount 
        {
            get 
            { 
                return (int) memories.Average(memory => memory.Elements);
            }
        }

        /// <summary>
        /// Возвращает или задает текущее моделирование.
        /// </summary>
        public IModeling CurrentModeling { get; set; }


        public Queue(IModeling modeling)
        {
            this.events = new Queue<Memory>();
            this.memories = new List<Memory>();

            this.CurrentModeling = modeling;
            this.Reset();
        }

        /// <summary>
        /// Вносит транзакт в очередь.
        /// </summary>
        /// <param name="transaction">Транзакция.</param>
        public void Enqueue(ITransaction transaction)
        {
            if (this.Capacity == 0 || this.Count < this.Capacity)
            {
                var memory = new Memory
                                 {
                                     Elements = this.TotalAmount,
                                     StartTime = this.CurrentModeling.Time,
                                     Transaction = transaction
                                 };

                this.events.Enqueue(memory);
                this.memories.Add(memory);
                this.TotalAmount++;
            }
            else
            {
                throw new InvalidOperationException("Вместимость очереди превышена.");
            }
        }

        /// <summary>
        /// Возвращает элемент из очереди.
        /// </summary>
        /// <returns></returns>
        public ITransaction Dequeue()
        {
            this.LastDequeue = this.CurrentModeling.Time;
            this.Left++;

            var memory = this.events.Dequeue();
            memory.EndTime = this.CurrentModeling.Time;

            return memory.Transaction;
        }

        /// <summary>
        /// Возвращает первый элемент из очереди, не вынимая его из очереди.
        /// </summary>
        /// <returns></returns>
        public ITransaction Peek()
        {
            return this.events.Peek().Transaction;
        }

        /// <summary>
        /// Восстанавливает базовые значения очереди.
        /// </summary>
        public void Reset()
        {
            this.Left = 0;
            this.TotalAmount = 0;
            this.LastDequeue = 0;

            this.events.Clear();
            this.memories.Clear();
        }

        /// <summary>
        /// Очищает очередь от элементов.
        /// </summary>
        public void Clear()
        {
            this.events.Clear();
        }
    }
}
