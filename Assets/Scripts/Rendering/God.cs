using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.Scripts.Chemical;

namespace Assets.Scripts.Rendering
{

    public class God : MonoBehaviour
    {
        public static God TheOne;
        List<Vessel> ActiveVessels;
        List<Pipe> ActivePipes;
        internal List<Sensor> Sensors;

        public void Awake()
        {
            if (TheOne != null) throw new Exception("God already instantiated!");
            TheOne = this;
        }

        void Start()
        {
            //Chemical.Product.TestSave();
            Chemical.Product.Load();
            // Chemical.Mixture.TestSave();
            Chemical.Mixture.LoadReactions();

            ActiveVessels = new List<Vessel>();
            ActivePipes = new List<Pipe>();
            Sensors = new List<Sensor>();
            Generate();
            /*
            ReactionVessel rvw = new ReactionVessel(10, 200);
            rvw.AddMixture(new Mixture(new Dictionary<string, float>() { { "water", 1f } }), 12000);
            ActiveVessels.Add(rvw);
            ReactionVessel rvv = new ReactionVessel(.1f, 200);
            rvv.AddMixture(new Mixture(new Dictionary<string, float>() { { "vcm", 1f } }), 200);
            ActiveVessels.Add(rvv);

            ReactionVessel rv = new ReactionVessel(5, 200);
            rv.AddMixture(new Mixture(new Dictionary<string, float>() { { "air", 1f } }), 200);
            //rv.AddMixture(new Mixture(new Dictionary<string, float>() { { "water", 0.998f }, { "vcm", 0.002f } }), 4000);
            ActiveVessels.Add(rv);
            ActivePipes.Add(new Pipe(rvw, rv, 5, 0.2f));
            ActivePipes.Add(new Pipe(rvv, rv, 5, 0.1f));
            */
        }

        // Update is called once per frame
        void Update()
        {
            ActivePipes.ForEach(p => p.Update(Time.deltaTime));
            ActiveVessels.ForEach(rv => rv.Update(Time.deltaTime));
        }

        void Generate()
        {
            OpenVessel ethylTank = new OpenVessel(100);
            ethylTank.AddMixture(new Mixture(Product.Find("ethylene")), 80000);
            Sensors.Add(new TankSensor("Ethylene Tank", ethylTank));

            ReactionVessel EDCTank = new ReactionVessel(1, 20);
            Sensors.Add(new TankSensor("EDC Tank", EDCTank));

            ReactionVessel HClTAnk = new ReactionVessel(10, 100);
            HClTAnk.AddMixture(new Mixture(Product.Find("HCl")), 10000);
            Sensors.Add(new TankSensor("HCl Tank", HClTAnk));
        }
    }
}