using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheBartender
{

    public class LiquidBehavior : MonoBehaviour
    {
        public List<DrinkBehavior.Mixture> currentMixture;
        public Color color;
        public DrinkBehavior source;
        public float lifeTime = 2f;

        public Action OnDestroySelf;

        [SerializeField] MeshRenderer meshRenderer;
        void Start()
        {
            Debug.Log(GetComponentInParent<LiquidBehavior>());
        }

        // Update is called once per frame
        void Update()
        {
            if(lifeTime > 0)
            {
                lifeTime -= Time.deltaTime;
                return;
            }

            Destroy(gameObject);
        }

        public void SetMixture(List<DrinkBehavior.Mixture> mixture, Color color, DrinkBehavior source)
        {
            currentMixture = mixture;
            this.color = color;
            this.source = source;

            meshRenderer.material.color = color;
        }

        private void OnTriggerEnter(Collider other)
        {
            var drinkBehavior = other.GetComponent<DrinkBehavior>();
            if (drinkBehavior) return;
            var liquidCatcher = other.GetComponent<LiquidCatcher>();

            if (liquidCatcher)
            {
                var parentBehavior = liquidCatcher.GetComponentInParent<DrinkBehavior>();
                if (parentBehavior == source) return;
                var success = parentBehavior.AddMixture(currentMixture);
                Debug.Log($"Added mixture to {parentBehavior}: {success}");
            }
            
            OnDestroySelf?.Invoke();
            Destroy(gameObject);
            return;
        }
    }

}