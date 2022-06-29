using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Base
{
    public class BaseDetection : MonoBehaviour
    {
        public SpawnAllys spawn;
        public GameObject pos;
        public GameObject prefeb;
        public Collider[] col;
        private async void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                foreach (Collider col in col)
                {
                    col.enabled = false;
                }
                //StartCoroutine(spawn.Spawn(prefeb, pos.transform, 3));
                await spawn.Spawn(prefeb, pos.transform, 3,other.transform);
            }
        }
    }
}