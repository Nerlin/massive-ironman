using System;
using System.Collections.Generic;

namespace SmplDotNet.Realization
{
    public class Queue : IQueue
    {
        protected Queue<ITransaction> events;

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

        private int left_;

        /// <summary>
        /// Возвращает количество элементов, которые покинули очередь.
        /// </summary>
        public int Left
        {
            get { return left_; }
        }

        private int totalAmount_;

        /// <summary>
        /// Возвращает количество поступивших транзактов в очередь.
        /// </summary>
        public int TotalAmount
        {
            get { return totalAmount_; }
        }

        private int lastDequeue_;

        /// <summary>
        /// Возвращает или задает время последнего освобождения очереди.
        /// </summary>
        public int LastDequeue
        {
            get
            {
                return lastDequeue_;
            }
            set
            {
                lastDequeue_ = value;
            }
        }

        /// <summary>
        /// Возвращает или задает текущее моделирование.
        /// </summary>
        public IModeling CurrentModeling
        {
            get;
            set;
        }


        public Queue(IModeling modeling)
        {
            events = new Queue<ITransaction>();
            
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
                events.Enqueue(transaction);
                totalAmount_++;
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
            lastDequeue_ = this.CurrentModeling.Time;
            left_++;

            return events.Dequeue();
        }

        /// <summary>
        /// Возвращает первый элемент из очереди, не вынимая его из очереди.
        /// </summary>
        /// <returns></returns>
        public ITransaction Peek()
        {
            return events.Peek();
        }

        /// <summary>
        /// Восстанавливает базовые значения очереди.
        /// </summary>
        public void Reset()
        {
            left_ = 0;
            totalAmount_ = 0;
        }

        /// <summary>
        /// Очищает очередь от элементов.
        /// </summary>
        public void Clear()
        {
            events.Clear();
        }
    }
}
