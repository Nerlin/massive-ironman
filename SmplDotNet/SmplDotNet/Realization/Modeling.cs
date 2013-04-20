using System;
using System.Collections.Generic;
using System.Linq;

namespace SmplDotNet.Realization
{
    public class Modeling : IModeling
    {
        protected List<IEvent> Events; 

        /// <summary>
        /// Возвращает или задает время моделирования.
        /// </summary>
        public int Time
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает количество итераций для моделирования.
        /// </summary>
        public int EndsAt
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или задает список устройств.
        /// </summary>
        public IList<IDevice> Devices
        {
            get;
            set;
        }

        /// <summary>
        /// Освобождает устройства, которые должны быть освобождены в данное время моделирования.
        /// </summary>
        public void ReleaseDevices()
        {
            foreach (var device in this.Devices.Where(device => this.Time >= device.LastReservationTime + device.HandlingTime))
            {
                device.Release();
            }
        }

        /// <summary>
        /// Событие, происходящее при выполнении запланированного события.
        /// </summary>
        public event ModelingCauseEventHandler OnCause;

        public Modeling()
        {
            this.Devices = new List<IDevice>();
            this.Events = new List<IEvent>();
        }

        /// <summary>
        /// Запускает симуляцию имитационной модели.
        /// </summary>
        public void Run()
        {
            do
            {
                this.Cause();

            } while (this.Time <= this.EndsAt);
        }

        /// <summary>
        /// Планирует событие для моделирования с заданным транзактом и 
        /// заданным номером события через указанное время.
        /// </summary>
        public void Schedule(IEvent scheduledEvent)
        {
            scheduledEvent.StartsAt = scheduledEvent.StartsSince + this.Time;

            this.Events.Add(scheduledEvent);
            this.Events.Sort((item1, item2) => item1.StartsAt.CompareTo(item2.StartsAt));
        }

        protected bool IsEventCurrentlyScheduled()
        {
            return this.Events.Any();
        }

        /// <summary>
        /// Удаляет событие, которое должно выполниться в текущий промежуток времени и
        /// устанавливает время моделирования на время, когда должно было произойти событие.
        /// </summary>
        /// <exception cref="InvalidOperationException">Выбрасывается, если новых событий не запланировано.</exception>
        /// <returns></returns>
        public IEvent Cause()
        {
            if (this.IsEventCurrentlyScheduled())
            {
                var @event = this.Events.First();
                this.Events.Remove(@event);

                this.Time = @event.StartsAt;
                if (this.OnCause != null) {
                    this.OnCause(this, new ModelingCauseEventArgs(@event));
                }


                return @event;
            }
            throw new InvalidOperationException("Новых событий не запланировано.");
        }

        /// <summary>
        /// Отменяет ближайшее по времени указанное событие и возвращает
        /// разность между временем, когда событие должно было произойти, и текущим временем моделирования.
        /// </summary>
        /// <param name="scheduledEvent"></param>
        /// <returns></returns>
        public int Cancel(IEvent scheduledEvent)
        {
            this.Events.Remove(scheduledEvent);
            return scheduledEvent.StartsAt - this.Time;
        }
        
    }
}
