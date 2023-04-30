using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
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
        public bool setMin;
        public bool setMax;

        public float currentValue;
        bool updatingValue;

        public float lerpValue;

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

            UpdateLiquid();
            
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
            var scale = liquidTransform.localScale;

            if (setMin)
            {
                setMin = false;
                liquidTransform.localScale = new Vector3(scale.x, minHeight, scale.z);
            }

            if (setMax)
            {
                setMax = false;
                liquidTransform.localScale = new Vector3(scale.x, maxHeight, scale.z);
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

        async void UpdateLiquid()
        {
            if (updatingValue) return;
            var behavior = drinkBehavior;
            var material = liquidRenderer.material;
            updatingValue = true;
            if(lerpValue == 0)
            {
                lerpValue += .0625f;
            }


            do
            {
                currentValue = Mathf.Lerp(currentValue, behavior.amount, lerpValue);
                material.color = Color.Lerp(material.color, behavior.currentColor, lerpValue);
                

                var fillPercent = currentValue / behavior.maxAmount;
                var scale = liquidTransform.localScale;
                var targetHeight = Mathf.Lerp(minHeight, maxHeight, fillPercent);

                if(behavior.amount == 0)
                {
                    liquidTransform.localScale = new Vector3(scale.x, 0.005f, scale.z);
                }
                else
                {
                    liquidTransform.localScale = new Vector3(scale.x, targetHeight, scale.z);

                }


                if (Mathf.Abs(behavior.amount - currentValue) < .0001)
                {
                    currentValue = behavior.amount;
                    material.color = behavior.currentColor;
                }

                if (!Application.isPlaying) return;
                await Task.Yield();
            } while (currentValue != behavior.amount);
            updatingValue = false;
            
        }
    }
}
