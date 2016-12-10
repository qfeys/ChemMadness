using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Chemical
{
    class Pipe
    {
        Mixture mixture;

        Vessel entrance;
        Vessel exit;
        float diameter;
        float area { get { return diameter * diameter * (float)Math.PI / 4; } }
        float length;
        float flowSpeed;

        public Pipe(Vessel entrance, Vessel exit, float length, float diameter)
        {
            this.entrance = entrance; this.exit = exit; this.length = length; this.diameter = diameter;
            flowSpeed = 0;
            mixture = new Mixture();
        }

        public void Update(float dT)
        {
            float Dp = (entrance.Pressure - exit.Pressure) * 10000; // in Pa
            float rho = mixture.Density;
            flowSpeed += (Dp / rho / length - 32* flowSpeed / diameter/diameter) * dT;
            float internalMass = rho * area * length;
            float movedMass = rho * length * flowSpeed;
            if (flowSpeed > 0)
            {
                exit.AddMixture(mixture, movedMass);
                Mixture newMix = entrance.RemoveMixture(movedMass);
                mixture.Add(newMix, movedMass / (internalMass - movedMass));
            }
            else if (flowSpeed < 0)
            {
                entrance.AddMixture(mixture, movedMass);
                Mixture newMix = exit.RemoveMixture(movedMass);
                mixture.Add(newMix, movedMass / (internalMass - movedMass));
            }
            UnityEngine.Debug.Log(flowSpeed + " : " + movedMass);
        }
    }
}
