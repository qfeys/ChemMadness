using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Chemical
{
    class HeatExchanger
    {
        Pipe pipe1;
        Pipe pipe2;
        float ThermalConductivity;    // in J/°C

        public HeatExchanger(Pipe pipe1, Pipe pipe2, float ThermalConductivity)
        {
            this.pipe1 = pipe1; this.pipe2 = pipe2; this.ThermalConductivity = ThermalConductivity;
        }

        public void Update(float dT)
        {
            float dTmp = pipe2.Temperature - pipe1.Temperature;
            float heat = dTmp * ThermalConductivity * dT;
            pipe1.AddHeat(-heat);
            pipe2.AddHeat(heat);
        }
    }
}
