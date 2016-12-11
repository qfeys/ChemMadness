using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Chemical
{
    interface Vessel
    {
        float mass { get; set; }
        float Pressure { get; }    // In atm
        float Temperature { get; }    // In atm
        void AddMixture(Mixture mix, float mass);
        Mixture RemoveMixture(float volume, out float mass);
        void Update(float dT);
    }
}
