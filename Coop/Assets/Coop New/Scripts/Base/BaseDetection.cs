using System.Collections;
using UnityEngine;

namespace Assets.Coop_New.Scripts.Base
{
    public class BaseDetection : MonoBehaviour
    {
        public SpawnAllys spawn;
        public GameObject pos;
        public GameObject prefeb;
        public Collider[] col;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                spawn.Spawn(prefeb,pos.transform,3);
                foreach (Collider col in col)
                {
                    col.enabled = false;
                }
            }
        }
    }
}