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
        public float Mass;  // mass per cubed meter at 1 atm at 0 °C
        public float ThermalExpansion;  // dV/dT
        public float Compressability;   // dV/dp
        public static Dictionary<string, Product> AllProducts;

        public static void Load()
        {
            using (StreamReader r = new StreamReader("products.json"))
            {
                string json = r.ReadToEnd();
                List<Product> pr = JsonUtility.FromJson<ProductWrapper>(json).list;
                foreach(Product p in pr)
                {
                    AllProducts.Add(p.Name, p);
                }
            }
        }

        public static void TestSave()
        {
            using (StreamWriter w = new StreamWriter("Assets/Json/products.json"))
            {
                Product p = new Product();
                p.Name = "pvc";
                p.Mass = 1.3f;
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
