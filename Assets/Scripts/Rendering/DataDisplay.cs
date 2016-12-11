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

        public GameObject standardInfoPanel;

        int step = 0;

        public void Update()
        {
            if (step == 0)
            {
                for (int i = 1; i < transform.GetChild(1).childCount; i++)
                {
                    transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
                }
                step++; return;
            } else if (step == 1)
            {
                for (int i = 1; i < transform.GetChild(1).childCount; i++)
                {
                    transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
                }
                step++; return;
            }
            List<Sensor> sens = God.TheOne.Sensors;
            GameObject overviewWindow = transform.GetChild(1).GetChild(0).gameObject;
            var tempInfo = overviewWindow.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<InfoTable>();
            tempInfo.SetInfo(sens.Find(s => s.name == "Ethylene Tank").Data());
            tempInfo.Redraw();
            GameObject PVCWindow = transform.GetChild(1).GetChild(1).gameObject;
            tempInfo = PVCWindow.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<InfoTable>();
            tempInfo.SetInfo(sens.Find(s => s.name == "HCl Tank").Data());
            tempInfo.Redraw();
            tempInfo = PVCWindow.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<InfoTable>();
            tempInfo.SetInfo(sens.Find(s => s.name == "EDC Tank").Data());
            tempInfo.Redraw();
            tempInfo = PVCWindow.transform.GetChild(1).GetChild(1).GetChild(2).GetComponent<InfoTable>();
            tempInfo.SetInfo(sens.Find(s => s.name == "p1").Data());
            tempInfo.Redraw();
        }
    }
}
