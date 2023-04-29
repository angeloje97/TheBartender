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
        public float tiltMaximum;
        [SerializeField]
        public Vector3 currentRotation;

        TiltState previousTiltState;
        public Action<TiltState> OnTiltStateChange;
        public UnityEvent<bool> OnTilted;

        void Start()
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

            if(currentRotation.x > tiltThreshHold && currentRotation.x < 360 - tiltThreshHold)
            {
                tiltState = TiltState.Tilted;
                return;
            }

            if(currentRotation.z > tiltThreshHold && currentRotation.z < 360 - tiltThreshHold)
            {
                tiltState = TiltState.Tilted;
                return;
            }

            tiltState = TiltState.Upright;
        }
    }
}
