using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Chemical
{
    class Pump : Pipe, Controlled
    {

        float pumpPressure;

        public Pump(Vessel entrance, Vessel exit, float length, float diameter, float dP) : base(entrance, exit, length, diameter)
        {
            pumpPressure = dP;
        }


        public override void Update(float dT)
        {
            float Dp = (entrance.Pressure - exit.Pressure) * 10000 + pumpPressure; // in Pa
            float rho = mixture.Density;
            if (diameter * diameter / 32 < 2 * dT)    // fast system, inmediatly go to steady state
            {
                flowSpeed = Dp * diameter * diameter / (32 * rho * length);
            }
            else
            {
                flowSpeed += (Dp / rho / length - 32 * flowSpeed / diameter / diameter) * dT;
            }
            if (flowSpeed > 500) flowSpeed = 500;   // To stay sain
            float internalMass = rho * area * length;
            float movedMass = rho * area * flowSpeed * dT;
            float movedVolume = area * flowSpeed * dT;
            if (flowSpeed > 0)
            {
                exit.AddMixture(mixture, movedMass);
                float mass;
                Mixture newMix = entrance.RemoveMixture(movedVolume, out mass);
                mixture.Add(newMix, mass / (internalMass - movedMass));
            }
            else if (flowSpeed < 0)
            {
                entrance.AddMixture(mixture, movedMass);
                float mass;
                Mixture newMix = exit.RemoveMixture(movedVolume, out mass);
                mixture.Add(newMix, mass / (internalMass - movedMass));
            }
        }
    }
}
