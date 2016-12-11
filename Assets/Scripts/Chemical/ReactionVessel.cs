using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Chemical
{
    class ReactionVessel : Vessel
    {
        float Volume;
        float pressureLimit;
        Mixture mixture;
        public float mass { get; set; }    // The mass of product in the vessel
        public float Pressure { get { return mixture != null ? mixture.pressure : 0; } }
        public float Temperature { get { return mixture.temperature; } }
        float filled { get { return mass / mixture.Density; } } // between 0 and 1

        public ReactionVessel(float Volume, float pressureLimit)
        {
            this.Volume = Volume; this.pressureLimit = pressureLimit;
            mixture = new Mixture();
        }

        public void AddMixture(Mixture mix, float mass)
        {
            if (mixture == null)
            {
                mixture = mix;
            }
            else
            {
                mixture.Add(mix, mass / this.mass);
            }
            this.mass += mass;
        }

        public Mixture RemoveMixture(float volume, out float mass)
        {
            mass = volume * mixture.Density;
            this.mass -= mass;
            return mixture;
        }

        private void RecalculatePressure()
        {
            float V0 = mass / mixture.Density;
            //UnityEngine.Debug.Log("pr: " + Pressure + " - V0: " + V0 + " - Density: " + mixture.Density);
            mixture.pressure = Pressure * V0 / Volume;
        }

        public void Update(float dT)
        {
            if (mixture != null)
            {
                RecalculatePressure();
                mixture.React(dT);
            }
            UnityEngine.Debug.Log("-> " + mass+" - "+Pressure);
        }
    }
}
