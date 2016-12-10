using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Chemical;

namespace Assets.Scripts.Rendering
{

    public class God : MonoBehaviour
    {
        List<ReactionVessel> ActiveVessels;
        // Use this for initialization
        void Start()
        {
            //Chemical.Product.TestSave();
            Chemical.Product.Load();
            Chemical.Mixture.TestSave();
            Chemical.Mixture.LoadReactions();

            ActiveVessels = new List<ReactionVessel>();

            ReactionVessel rv = new ReactionVessel(5, 200);
            rv.AddMixture(new Mixture(new Dictionary<string, float>() { { "water", 0.998f }, { "vcm", 0.002f } }), 2000);
            ActiveVessels.Add(rv);
        }

        // Update is called once per frame
        void Update()
        {
            ActiveVessels.ForEach(rv => rv.Update(Time.deltaTime));
        }
    }
}