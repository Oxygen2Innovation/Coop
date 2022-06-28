﻿using System.Collections;
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
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                //StartCoroutine(spawn.Spawn(prefeb, pos.transform, 3));
                spawn.Spawn(prefeb, pos.transform, 3);
               
                foreach (Collider col in col)
                {
                    col.enabled = false;
                }
            }
        }
    }
}