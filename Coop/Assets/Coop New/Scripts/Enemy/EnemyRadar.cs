using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Enemy
{
    public class EnemyRadar : MonoBehaviour
    {
        public LayerMask player;
        public EnemyController controller;
        private async void OnTriggerEnter(Collider other)
        {
            var colliderTask = Task.Run(() => {
                if (other.CompareTag("Player"))
                {
                    //GameObject is Enemy.
                    controller.target = other.transform;
                    print("Player Found");
                }
            });
            await colliderTask;
        }
    }
}