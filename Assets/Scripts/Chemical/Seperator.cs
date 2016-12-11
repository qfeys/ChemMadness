using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Chemical
{
    class Seperator
    {
        float Volume;
        float mass;
        Product primeSeperation;
        float seperationSpeed;      // halflive in seconds
        public subVessel main;
        public subVessel primeOut;
        public subVessel restOut;

        public float Pressure { get { return mixture != null ? mixture.pressure : 0; } }

        public void AddMixture(Mixture mix, float mass)
        {
            main.AddMixture(mix, mass);
        }

        public void Update(float dT)
        {
            float maxMass = main.mixture.Density * Volume;
            if (main.mixture.products.ContainsKey(primeSeperation))
            {
                float massRemoved = maxMass / seperationSpeed * dT;
                float massSeperated = massRemoved * main.mixture.products[primeSeperation];
                primeOut.AddMixture(new Mixture(new Dictionary<Product, float>() { { primeSeperation, 1.0f } }),massSeperated);
                var restProducts = new Dictionary<Product,float>( main.mixture.products);
                restProducts.Remove(primeSeperation);
                restOut.AddMixture(new Mixture(restProducts), massRemoved - massSeperated);
                float a;
                main.RemoveMixture(massRemoved * main.mixture.Density, out a);
            }
        }

        public class subVessel : Vessel
        {
            public Mixture mixture;
            Seperator parent;
            public float Pressure { get { return parent.Pressure; } }

            public void AddMixture(Mixture mix, float mass)
            {
                throw new NotImplementedException();
            }

            public Mixture RemoveMixture(float volume, out float mass)
            {
                throw new NotImplementedException();
            }

            public void Update(float dT)
            {
                throw new NotImplementedException();
            }
        }
    }
}
