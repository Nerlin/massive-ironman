using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmplDotNet
{
    public interface IQueue
    {
        /// <summary>
        /// Возвращает или задает название очереди.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Возвращает или задает вместимость очереди.
        /// </summary>
        int Capacity { get; set; }

        /// <summary>
        /// Возвращает количество элементов в очереди.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Возвращает количество элементов, которые покинули очередь.
        /// </summary>
        int Left { get; }

        /// <summary>
        /// Возвращает количество поступивших транзактов в очередь.
        /// </summary>
        int TotalAmount { get; }

        /// <summary>
        /// Возвращает или задает время последнего освобождения очереди.
        /// </summary>
        int LastDequeue { get; set; }

        /// <summary>
        /// Возвращает или задает текущее моделирование.
        /// </summary>
        IModeling CurrentModeling { get; set; }

        /// <summary>
        /// Вносит транзакт в очередь.
        /// </summary>
        /// <param name="transaction">Транзакция.</param>
        void Enqueue(ITransaction transaction);

        /// <summary>
        /// Возвращает элемент из очереди.
        /// </summary>
        /// <returns></returns>
        ITransaction Dequeue();

        /// <summary>
        /// Возвращает первый элемент из очереди, не вынимая его из очереди.
        /// </summary>
        /// <returns></returns>
        ITransaction Peek();

        /// <summary>
        /// Восстанавливает базовые значения очереди.
        /// </summary>
        void Reset();

        /// <summary>
        /// Очищает очередь от элементов.
        /// </summary>
        void Clear();
    }
}
