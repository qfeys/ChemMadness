using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using QuintensUITools;

namespace Assets.Scripts.Rendering
{
    class DataDisplay : MonoBehaviour
    {
        public void Update()
        {
            List<Sensor> sens = God.TheOne.Sensors;

            GameObject overviewWindow = transform.GetChild(1).GetChild(0).gameObject;
            var EthylInfo = overviewWindow.transform.GetChild(1).GetChild(1).GetComponent<InfoTable>();
            EthylInfo.SetInfo(sens[1].Data());
            EthylInfo.Redraw();
        }
    }
}
