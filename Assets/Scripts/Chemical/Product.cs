using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Chemical
{
    [Serializable]
    public class Product
    {
        public string Name;
        public float Density;  // mass per cubed meter at 1 atm at 0 °C
        public float ThermalExpansion;  // dV/dT
        public float Compressability;   // dV/dp
        public static Dictionary<string, Product> AllProducts;

        public float ActualDensity(float temp, float press)
        {
            //Debug.Log("name: "+Name+" Dens: " + Density + " thermal: " + ThermalExpansion +" press: "+press+ " comprs: " + Compressability + " result:"+
            //    (Density / (((ThermalExpansion - 1) * temp) + 1) *Mathf.Pow(press, Compressability - 1)));
            return Density / (((ThermalExpansion - 1) * temp) + 1) * Mathf.Pow(press, Compressability - 1);
        }

        internal static Product Find(string name)
        {
            return AllProducts[name];
        }

        public static void Load()
        {
            using (StreamReader r = new StreamReader("Assets/Json/products.json"))
            {
                string json = r.ReadToEnd();
                List<Product> pr = JsonUtility.FromJson<ProductWrapper>(json).list;
                AllProducts = new Dictionary<string, Product>();
                foreach(Product p in pr)
                {
                    AllProducts.Add(p.Name, p);
                }
            }
        }

        public static void TestSave()
        {
            throw new NotSupportedException();
            using (StreamWriter w = new StreamWriter("Assets/Json/products.json"))
            {
                Product p = new Product();
                p.Name = "pvc";
                p.Density = 1.3f;
                ProductWrapper pw = new ProductWrapper();
                pw.list = new List<Product>() { p };
                string js = JsonUtility.ToJson(pw,true);
                w.Write(js);
            }
        }

        [Serializable]
        struct ProductWrapper
        {
            public List<Product> list;
        }
    }
}
