using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuintensUITools;

namespace Assets.Scripts.Rendering
{
    interface Sensor
    {
        string name { get; }

        List<Tuple<string, string>> Data();
    }
}
