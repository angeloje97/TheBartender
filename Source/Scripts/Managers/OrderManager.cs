using Architome;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheBartender
{

    public class OrderManager : MonoBehaviour
    {

        public static OrderManager active;
        public List<Drink> possibleDrinks;

        public List<DrinkBehavior.Mixture> currentOrder, result;
        public List<float> availbleSizes;

        public bool orderActive;

        public Action<OrderManager> OnCreateOrder;
        public Action<OrderManager> OnFulFillOrder;
        public Action<OrderManager> OnEndOrder;

        private void Awake()
        {
            if(active && active != this)
            {
                Destroy(gameObject);
                return;
            }

            active = this;
        }

        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void EndOrder()
        {
            orderActive = false;
            currentOrder = null;
            OnEndOrder?.Invoke(this);
        }

        public float Accuracy()
        {
            if (result == null) throw new Exception("There needs to be an order placed");
            var differences = new List<float>();
            var totalDifference = 0f;
            foreach(var a in result)
            {
                float difference = 1f;
                foreach(var b in currentOrder)
                {
                    if (!a.drink.Equals(b.drink)) continue;
                    difference = (Mathf.Abs(a.amount - b.amount)) / ((a.amount + b.amount) / 2);
                }

                Debug.Log($"7901: {a.drink.name} has a difference of {difference}");


                totalDifference += difference;
                differences.Add(difference);
            }


            var finalDifference = differences.Count > 0 ? totalDifference / differences.Count : 1f;

            return 1 - finalDifference;
        }

        public void CreateOrder(List<DrinkBehavior.Mixture> mixtures)
        {
            if (orderActive) return;
            currentOrder = mixtures;
            result = null;
            orderActive = true;
            OnCreateOrder?.Invoke(this);
        }

        public void CreateRandomOrder(int difficulty = 1)
        {
            //var drinks = ArchGeneric.RandomSubset(possibleDrinks, difficulty);
            var drinks = ArchGeneric.Shuffle(possibleDrinks);
            var size = ArchGeneric.RandomItem(availbleSizes);
            var currentSize = 0f;

            var order = new List<DrinkBehavior.Mixture>();

            foreach(var drink in drinks)
            {
                if (size - currentSize < 1f) break;
                var randomAmount = UnityEngine.Random.Range(1f, (size - 1f) - currentSize);
                randomAmount = Mathf.Floor(randomAmount);
                randomAmount = Mathf.Clamp(randomAmount,1f, size);

                bool contains = false;
                foreach(var mixture in order)
                {
                    if (mixture.drink.Equals(drink))
                    {
                        contains = true;
                        break;
                    }
                }

                if (contains) continue;

                order.Add(new() {
                    drink = drink,
                    amount = randomAmount,
                });

                currentSize += randomAmount;

                if (order.Count == difficulty) break;
            }

            CreateOrder(order);

        }

        public void FulFillOrder(DrinkBehavior drink)
        {
            result = drink.mixtures.ToList();
            drink.EmptyDrink();
            OnFulFillOrder?.Invoke(this);
            EndOrder();
        }

        public void CancelOrder()
        {
            EndOrder();
        }


    }

}