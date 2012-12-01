using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarsRover.Interfaces
{
    interface IAnimated
    {
        /// <summary>
        /// Выбранная анимация.
        /// </summary>
        string SelectedAnimation { get; set; }
        /// <summary>
        /// Устанавливает анимацию у данного объекта.
        /// </summary>
        /// <param name="name">Название анимации.</param>
        void SetAnimation(string name);
        /// <summary>
        /// Осуществляет сброс анимации на базовую.
        /// </summary>
        void Reset();
        void Update();
    }
}
