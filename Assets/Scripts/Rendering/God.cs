using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Chemical;

namespace Assets.Scripts.Rendering
{

    public class God : MonoBehaviour
    {
        List<ReactionVessel> ActiveVessels;
        List<Pipe> ActivePipes;
        // Use this for initialization
        void Start()
        {
            //Chemical.Product.TestSave();
            Chemical.Product.Load();
            // Chemical.Mixture.TestSave();
            Chemical.Mixture.LoadReactions();

            ActiveVessels = new List<ReactionVessel>();
            ActivePipes = new List<Pipe>();

            ReactionVessel rvw = new ReactionVessel(10, 200);
            rvw.AddMixture(new Mixture(new Dictionary<string, float>() { { "water", 1f } }), 10000);
            ActiveVessels.Add(rvw);
            ReactionVessel rvv = new ReactionVessel(.1f, 200);
            rvv.AddMixture(new Mixture(new Dictionary<string, float>() { { "vcm", 1f } }), 100);
            ActiveVessels.Add(rvv);

            ReactionVessel rv = new ReactionVessel(5, 200);
            //rv.AddMixture(new Mixture(new Dictionary<string, float>() { { "water", 0.998f }, { "vcm", 0.002f } }), 4000);
            ActiveVessels.Add(rv);
            ActivePipes.Add(new Pipe(rvw, rv, 5, 0.2f));
            ActivePipes.Add(new Pipe(rvv, rv, 5, 0.1f));
        }

        // Update is called once per frame
        void Update()
        {
            ActivePipes.ForEach(p => p.Update(Time.deltaTime));
            ActiveVessels.ForEach(rv => rv.Update(Time.deltaTime));
        }
    }
}