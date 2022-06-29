using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerRadar : MonoBehaviour
{
    public LayerMask enemy;
    public LayerMask missile;
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Enemy"))
        {
            //GameObject is Player.
            print("Enemy Found");
        }
        
        
    }
}
