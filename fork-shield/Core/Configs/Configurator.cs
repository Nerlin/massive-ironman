using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace Core
{
    static class Configurator
    {
        /// <summary>
        /// Загружает конфигурируемые объекты из потока.
        /// </summary>
        /// <param name="stream">Бинарный поток.</param>
        /// <returns>Конфигурируемый объект из потока.</returns>
        public static ISerializable LoadObjects(Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            ISerializable result = (ISerializable)formatter.Deserialize(stream);

            return result;
        }

        /// <summary>
        /// Сохраняет конфигурируемый объект в поток.
        /// </summary>
        /// <param name="configurableObjects">Конфигурируемый объект. Должен поддерживать возможность сериализации.</param>
        /// <param name="stream">Поток, в который будет сохранен конфигурируемый объект.</param>
        public static void SaveObjects(ISerializable configurableObjects, Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, configurableObjects);
        }
    }
}
