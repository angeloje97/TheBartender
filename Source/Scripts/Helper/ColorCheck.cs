using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheBartender
{

    public class ColorCheck : MonoBehaviour
    {
        public Drink drink;
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnValidate()
        {
            if (drink == null) return;

            var renderer = GetComponent<MeshRenderer>();

            renderer.sharedMaterial.color = drink.liquidColor;
        }
    }
}
