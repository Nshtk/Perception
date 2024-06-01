using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedDestruction : MonoBehaviour
{
    public float Delay = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, Delay);
    }
}
