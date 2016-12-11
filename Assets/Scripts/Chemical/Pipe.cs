using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Chemical
{
    class Pipe
    {
        protected Mixture mixture;

        protected Vessel entrance;
        protected Vessel exit;
        public float diameter { get; protected set; }
        public float area { get { return diameter * diameter * (float)Math.PI / 4; } }
        public float length { get; protected set; }
        public float flowSpeed { get; protected set; }
        public float Temperature { get { return mixture.temperature; } }
        public float InternalMass { get { return mixture.Density * area * length; } }
        public float MassFlow { get { return mixture.Density * area * flowSpeed; } }

        public Pipe(Vessel entrance, Vessel exit, float length, float diameter)
        {
            this.entrance = entrance; this.exit = exit; this.length = length; this.diameter = diameter;
            flowSpeed = 0;
            mixture = new Mixture();
        }

        public virtual void Update(float dT)
        {
            float Dp = (entrance.Pressure - exit.Pressure) * 10000; // in Pa
            float rho = mixture.Density;
            if (diameter * diameter / 32 < 2 * dT)    // fast system, inmediatly go to steady state
            {
                flowSpeed = Dp * diameter * diameter / (32 * rho * length);
            }
            else
            {
                flowSpeed += (Dp / rho / length - 32 * flowSpeed / diameter / diameter) * dT;
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

        internal void AddHeat(float heat)
        {
            float heatCap = InternalMass;
            float dTemp = heat / heatCap;
            mixture.temperature += dTemp;
        }
    }
}
