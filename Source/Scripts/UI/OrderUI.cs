using Architome;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace TheBartender
{
    public class OrderUI : MonoBehaviour
    {
        public OrderManager orderManager;

        public DrinkTray drinkTray;


        [Header("Menu Board")]
        public TextMeshProUGUI number;
        public int currentDrinks;


        [Header("In Progress Components")]
        public TextMeshProUGUI orderTitle;
        public TextMeshProUGUI orderDetail;

        [Header("Result Screen")]
        public TextMeshProUGUI resultText;

        public UnityEvent OnFulfillOrder;

        void Start()
        {
            GetDependencies();
            
        }

        void GetDependencies()
        {
            orderManager = OrderManager.active;

            if (orderManager)
            {
                orderManager.OnCreateOrder += HandleCreateOrder;
                orderManager.OnFulFillOrder += HandleFulFillOrder;
                orderManager.OnEndOrder += HandleEndOrder;
                 
            }

            currentDrinks = 1;
            UpdateNumber();
        
        }

        public void FulfillOrder()
        {
            if (drinkTray.currentBehavior == null) return;
            orderManager.FulFillOrder(drinkTray.currentBehavior);
            OnFulfillOrder?.Invoke();
        }

        public void CancelOrder()
        {
            orderManager.CancelOrder();
        }

        void HandleEndOrder(OrderManager manager)
        {

        }

        void HandleFulFillOrder(OrderManager orderManager)
        {
            var accuracy = orderManager.Accuracy();
            resultText.text = $"Order completed with a {(int) (accuracy * 100)}% accuracy";
        }

        void HandleCreateOrder(OrderManager manager)
        {
            var list = new List<string>();
            orderTitle.text = "Drink Recipe";
            foreach(var mixture in manager.currentOrder)
            {
                list.Add($"{mixture.drink.name}: {mixture.amount}u");
            }

            orderDetail.text = ArchString.NextLineList(list);

        }

        public void CreateOrder()
        {
            orderManager.CreateRandomOrder(currentDrinks);
        }

        public void IncreaseNumber()
        {
            currentDrinks++;
            currentDrinks = Mathf.Clamp(currentDrinks, 1, 3);
            UpdateNumber();
        }

        public void DecreaseNumber()
        {
            currentDrinks--;
            currentDrinks = Mathf.Clamp(currentDrinks, 1, 3);
            UpdateNumber();
        }

        public void UpdateNumber()
        {
            number.text = $"{currentDrinks}";
        }


    }

}
