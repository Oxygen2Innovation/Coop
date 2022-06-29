using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileHit : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name + " Collider");
        if(collision.gameObject.CompareTag("Ground"))
        {
            return;
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    print(other.name + " trigger");
    //    if (other.CompareTag("Ground"))
    //    {
    //        return;
    //    }
    //    else
    //    {
    //        Destroy(other.gameObject);
    //    }
    //}
}
