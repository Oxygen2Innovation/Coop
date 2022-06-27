using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCollisionDetection : MonoBehaviour
{
    public bool isBase;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Missile"))
        {
            GameObject bases = gameObject.transform.parent.gameObject;
            Destroy(bases);
        }
    }
}
