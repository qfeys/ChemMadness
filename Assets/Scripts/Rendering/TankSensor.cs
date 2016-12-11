using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuintensUITools;

namespace Assets.Scripts.Rendering
{
    class TankSensor : Sensor
    {
        Chemical.Vessel tank;
        public string name { get; private set; }

        public TankSensor(string name, Chemical.Vessel tank)
        {
            this.name = name; this.tank = tank;
        }

        public List<Tuple<string,string>> Data()
        {
            List<Tuple<string, string>> ret = new List<Tuple<string, string>>();
            ret.Add(new Tuple<string, string>("Pressure", tank.Pressure.ToString()+ " atm"));
            ret.Add(new Tuple<string, string>("Content", tank.mass.ToString() + " kg"));
            return ret;
        }
    }
}
