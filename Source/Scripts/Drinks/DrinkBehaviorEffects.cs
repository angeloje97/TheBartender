using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheBartender
{
    public class DrinkBehaviorEffects : MonoBehaviour
    {
        public DrinkBehavior behavior;
        public AudioClip outputSound;
        public AudioSource outputSource;
        public AudioSource inputSource;
        void Start()
        {
            GetDependencies();
        }

        void GetDependencies()
        {
            behavior = GetComponent<DrinkBehavior>();

            if (behavior)
            {
                behavior.OnRemoveMixture += HandleOutput;
                behavior.OnAddMixture += HandleInput;
            }
        }

        void HandleOutput(DrinkBehavior behavior)
        {
            if (outputSound == null) return;
            if (outputSource == null) return;
            var pitch = Random.Range(.95f, 1.05f);

        }

        void HandleInput(DrinkBehavior behavior)
        {

        }
    }
}
