using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Assets.Scripts.Chemical
{
    public class Mixture {

        List<Product> products;
        float temperature;      // in °C
        public float pressure { get; private set; }
        public float mass { get { return products.Average(p => p.Mass / p.ThermalExpansion / temperature / pressure / p.Compressability); } }


        static List<Reaction> reactions;
        public static void LoadReactions()
        {
            using (StreamReader r = new StreamReader("reactions.json"))
            {
                string json = r.ReadToEnd();
                reactions = JsonUtility.FromJson<ReactionWrapper>(json).list;
            }
        }

        [Serializable]
        struct Reaction
        {
            Dictionary<Product, float> products;
            float Tmin;
            float Tmax;
            float pMin;
            Product catalyst;
        }
        [Serializable]
        struct ReactionWrapper
        {
            public List<Reaction> list;
        }
    }
}
