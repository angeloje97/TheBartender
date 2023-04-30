using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TheBartender
{
    public enum UIManagerEvent
    {
        OnToggleShowDrinks
    }
    public class UIManager : MonoBehaviour
    {
        public static UIManager active { get; private set; }
        public bool showToolTips;

        //Action<UIManager> OnToggleToolTips;

        Dictionary<UIManagerEvent, Action<UIManager>> actionDictionary;

        private void Awake()
        {
            if (active && active != this)
            {
                Destroy(gameObject);
                return;
            }

            active = this;
        }

        public Action AddListener<T>(UIManagerEvent eventType, Action<UIManager> action, T caller) where T: MonoBehaviour
        {
            if (actionDictionary.ContainsKey(eventType))
            {
                actionDictionary.Add(eventType, null);
            }

            actionDictionary[eventType] += MiddleWare;
            bool removed = false;
            return () => {
                if (removed) return;

                actionDictionary[eventType] -= MiddleWare;
                removed = true;
            };

            void MiddleWare(UIManager manager)
            {
                if(caller == null)
                {
                    actionDictionary[eventType] -= MiddleWare;
                    removed = true;
                    return;
                }

                action(manager);
            }
        }

        public void InvokeEvent(UIManagerEvent eventType)
        {
            actionDictionary ??= new();

            if (!actionDictionary.ContainsKey(eventType))
            {
                actionDictionary.Add(eventType, null);
            }

            actionDictionary[eventType]?.Invoke(this);
        }

        public void ToggleToolTips()
        {
            showToolTips = !showToolTips;
            InvokeEvent(UIManagerEvent.OnToggleShowDrinks);
        }
    }
}

