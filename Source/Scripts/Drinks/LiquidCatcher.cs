using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheBartender
{

    public class LiquidCatcher : MonoBehaviour
    {
        DrinkBehavior behavior;
        void Start()
        {
            behavior = GetComponentInParent<DrinkBehavior>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter(Collider other)
        {
            if (behavior == null) return;
           
            var liquidBehavior = other.GetComponent<LiquidBehavior>();
            if (!liquidBehavior) return;
            if (liquidBehavior.source == behavior) return;
            behavior.AddMixture(liquidBehavior.currentMixture);
            liquidBehavior.DestroySelf("From Liquid Catcher");
        }
    }
}
