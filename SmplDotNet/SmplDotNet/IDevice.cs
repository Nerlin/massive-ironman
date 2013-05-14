using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmplDotNet
{
    public interface IDevice
    {
        /// <summary>
        /// Возвращает или задает номер устройства.
        /// </summary>
        int Number { get; set; }

        /// <summary>
        /// Возвращает или задает название устройства.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Возвращает статус данного устройства.
        /// </summary>
        bool Reserved { get; }

        /// <summary>
        /// Возвращает текущую транзакцию, которая находится на обработке в данном устройстве.
        /// </summary>
        ITransaction CurrentTransaction { get; }

        /// <summary>
        /// Возвращает или задает время обработки транзакта данным устройством.
        /// </summary>
        int HandlingTime { get; set; } 

        /// <summary>
        /// Возвращает время, когда транзакт будет обработан.
        /// </summary>
        int ReleaseTime { get; }

        /// <summary>
        /// Возвращает время, когда в последний раз было зарезервировано устройство.
        /// </summary>
        int LastReservationTime { get; }

        /// <summary>
        /// Возвращает количество полученных на обработку заявок.
        /// </summary>
        int ReceivedTransactionsCount { get; }
    
        /// <summary>
        /// Возвращает количество обработанных транзакций.
        /// </summary>
        int HandledTransactionsCount { get; }

        /// <summary>
        /// Возвращает эффективное время работы устройства.
        /// </summary>
        int EffecientTime { get; }

        /// <summary>
        /// Возвращает среднее время обработки транзакта в устройстве.
        /// </summary>
        int AverageHandlingTime { get; }

        /// <summary>
        /// Возвращает или задает очередь для данного устройства.
        /// </summary>
        IQueue Queue { get; set; }

        /// <summary>
        /// Возвращает или задает объект моделирования, в котором используется данное устройство.
        /// </summary>
        IModeling Modeling { get; set; }


        /// <summary>
        /// Резервирует указанное устройство за заданным транзактом.
        /// </summary>
        /// <param name="transaction"></param>
        void Reserve(ITransaction transaction);

        /// <summary>
        /// Освобождает данное устройство.
        /// </summary>
        void Release();

        /// <summary>
        /// Сбрасывает статистику устройства.
        /// </summary>
        void Reset();

        /// <summary>
        /// Событие, происходящее когда заявка приходит на обслуживание.
        /// </summary>
        event EventHandler OnStartHandling;

        /// <summary>
        /// Событие, происходящее когда заявка прошла обслуживание.
        /// </summary>
        event EventHandler OnRelease;
    }
}
