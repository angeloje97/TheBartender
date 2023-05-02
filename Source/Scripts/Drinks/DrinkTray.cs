using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheBartender
{

    public class DrinkTray : MonoBehaviour
    {
        public DrinkBehavior currentBehavior;
        DrinkBehavior previousBehavior;


        public Action<DrinkTray> OnBehaviorChange;
        public UnityEvent<bool> OnDrinkFound;
        void Start()
        {
            OnDrinkFound?.Invoke(currentBehavior != null);
        }

        // Update is called once per frame
        void Update()
        {
            if(currentBehavior != previousBehavior)
            {
                previousBehavior = currentBehavior;
                OnBehaviorChange?.Invoke(this);
                OnDrinkFound?.Invoke(currentBehavior != null);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (currentBehavior == null) return;
            if(collision.gameObject == currentBehavior.gameObject)
            {
                currentBehavior = null;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            var drinkBehavior = collision.gameObject.GetComponent<DrinkBehavior>();
            if (drinkBehavior == null) return;
            if (!drinkBehavior.sellable) return;
            currentBehavior = drinkBehavior;
        }
    }
}
