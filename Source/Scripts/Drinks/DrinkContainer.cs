using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheBartender
{

    public class DrinkContainer : MonoBehaviour
    {
        public DrinkBehavior drinkBehavior;

        public float currentAmount;

        public float maxHeight;
        public float minHeight;
        

        public Transform liquidTransform;
        public MeshRenderer liquidRenderer;

        public bool copyMaxHeight;
        public bool copyMinHeight;


        void Start()
        {
            GetDependencies();
        }

        void GetDependencies()
        {
            drinkBehavior = GetComponent<DrinkBehavior>();

            if (drinkBehavior)
            {
                drinkBehavior.OnAddMixture += HandleAddMixture;
                drinkBehavior.OnRemoveMixture += HandleRemoveMixture;
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnValidate()
        {
            if (copyMaxHeight)
            {
                copyMaxHeight = false;
                maxHeight = liquidTransform.localScale.y;
            }

            if (copyMinHeight)
            {
                copyMinHeight = false;
                minHeight = liquidTransform.localScale.y;
            }
        }

        void HandleAddMixture(DrinkBehavior behavior)
        {

        }

        void HandleRemoveMixture(DrinkBehavior behavior)
        {

        }

        void UpdateLiquid()
        {
            //currentAmount = TotalAmount();

            //liquidRenderer.material.color = FinalColor(currentAmount);
            //var scale = liquidTransform.localScale;
            //var lerpValue = currentAmount / maxAmount;
            //Debug.Log(lerpValue);
            //var value = Mathf.Lerp(minHeight, maxHeight, lerpValue);
            //liquidTransform.localScale = new Vector3(scale.x, value, scale.z);
        }
    }
}
