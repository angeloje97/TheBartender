using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using System;

namespace TheBartender
{

    public class DrinkBehavior : MonoBehaviour
    {
        [Serializable]
        public class Mixture
        {
            public Drink drink;
            public float amount;

        }

        public List<Mixture> mixtures;
        public float amount;
        public float maxAmount;
        public float amountPouredPerSecond;
        public Color currentColor;
        public TiltDetection tiltDetection;

        public Transform pourTransform;

        public float spread;
        public bool acceptsLiquid;
        public LiquidBehavior liquidBehavior;

        public Action OnUpdate;

        public Action<DrinkBehavior> OnAddMixture { get; set; }
        public Action<DrinkBehavior> OnRemoveMixture { get; set; }

        private void Start()
        {
            GetDependencies();
        }

        public virtual void GetDependencies()
        {
            TotalAmount();
            tiltDetection = GetComponent<TiltDetection>();
            

            if (tiltDetection)
            {
                tiltDetection.OnTiltStateChange += HandleTiltStateChange;
            }
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        private void OnDrawGizmosSelected()
        {
            if (pourTransform == null) return;
            Gizmos.DrawWireCube(pourTransform.position, Vector3.one * spread);
            float size = spread > .05f ? .05f : spread / 2f;
            Gizmos.DrawWireSphere(pourTransform.position, size);
        }

        

        protected virtual async void HandleTiltStateChange(TiltState state)
        {
            if (state != TiltState.Tilted) return;
            var pourInterval = .125f;
            var amountPerInterval = amountPouredPerSecond * pourInterval;

            while(tiltDetection.tiltState == TiltState.Tilted)
            {
                if (!Application.isPlaying) return;
                if (!ReleaseMixture(amountPerInterval)) break;

                await Task.Delay((int) (1000 * pourInterval));
            }

        }

        public float TotalAmount()
        {
            mixtures ??= new();

            var total = 0f;

            foreach(var mixture in mixtures)
            {
                total += mixture.amount;
            }

            amount = total;

            return total;
        }

        public Color FinalColor(float total)
        {
            Color color = new(0, 0, 0, 0);
            mixtures ??= new();
            
            foreach (var mixture in mixtures)
            {
                color = Color.Lerp(color, mixture.drink.liquidColor, mixture.amount / total);
            }

            currentColor = color;
            return color;
        }

        public bool AddMixture(List<Mixture> incomingMixture)
        {
            if (!acceptsLiquid) return false;
            mixtures ??= new();

            var addedMixtures = false;

            foreach (var incoming in incomingMixture)
            {
                var drink = incoming.drink;
                var amount = incoming.amount;

                var totalAmount = TotalAmount();

                if (amount + totalAmount > maxAmount)
                {
                    amount = maxAmount - totalAmount;
                }

                Debug.Log($"4393: Total Amount : {amount}");

                if (amount <= 0) continue;

                var added = false;

                foreach (var mixture in mixtures)
                {
                    if (mixture.drink.Equals(drink))
                    {
                        mixture.amount = Mathf.Clamp(mixture.amount + amount, 0f, maxAmount);
                        added = true;
                        addedMixtures = true;
                        break;
                    }
                }

                if (!added)
                {
                    mixtures.Add(new()
                    {
                        drink = drink,
                        amount = Mathf.Clamp(amount, 0f, maxAmount)
                    });

                    addedMixtures = true;

                }
            }
            TotalAmount();
            FinalColor(amount);
            OnAddMixture?.Invoke(this);
            return addedMixtures;
        }

        public bool ReleaseMixture(float amountPerInterval)
        {
            mixtures ??= new();
            if (mixtures.Count <= 0) return false;
            if (pourTransform == null) return false;

            var total = amount;
            var finalColor = FinalColor(total);
            var mixtureOutput = new List<Mixture>();

            Action finishAction = () => { };

            foreach(var mixture in mixtures)
            {
                var percentAmount = mixture.amount / total;
                Debug.Log($"Drink {mixture.drink} percent amount is {percentAmount}");

                var targetAmount = percentAmount * amountPerInterval;

                mixture.amount -= targetAmount;

                if(mixture.amount <= 0)
                {
                    finishAction += () =>
                    {
                        mixtures.Remove(mixture);
                    };
                }

                mixtureOutput.Add(new()
                {
                    drink = mixture.drink,
                    amount = targetAmount,
                });
            }


            finishAction?.Invoke();
            var posX = UnityEngine.Random.Range(-spread, spread);
            var posZ = UnityEngine.Random.Range(-spread, spread);

            var liquidBehavior = Instantiate(this.liquidBehavior, pourTransform);
            liquidBehavior.transform.localPosition = new(posX, 0, posZ);
            liquidBehavior.transform.SetParent(null);

            liquidBehavior.SetMixture(mixtureOutput, finalColor, this);
            TotalAmount();
            OnRemoveMixture?.Invoke(this);
            return true;
        }



    }
}
