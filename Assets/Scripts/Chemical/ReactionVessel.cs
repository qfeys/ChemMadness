using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Chemical
{
    class ReactionVessel : Vessel
    {
        Mixture mixture;
        float mass;     // The mass of product in the vessel
        public float Pressure { get { return mixture.pressure; } }
        float Volume;
        float filled { get { return mass / mixture.mass; } } // between 0 and 1


        public void Update(float dT)
        {

        }
    }
}
