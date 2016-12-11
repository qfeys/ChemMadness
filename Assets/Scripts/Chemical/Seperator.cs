using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Chemical
{
    class Seperator: Controlled
    {
        float Volume;
        Product primeSeperation;
        float seperationSpeed;      // halflive in seconds
        public subVessel main;
        public subVessel primeOut;
        public subVessel restOut;

        public float Control { get; set; }

        public Seperator(float Volume, Product primeSeperation, float seperationSpeed)
        {
            this.Volume = Volume; this.primeSeperation = primeSeperation; this.seperationSpeed = seperationSpeed;
        }

        public float Pressure
        {
            get
            {
                main.mixture.pressure = _pressure; primeOut.mixture.pressure = _pressure; restOut.mixture.pressure = _pressure;
                float V0 = main.mass / main.mixture.Density + primeOut.mass / primeOut.mixture.Density + restOut.mass / restOut.mixture.Density;
                _pressure = _pressure * V0 / Volume;
                main.mixture.pressure = _pressure; primeOut.mixture.pressure = _pressure; restOut.mixture.pressure = _pressure;
                return Pressure;
            }
        }
        float _pressure = 1;

        public void AddMixture(Mixture mix, float mass)
        {
            main.AddMixture(mix, mass);
        }

        public void Update(float dT)
        {
            float maxMass = main.mixture.Density * Volume;
            if (main.mixture.products.ContainsKey(primeSeperation))
            {
                float massRemoved = maxMass / seperationSpeed * dT * Control;
                float massSeperated = massRemoved * main.mixture.products[primeSeperation];
                primeOut.AddMixture(new Mixture(new Dictionary<Product, float>() { { primeSeperation, 1.0f } }),massSeperated);
                var restProducts = new Dictionary<Product,float>( main.mixture.products);
                restProducts.Remove(primeSeperation);
                restOut.AddMixture(new Mixture(restProducts), massRemoved - massSeperated);
                main.QuickRemove(massRemoved);
            }
        }

        private void FlushThrough(subVessel demander, float mass)
        {
            demander.AddMixture(main.mixture, mass);
            main.QuickRemove(mass);
        }

        public class subVessel : Vessel
        {
            public Mixture mixture;
            public float mass { get; set; }
            Seperator parent;
            public float Pressure { get { return parent.Pressure; } }
            public float Temperature { get { return mixture.temperature; } }

            public subVessel()
            {
                mixture = new Mixture();
            }

            public void AddMixture(Mixture mix, float mass)
            {
                mixture.Add(mix, mass / this.mass);
                this.mass += mass;
            }

            public void QuickRemove(float mass)
            {
                this.mass -= mass;
                if (mass <= 0)
                {
                    mixture = new Mixture();
                    mass = 0;
                }
            }

            public Mixture RemoveMixture(float volume, out float mass)
            {
                mass = volume * mixture.Density;
                if (this.mass < mass)
                {
                    parent.FlushThrough(this, this.mass - mass);
                }
                this.mass -= mass;
                return mixture;
            }

            public void Update(float dT)
            {
                throw new NotImplementedException();
            }
        }
    }
}
