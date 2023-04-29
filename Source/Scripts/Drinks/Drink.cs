using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheBartender
{
    [CreateAssetMenu(fileName = "New Drink", menuName = "Bartender/Drink")]
    public class Drink : ScriptableObject
    {
        public static float alpha { get { return .80f; } }

        public new string name;
        public Color liquidColor;
        public int id;
        public GameObject pourParticles;
        public int particlesPerPour;

        public bool Equals(Drink other)
        {
            return other.id == id; 
        }
    }
}
