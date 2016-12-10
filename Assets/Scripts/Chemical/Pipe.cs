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
    }
}
