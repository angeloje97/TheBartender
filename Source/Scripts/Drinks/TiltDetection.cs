using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheBartender
{
    public enum TiltState
    {
        Upright,
        Tilted
    }

    public class TiltDetection : MonoBehaviour
    {
        public TiltState tiltState;
        public float tiltThreshHold;

        public float tiltValue;
        public float tiltLerpValue;
        [SerializeField]
        public Vector3 currentRotation;

        TiltState previousTiltState;
        public Action<TiltState> OnTiltStateChange;
        public UnityEvent<bool> OnTilted;

        void Start()
        {
        
        }

        private void OnValidate()
        {
        }

        // Update is called once per frame
        void Update()
        {
            HandleState();
            HandleEvents();
        }

        void HandleEvents()
        {
            if(tiltState != previousTiltState)
            {
                previousTiltState = tiltState;
                OnTiltStateChange?.Invoke(tiltState);
            }
        }

        void HandleState()
        {
            currentRotation = transform.eulerAngles;


            var xValue = currentRotation.x > 180f ? 360f - currentRotation.x : currentRotation.x;
            var zValue = currentRotation.z > 180f ? 360f - currentRotation.z : currentRotation.z;

            tiltValue = Mathf.Max(xValue, zValue);

            tiltLerpValue = Mathf.InverseLerp(tiltThreshHold, 150, tiltValue);

            tiltState = tiltValue > tiltThreshHold ? TiltState.Tilted : TiltState.Upright;
        }
    }
}
