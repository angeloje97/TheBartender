using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Architome;

namespace TheBartender
{
    public class DrinkBehaviorEffects : MonoBehaviour
    {
        public DrinkBehavior behavior;
        public AudioManager audioManager;
        public AudioClip outSound;
        public AudioClip inSound;
        void Start()
        {
            GetDependencies();
        }

        void GetDependencies()
        {
            behavior = GetComponent<DrinkBehavior>();
            audioManager = GetComponent<AudioManager>();

            if (behavior && audioManager)
            {
                behavior.OnRemoveMixture += HandleOutput;
                behavior.OnAddMixture += HandleInput;
            }
        }

        void HandleOutput(DrinkBehavior behavior)
        {
            if (outSound == null) return;
            Debug.Log($"445 Playing sound for {gameObject}");
            audioManager.PlayAudioClip(outSound);
        }

        void HandleInput(DrinkBehavior behavior)
        {
        }
    }
}
