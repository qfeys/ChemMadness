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
        public float pressure { get;  set; }
        public float Density { get {
                return products.Count == 0 ? 0.001f : products.Sum(p => p.Key.ActualDensity(temperature, pressure) * p.Value) / products.Sum(p => p.Value)*1000;    // in kg/m^3
            } }

        public Mixture()
        {
            products = new Dictionary<Product, float>();
            products.Add(Product.Find("air"), 1);
            pressure = 1;
        }

        public Mixture(Dictionary<Product, float> products)
        {
            this.products = products;
            pressure = 1;
        }
        public Mixture(Dictionary<string, float> products)
        {
            this.products = new Dictionary<Product, float>();
            foreach(var p in products)
            {
                this.products.Add(Product.Find(p.Key), p.Value);
            }
            pressure = 1;
        }

        static List<Reaction> AllReactions;

        /// <summary>
        /// Ratio is newMass / oldMass
        /// </summary>
        /// <param name="mix"></param>
        /// <param name="ratio"></param>
        internal void Add(Mixture mix, float ratio)
        {
            Dictionary<Product, float> buffer = new Dictionary<Product, float>(products);
            foreach (var p in buffer)
            {
                if (mix.products.ContainsKey(p.Key))
                {
                    products[p.Key] += mix.products[p.Key] * ratio;
                }
            }
            // normilize
            float s = products.Sum(p => p.Value);
            foreach (Product p in buffer.Keys)
            {
                products[p] *= 1 / s;
            }
        }

        internal void React(float dT)
        {
            if (products.Count > 1)
                Debug.Log("Reacting?");
            foreach (Reaction reaction in AllReactions)
            {
                if(reaction.reagens.All(n => products.Any(p => p.Key.Name.Equals(n.name)))             // all reagents are present
                    && pressure >= reaction.pMin && temperature >= reaction.Tmin && temperature <= reaction.Tmax)    // conditions are met
                {
                    string log = "Reaction Happens! ";
                    foreach(var p in products)
                    {
                        log += ":" + p.Key.Name + ": " + p.Value;
                    }
                    Debug.Log(log);
                    // Find critical product, aka the product with the smallest relative 
                    // fraction, aka it's fraction corrected by the reaction ratio
                    ProdRatio critProd = new ProdRatio();
                    List<ProdRatio> normilizedReagensRatios = new List<ProdRatio>();
                    float ReagensSum = reaction.reagens.Sum(rr => rr.ratio);
                    foreach(var r in reaction.reagens)
                    {
                        normilizedReagensRatios.Add(new ProdRatio(r.name, r.ratio / ReagensSum));
                    }
                    foreach (var newProduct in normilizedReagensRatios)
                    {
                        if (critProd.name == null)
                        {
                            critProd = newProduct;
                        }
                        else
                        {
                            float critProtRelativeFraction = products[Product.Find(critProd.name)] / critProd.ratio;
                            float newProdRelativeFraction = products[Product.Find(newProduct.name)] / newProduct.ratio;
                            if (newProdRelativeFraction < critProtRelativeFraction)
                                critProd = newProduct;
                        }
                    }

                    // There are 3 components that adjust the reactionspeed:
                    // 1) If the critical product is only lesser present, the reaction will be slower then if it was fully present
                    //      This is implemented as the relative fraction
                    // 2) If there are other product present, the reactionspeed will slow down due to the interference of the other products
                    //      This is implemented as the product of all deficiancies.
                    // 3) At elevated temperatures, the reactionspeed is higher
                    float relativeFraction = products[Product.Find(critProd.name)] / critProd.ratio;
                    float productOfDeficianties = 1;
                    foreach(var p in normilizedReagensRatios)
                    {
                        productOfDeficianties *= Mathf.Pow(products[Product.Find(p.name)] / p.ratio, p.ratio);
                    }
                    float changeByT = 1;

                    float ModifiedReactionTime = reaction.reactionTime / (relativeFraction * productOfDeficianties * changeByT);
                    Debug.Log("ReactionSpeed: " + ModifiedReactionTime);

                    // Create new product and remove the old product
                    // 1) removal of reagens
                    foreach(var r in normilizedReagensRatios)
                    {
                        products[Product.Find(r.name)] -= r.ratio * dT / ModifiedReactionTime;
                    }
                    // 2) creation of new product
                    List<ProdRatio> normilizedProductRatios = new List<ProdRatio>();
                    float ProductsSum = reaction.reagens.Sum(rr => rr.ratio);
                    foreach (var p in reaction.product)
                    {
                        normilizedProductRatios.Add(new ProdRatio(p.name, p.ratio / ReagensSum));
                    }
                    foreach (var p in normilizedProductRatios)
                    {
                        if (products.ContainsKey(Product.Find(p.name)))
                            products[Product.Find(p.name)] += p.ratio * dT / ModifiedReactionTime;
                        else
                            products.Add(Product.Find(p.name), p.ratio * dT / ModifiedReactionTime);
                    }
                    // remove products that no longer exist from the list
                    foreach (var r in normilizedReagensRatios)
                    {
                        if (products[Product.Find(r.name)] <= 0)
                            products.Remove(Product.Find(r.name));
                    }
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
            public float reactionEnergy;
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
