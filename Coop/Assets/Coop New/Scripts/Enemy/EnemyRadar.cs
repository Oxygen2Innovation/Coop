using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class EnemyRadar : MonoBehaviour
    {
        public LayerMask player;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == player)
            {
                //GameObject is Enemy.
                transform.parent.gameObject.GetComponent<PlaneController>().target = other.gameObject.transform;
                print("Player Found");
            }
        }
        
    }
}