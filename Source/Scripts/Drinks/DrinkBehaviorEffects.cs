using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Architome;
using System.Threading.Tasks;

namespace TheBartender
{
    public class DrinkBehaviorEffects : MonoBehaviour
    {
        public DrinkBehavior behavior;
        public AudioManager audioManager;
        public AudioClip outSound;
        public AudioClip inSound;

        float outDelay;
        float inTimer = 0f;

        void Start()
        {
            GetDependencies();
        }

        private void Update()
        {
            if(outDelay > 0)
            {
                outDelay -= Time.deltaTime;
            }

            if(inTimer > 0)
            {
                inTimer -= Time.deltaTime;
            }
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
            if (outDelay > 0) return;
            outDelay = .25f;
            Debug.Log($"445 Playing sound for {gameObject}");
            audioManager.PlayAudioClip(outSound);
        }

        async void HandleInput(DrinkBehavior behavior)
        {
            if (inSound == null) return;
            if (!audioManager) return;
            if(inTimer > 0)
            {
                inTimer = 1f;
                return;
            };

            inTimer = 1f;

            var source = audioManager.PlaySoundLoop(inSound);

            while(inTimer > 0)
            {
                await Task.Yield();
            }

            var target = 0f;

            while(source.volume != target)
            {
                source.volume = Mathf.Lerp(source.volume, target, .125f);
                var difference = Mathf.Abs(source.volume - target);
                if(difference < .0625f)
                {
                    break;
                }
            }

            source.Stop();
        }
    }
}
