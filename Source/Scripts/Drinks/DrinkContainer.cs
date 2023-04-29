using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace TheBartender
{

    public class DrinkContainer : MonoBehaviour
    {
        public DrinkBehavior drinkBehavior;

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
            UpdateLiquid();
        }

        void HandleRemoveMixture(DrinkBehavior behavior)
        {
            UpdateLiquid();

        }

        void UpdateLiquid()
        {
            var behavior = drinkBehavior;
            liquidRenderer.material.color = behavior.currentColor;
            var fillPercent = behavior.amount / behavior.maxAmount;
            var scale = liquidTransform.localScale;
            var targetHeight = Mathf.Lerp(minHeight, maxHeight, fillPercent);
            liquidTransform.localScale = new Vector3(scale.x, targetHeight, scale.z);
        }
    }
}
