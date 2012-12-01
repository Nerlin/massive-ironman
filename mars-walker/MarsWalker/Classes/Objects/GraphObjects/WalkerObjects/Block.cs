using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MarsRover.Classes.Objects.WalkerObjects;
using MarsRover.Classes.Core;
using MarsRover.Classes.Core.Data;

using Tao;
using Tao.OpenGl;

namespace MarsRover.Classes.Objects.WalkerObjects
{
    public class Block: GraphObject
    {
        private GraphObject object1;
        private GraphObject object2;

        public GraphObject Object1 { get { return object1; } set { object1 = value; } }
        public GraphObject Object2 { get { return object2; } set { object2 = value; } }

        public new Point3d Position { get { return object1.Position; } }

        public double Width { get; set; }
        public double Depth { get; set; }

        public Block(GraphObject obj1, GraphObject obj2)
            : base()
        {
            object1 = obj1;
            object2 = obj2;
            this.Width = this.Depth = 0.25;
        }

        public override void Draw()
        {
            DrawingService.DrawRim(object1.Position, object2.Position, 0.25, 0.25);
        }
    }
}
