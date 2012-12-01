using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MarsRover.Classes.Core
{
    public class Texture
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Размер текстуры.
        /// </summary>
        public Size Size { get; set; }
        /// <summary>
        /// Изображение.
        /// </summary>
        public Byte[] Image { get; set; }

        /// Thoughts:
        /// Сделать коллекцию массивов изображений текстуры.
    }
}
