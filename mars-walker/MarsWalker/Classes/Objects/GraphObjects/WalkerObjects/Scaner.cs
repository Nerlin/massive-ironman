using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tao.OpenGl;
using MarsRover.Classes.Core;

namespace MarsRover.Classes.Objects.GraphObjects.WalkerObjects
{
    public class Scaner: Radar
    {
        public Scaner(): base()
        {
            frontTexture = TextureNames.Sensor;
        }
        protected override void PrepareAnimation(string name)
        {
            switch (name)
            {
                case BaseAnimation: { break; }
                case ScanAnimation: { normalAngle = this.Angle.Z; break; }
                case StopScaningAnimation:
                    {
                        double diff = normalAngle - this.Angle.Z;
                        direction = diff / Math.Abs(diff); break;
                    }
            }
        }
        protected override void PlayScan()
        {
            double enlarger = turningRight ? 1 : -1;

            this.Angle.Z += enlarger;
            bool flag = false;

            if (turningRight)
                flag = this.Angle.Z >= normalAngle + 45;
            else
                flag = this.Angle.Z <= normalAngle - 45;

            if (flag)
                turningRight = !turningRight;
        }
        protected override void PlayStopScan()
        {
            if (normalAngle != this.Angle.Z)
                this.Angle.Z += direction;
            else
            {
                this.SetAnimation(BaseAnimation);
                this.Enabled = false;
            }
        }

        public override void Draw()
        {        
           base.Draw();
        }
        
    }
}
