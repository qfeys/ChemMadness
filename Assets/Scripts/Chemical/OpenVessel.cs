using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Chemical
{
    class OpenVessel : Vessel
    {
        float mass;
        float Volume;
        Mixture mixture;


        public float Pressure { get { return 1; } }

        public void AddMixture(Mixture mix, float mass)
        {
            if (mixture == null)
                mixture = mix;
            else
                mixture.Add(mix, mass / this.mass);
            this.mass += mass;
        }

        public Mixture RemoveMixture(float volume, out float mass)
        {
            mass = volume * mixture.Density;
            this.mass -= mass;
            return mixture;
        }

        public void Update(float dT)
        {
            if (mixture != null)
            {
                RecalculateVolume();
                mixture.React(dT);
            }
        }

        private void RecalculateVolume()
        {
            mixture.pressure = 1;
            if(mixture.Density * mass > Volume)
            {
                throw new Exception("Open vessel has overflown");
            }
        }
    }
}
