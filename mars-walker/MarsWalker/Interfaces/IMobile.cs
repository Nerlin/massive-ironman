using System;
using MarsRover.Classes.Objects;

namespace MarsRover.Interfaces
{
    public interface IMobile
    {
        /// <summary>
        /// Скорость объекта.
        /// </summary>
        double Speed
        {
            get;
            set;
        }
        /// <summary>
        /// Максимальная скорость.
        /// </summary>
        double MaxSpeed
        {
            get;
            set;
        }

        void Move();
    }
}
