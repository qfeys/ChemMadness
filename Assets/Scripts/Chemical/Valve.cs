using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Chemical
{
    class Valve : Pipe, Controlled
    {
        public float Control { get; set; }

        public Valve(Vessel entrance, Vessel exit, float length, float diameter) 
            : base(entrance, exit, length, diameter)
        {
        }

        public override void Update(float dT)
        {
            float Dp = (entrance.Pressure - exit.Pressure) * 10000; // in Pa
            float rho = mixture.Density;
            float dim = diameter * Control;
            if (diameter * diameter / 32 < 2 * dT)    // fast system, inmediatly go to steady state
            {
                flowSpeed = Dp * dim * dim / (32 * rho * length);
            }
            else
            {
                flowSpeed += (Dp / rho / length - 32 * flowSpeed / dim / dim) * dT;
            }
            if (flowSpeed > 500) flowSpeed = 500;
            float movedMass = rho * area * flowSpeed * dT;
            float movedVolume = area * flowSpeed * dT;
            if (flowSpeed > 0)
            {
                exit.AddMixture(mixture, movedMass);
                float mass;
                Mixture newMix = entrance.RemoveMixture(movedVolume, out mass);
                mixture.Add(newMix, mass / (InternalMass - movedMass));
            }
            else if (flowSpeed < 0)
            {
                entrance.AddMixture(mixture, movedMass);
                float mass;
                Mixture newMix = exit.RemoveMixture(movedVolume, out mass);
                mixture.Add(newMix, mass / (InternalMass - movedMass));
            }
        }
    }
}
