using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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

        bool destroyed;
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
            if (other.GetComponent<XRDirectInteractor>()) return;
            if (other.GetComponentInParent<DrinkBehavior>()) return;

            DestroySelf($"{other.gameObject}");
            return;
        }

        public async void DestroySelf(string reason)
        {
            if (destroyed) return;
            await Task.Yield();
            Debug.Log($"5393 Destroyed from {reason}");
            OnDestroySelf?.Invoke();
            Destroy(gameObject);
        }
    }

}