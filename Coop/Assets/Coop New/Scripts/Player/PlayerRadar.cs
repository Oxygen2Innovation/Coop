using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRadar : MonoBehaviour
{
    public LayerMask enemy;
    public LayerMask missile;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == enemy)
        {
            //GameObject is Player.
            print("Enemy Found");

        }
    }
}
