using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildRotatingStop : MonoBehaviour
{
    public Transform playerBody;

    // Update is called once per frame
    void Update()
    {
        playerBody.transform.position = new Vector3(gameObject.transform.position.x, playerBody.transform.position.y, gameObject.transform.position.z);
    }
}
