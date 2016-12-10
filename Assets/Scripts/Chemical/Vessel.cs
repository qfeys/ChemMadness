using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Chemical
{
    interface Vessel
    {
        float Pressure { get; }    // In atm
        void AddMixture(Mixture mix, float mass);
        Mixture RemoveMixture(float mass);
    }
}
