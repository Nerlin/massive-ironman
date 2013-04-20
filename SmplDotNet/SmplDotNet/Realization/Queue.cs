using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmplDotNet
{
    public class Queue : IQueue
    {
        protected Queue<ITransaction> events;

        public string Name
        {
            get;
            set;
        }

        public int Capacity
        {
            get;
            set;
        }

        public int Count
        {
            get
            {
                return events.Count;
            }
        }

        private int left_;
        public int Left
        {
            get { return left_; }
        }

        private int totalAmount_;
        public int TotalAmount
        {
            get { return totalAmount_; }
        }

        private int lastDequeue_;
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

        public ITransaction Dequeue()
        {
            lastDequeue_ = this.CurrentModeling.Time;
            left_++;

            return events.Dequeue();
        }

        public ITransaction Peek()
        {
            return events.Peek();
        }

        public void Reset()
        {
            left_ = 0;
            totalAmount_ = 0;
        }

        public void Clear()
        {
            events.Clear();
        }
    }
}
