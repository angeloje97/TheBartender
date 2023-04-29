using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace TheBartender
{

    public class BarTask : MonoBehaviour
    {
        public static BarTask active;

        public Action OnUpdate;

        private void Awake()
        {
            if (active)
            {
                Destroy(gameObject);
                return;
            }

            active = this;
        }

        private void Start()
        {
        
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        public async Task Delay(float time)
        {
            float currentTime = 0;
            Action actions = () => {
                currentTime += Time.deltaTime;
            };

            OnUpdate += actions;

            while (currentTime < time)
            {
                await Task.Yield();
            }

            OnUpdate -= actions;
        }

        public async Task Interval(Predicate<int> predicate, float time)
        {
            var running = true;

            var currentTime = 0f;

            int intervals = 0;

            Action action = () => {
                if (currentTime < time)
                {
                    currentTime += Time.deltaTime;
                    return;
                }

                currentTime = 0f;
                intervals++;
                running = predicate(intervals);
            };


            OnUpdate += action;

            while (running)
            {
                await Task.Yield();
            }


            OnUpdate -= action;
        }
    }
}

