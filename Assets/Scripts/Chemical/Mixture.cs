using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Assets.Scripts.Chemical
{
    public class Mixture {

        Dictionary<Product, float> products;
        float temperature;      // in °C
        public float pressure { get; private set; }
        public float Density { get {
                return products.Average(p => p.Key.Density / p.Key.ThermalExpansion / temperature / pressure / p.Key.Compressability * p.Value);
            } }

        public Mixture(Dictionary<Product, float> products)
        {
            this.products = products;
        }
        public Mixture(Dictionary<string, float> products)
        {
            this.products = new Dictionary<Product, float>();
            foreach(var p in products)
            {
                this.products.Add(Product.Find(p.Key), p.Value);
            }
        }

        static List<Reaction> AllReactions;

        internal void Add(Mixture mix, float ratio)
        {
            foreach(var p in products)
            {
                if (mix.products.ContainsKey(p.Key))
                {
                    products[p.Key] += mix.products[p.Key] * ratio;
                }
            }
            // normilize
            float s = products.Sum(p => p.Value);
            foreach(var p in products.Keys) { products[p] *= 1 / s; }
        }

        internal void React(float dT)
        {
            foreach (Reaction r in AllReactions)
            {
                if(r.reagens.All(n => products.Any(p => p.Key.Name.Equals(n.name)))             // all reagents are present
                    && pressure >= r.pMin && temperature >= r.Tmin && temperature <= r.Tmax)    // conditions are met
                {
                    Debug.Log("Reaction Happens!");
                    float stoichMod = r.reagens.Min(p => p.ratio / products[Product.Find(p.name)]) / r.reagens.Sum(p => p.ratio);
                    if (stoichMod > 1) throw new Exception("Check this out!");
                    // temp & press mod
                    //float reactionSize
                }
            }
        }

        public static void LoadReactions()
        {
            using (StreamReader r = new StreamReader("Assets/Json/reactions.json"))
            {
                string json = r.ReadToEnd();
                AllReactions = JsonUtility.FromJson<ReactionWrapper>(json).list;
            }
        }

        public static void TestSave()
        {
            using (StreamWriter w = new StreamWriter("Assets/Json/reactions.json"))
            {
                Reaction r = new Reaction();
                r.reagens = new List<ProdRatio>() { new ProdRatio("vcm", 1), new ProdRatio("water", 1000) };
                r.product = new List<ProdRatio>() { new ProdRatio("pvc", 1), new ProdRatio("water", 1000) };
                r.reactionTime = 20;
                ReactionWrapper pw = new ReactionWrapper();
                pw.list = new List<Reaction>() { r };
                string js = JsonUtility.ToJson(pw, true);
                w.Write(js);
            }
        }

        [Serializable]
        struct Reaction
        {
            public List<ProdRatio> reagens;
            public List<ProdRatio> product;
            public float Tmin;
            public float Tmax;
            public float pMin;
            public string catalyst;
            public float reactionTime; // as halflive in seconds
        }
        [Serializable]
        struct ProdRatio
        {
            public string name; public float ratio;
            public ProdRatio(string name, float ratio) { this.name = name;  this.ratio = ratio; }
        }
        [Serializable]
        struct ReactionWrapper
        {
            public List<Reaction> list;
        }
    }
}
