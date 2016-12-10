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
        float mass;     // The mass of product in the vessel
        public float Pressure { get { return mixture.pressure; } }
        float filled { get { return mass / mixture.Density; } } // between 0 and 1

        public ReactionVessel(float Volume, float pressureLimit)
        {
            this.Volume = Volume; this.pressureLimit = pressureLimit;
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

        public void Update(float dT)
        {
            mixture.React(dT);        
        }
    }
}
