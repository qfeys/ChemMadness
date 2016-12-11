using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuintensUITools;

namespace Assets.Scripts.Rendering
{
    class PipeSensor : Sensor
    {
        public string name { get; private set; }
        Chemical.Pipe pipe;

        public PipeSensor(string name, Chemical.Pipe pipe)
        {
            this.name = name; this.pipe = pipe;
        }

        public List<Tuple<string, string>> Data()
        {
            List<Tuple<string, string>> ret = new List<Tuple<string, string>>();
            ret.Add(new Tuple<string, string>("Flow Speed", pipe.flowSpeed.ToString("0.000 0") + " m/s"));
            ret.Add(new Tuple<string, string>("Mass Flow", pipe.MassFlow.ToString("0.000 00") + " kg/s"));
            ret.Add(new Tuple<string, string>("Temperature", pipe.Temperature.ToString("0.0") + " °C"));
            ret.Add(new Tuple<string, string>("Length", pipe.length.ToString("0.000") + " m"));
            ret.Add(new Tuple<string, string>("Diameter", (pipe.diameter*1000).ToString("0") + " mm"));
            return ret;
        }
    }
}
