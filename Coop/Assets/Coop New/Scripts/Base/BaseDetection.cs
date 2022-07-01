using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Base
{
    public class BaseDetection : MonoBehaviour
    {
        public GameObject pos;
        public GameObject prefeb;
        public Collider[] col;
        public PlanesData environment;
        private void Start()
        {
            environment = GameObject.Find("Environment").GetComponent<PlanesData>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                foreach (Collider col in col)
                {
                    col.enabled = false;
                }
                //StartCoroutine(spawn.Spawn(prefeb, pos.transform, 3));
                //await spawn.Spawn(prefeb, pos.transform, 3,other.transform);
                foreach(var enemy in environment.enemyPlanes)
                {
                    enemy.GetComponent<EnemyController>().target = other.transform;
                    enemy.SetActive(true);
                }
            }
        }
    }
}