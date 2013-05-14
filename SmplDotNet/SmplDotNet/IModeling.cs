using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmplDotNet
{
    public interface IModeling
    {
        /// <summary>
        /// Возвращает или задает время моделирования.
        /// </summary>
        int Time { get; set; }

        /// <summary>
        /// Возвращает или задает количество итераций для моделирования.
        /// </summary>
        int EndsAt { get; set; }

        /// <summary>
        /// Возвращает или задает количество заявок, которые прошли весь этап моделирования.
        /// </summary>
        int Completed { get; set; }

        /// <summary>
        /// Возвращает или задает список устройств.
        /// </summary>
        IList<IDevice> Devices { get; set; }

        /// <summary>
        /// Запускает симуляцию имитационной модели.
        /// </summary>
        void Run();

        /// <summary>
        /// Планирует событие для моделирования с заданным транзактом и 
        /// заданным номером события через указанное время.
        /// </summary>
        void Schedule(IEvent scheduledEvent);

        /// <summary>
        /// Задает количество транзакций для моделирования.
        /// </summary>
        /// <param name="status">Статус транзакции, при котором ведется подсчет.</param>
        /// <param name="amount">Количество транзактов.</param>
        void SetTranscationProcessingAmount(object status, int amount);

        /// <summary>
        /// Удаляет событие, которое должно выполниться в текущий промежуток времени и
        /// устанавливает время моделирования на время, когда должно было произойти событие.
        /// </summary>
        /// <returns></returns>
        IEvent Cause();

        /// <summary>
        /// Отменяет ближайшее по времени указанное событие и возвращает
        /// разность между временем, когда событие должно было произойти, и текущим временем моделирования.
        /// </summary>
        /// <param name="scheduledEvent"></param>
        /// <returns></returns>
        int Cancel(IEvent scheduledEvent);

        /// <summary>
        /// Освобождает устройства, которые должны быть освобождены в данное время моделирования.
        /// </summary>
        void ReleaseDevices();

        /// <summary>
        /// Останавливает моделирование.
        /// </summary>
        void Break();

        /// <summary>
        /// Событие, происходящее при выполнении запланированного события.
        /// </summary>
        event ModelingCauseEventHandler OnCause;

        void Reset();
    }
}
