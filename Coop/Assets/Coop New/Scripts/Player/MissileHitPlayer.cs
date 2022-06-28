using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileHitPlayer : MonoBehaviour
{
    public ParticleSystem blast;
    public LayerMask missile;
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Missile"))
        if (collision.gameObject.layer == missile)
        {
            print("hit");
            blast = Instantiate(blast);
            blast.transform.position = collision.transform.position;
            blast.transform.localScale = Vector3.one * 10f; 
            Destroy(blast,4f);
            Destroy(gameObject);
        }
    }
}
