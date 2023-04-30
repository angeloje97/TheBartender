using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheBartender
{
    public class DrinkContainerUI : MonoBehaviour
    {
        public DrinkBehavior behavior;
        public UIManager uiManager;

        public CanvasGroup canvasGroup;
        void Start()
        {
            GetDependencies();
        }

        void GetDependencies()
        {
            uiManager = UIManager.active;

            if (uiManager)
            {
                uiManager.AddListener(UIManagerEvent.OnToggleShowDrinks, OnToggleDrinks, this);
            }
        }

        void OnToggleDrinks(UIManager manager)
        {

        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }

}