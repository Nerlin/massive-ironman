using System;
using System.Collections.Generic;
using System.Linq;

namespace SmplDotNet.Realization
{
    class MultichannelDevice : IDevice
    {
        private readonly List<Device> devices;

        // @todo: Aggregation to all devices
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
        public ITransaction CurrentTransaction { get; private set; }

        /// <summary>
        /// Возвращает или задает время обработки транзакта данным устройством.
        /// </summary>
        public int HandlingTime { get; set; }

        /// <summary>
        /// Возвращает время, когда транзакт будет обработан.
        /// </summary>
        public int ReleaseTime { get; private set; }

        /// <summary>
        /// Возвращает время, когда в последний раз было зарезервировано устройство.
        /// </summary>
        public int LastReservationTime { get; private set; }

        /// <summary>
        /// Возвращает количество обработанных транзакций.
        /// </summary>
        public int HandledTransactionsCount { get; private set; }

        /// <summary>
        /// Возвращает или задает очередь для данного устройства.
        /// </summary>
        public IQueue Queue { get; set; }

        /// <summary>
        /// Возвращает или задает объект моделирования, в котором используется данное устройство.
        /// </summary>
        public IModeling Modeling { get; set; }

        /// <summary>
        /// Возвращает или задает количество каналов в устройстве.
        /// </summary>
        public int Channels
        {
            get { return Devices.Count; }
            set
            {
                Devices.Clear();
                for (var i = 0; i < value; i++)
                    Devices.Add(new Device(this.Modeling) { Queue = this.Queue });
            }
        }

        public List<Device> Devices
        {
            get { return devices; }
        }


        public MultichannelDevice(IModeling modeling)
        {
            this.devices = new List<Device>();

            this.Modeling = modeling;
        }

        /// <summary>
        /// Резервирует указанное устройство за заданным транзактом.
        /// </summary>
        /// <param name="transaction"></param>
        public void Reserve(ITransaction transaction)
        {
            this.Queue.Enqueue(transaction);

            // @todo : Sync Transaction through devices
        }

        /// <summary>
        /// Освобождает данное устройство.
        /// </summary>
        public void Release()
        {
            foreach (var device in Devices)
            {
                device.Release();
            }
        }

        // @todo: Refactor :: Extract Interface ?
        public void Release(IDevice device)
        {
            foreach (var item in Devices.Where(item => item == device))
            {
                item.Release();
            }
        }

        /// <summary>
        /// Сбрасывает статистику устройства.
        /// </summary>
        public void Reset()
        {
            foreach (var device in Devices)
            {
                device.Reset();
            }
        }

        /// <summary>
        /// Событие, происходящее когда заявка приходит на обслуживание.
        /// </summary>
        public event EventHandler OnStartHandling;

        /// <summary>
        /// Событие, происходящее когда заявка прошла обслуживание.
        /// </summary>
        public event EventHandler OnRelease;
    }
}
