using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoTemp : MonoBehaviour
{
    public Action<MonoTemp> OnUpdate;
    public Action<Collision> OnCollision;
    public Action<Collider> OnTrigger;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdate?.Invoke(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTrigger?.Invoke(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollision?.Invoke(collision);
    }
}
