using System;
using SmplDotNet;
using SmplDotNet.Realization;

namespace SmplSample
{
    class Program
    {
        static IDevice mainDevice;

        static void Main(string[] args)
        {
            IModeling modeling = new Modeling();

            mainDevice = new Device(modeling);
            mainDevice.HandlingTime = 10;
            mainDevice.Queue = new Queue(modeling);
            mainDevice.OnStartHandling += mainDevice_OnStartHandling;

            modeling.Devices.Add(mainDevice);
            modeling.EndsAt = 100;
            modeling.OnCause += modeling_OnCause;

            modeling.Schedule(new Event { Transaction = new Transaction { Status = SampleTransactionStatus.Arriving }});
            modeling.Run();

            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("Время моделирования: {0}", modeling.EndsAt);
            Console.WriteLine("Обработано заявок:   {0}", mainDevice.HandledTransactionsCount);
            Console.WriteLine("Поступило заявок:    {0}", mainDevice.Queue.TotalAmount);
            Console.Read();
        }


        static void modeling_OnCause(object sender, ModelingCauseEventArgs e)
        {
            var modeling = (IModeling)sender;
            switch ((SampleTransactionStatus)e.Event.Transaction.Status)
            {
                case SampleTransactionStatus.Arriving: 
                    modeling_OnCause_Arriving(modeling, e.Event);
                    break;
                case SampleTransactionStatus.DeviceAdvanced:
                    modeling_OnCause_DeviceAdvanced(modeling, e.Event);
                    break;
            }

            mainDevice.Reserve(e.Event.Transaction);

        }

        static void modeling_OnCause_Arriving(IModeling modeling, IEvent @event)
        {
            modeling.Schedule(new Event { StartsSince = 7, Transaction = new Transaction { Status = SampleTransactionStatus.Arriving } });

            mainDevice.Reserve(@event.Transaction);
            modeling.Schedule(new Event { 
                StartsSince = mainDevice.HandlingTime,
                Transaction = new Transaction
                                  {
                                      Status = SampleTransactionStatus.DeviceAdvanced
                                  }
            });

            Console.WriteLine("[{0,5}]: Заявка встала в очередь на обработку.", modeling.Time);
        }

        static void modeling_OnCause_DeviceAdvanced(IModeling modeling, IEvent @event)
        {
            Console.WriteLine("[{0,5}]: Заявка обработана устройством.", modeling.Time);

            mainDevice.Release();
        }

        static void mainDevice_OnStartHandling(object sender, EventArgs e)
        {
            Console.WriteLine("[{0,5}]: Заявка начала обрабатываться.", ((IDevice)sender).Modeling.Time);
        }

    }
}
