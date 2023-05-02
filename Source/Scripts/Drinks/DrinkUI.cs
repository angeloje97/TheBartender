using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Architome;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace TheBartender
{
    public class DrinkUI : MonoBehaviour
    {
        public DrinkBehavior behavior;
        public UIManager uiManager;

        public CanvasGroup canvasGroup;
        public TextMeshProUGUI text;

        public Vector3 originalPosition;
        public Transform parentObject;
        Camera mainCamera;
        bool worldCanvasSet;


        void Start()
        {
            GetDependencies();
            GoToWorldCanvas();
            GetMainCamera();
        }

        void GoToWorldCanvas()
        {
            var worldCanvas = uiManager.worldCanvas;
            originalPosition = transform.localScale;
            if (worldCanvas == null) return;

            parentObject = new GameObject("DrinkUI Anchor").transform;

            var localPosition = transform.localPosition;

            transform.SetParent(parentObject, true);
            parentObject.SetParent(worldCanvas);

            transform.localPosition = localPosition;
            transform.localRotation = new();

            worldCanvasSet = true;



        }

        private void Update()
        {
            if (!worldCanvasSet) return;
            if (parentObject == null) return;
            if (mainCamera == null) return;
            if (behavior == null) Destroy(gameObject);

            parentObject.LookAt(mainCamera.transform, Vector3.up);
            parentObject.transform.position = behavior.transform.position;

        }

        async void GetMainCamera()
        {
            while(Camera.main == null)
            {
                await Task.Yield();
            }

            mainCamera = Camera.main;
        }

        void GetDependencies()
        {
            uiManager = UIManager.active;
            behavior = GetComponentInParent<DrinkBehavior>();

            if (uiManager)
            {
                uiManager.AddListener(UIManagerEvent.OnToggleShowDrinks, OnToggleDrinks, this);
            }

            canvasGroup.SetCanvas(uiManager && uiManager.showToolTips);



            if (behavior)
            {
                behavior.OnAddMixture += UpdateText;
                behavior.OnRemoveMixture += UpdateText;
                UpdateText(behavior);
            }
            else
            {
                canvasGroup.gameObject.SetActive(false);
            }

        }

        void OnToggleDrinks(UIManager manager)
        {
            canvasGroup.SetCanvas(manager.showToolTips);

        }

        void UpdateText(DrinkBehavior behavior)
        {
            if (behavior == null) return;
            var mixtureString = MixtureString(behavior.mixtures);
            var fill = $"{Mathg.Round(behavior.amount, 2)}u/{(int) behavior.maxAmount}u";

            text.text = ArchString.NextLineList(new() {
                fill,
                mixtureString
            });

        }

        public string MixtureString(List<DrinkBehavior.Mixture> mixtureList)
        {
            var drinkLines = new List<string>();

            foreach(var mixture in mixtureList)
            {
                drinkLines.Add($"{mixture.drink.name}: {Mathg.Round(mixture.amount, 2)}u");
            }

            return ArchString.NextLineList(drinkLines);
        }
    }

}