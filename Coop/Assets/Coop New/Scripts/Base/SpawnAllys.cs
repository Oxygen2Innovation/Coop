using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Base
{
    public class SpawnAllys : MonoBehaviour
    {
        public async Task Spawn(GameObject objects,Transform pos,int count,Transform target)
        {
            for(int i = 0; i < count; i++)
            {
                Vector3 position = Vector3.zero;
                position.z = -10;
                if(i/2 == 0)
                {
                    position.x = 20;
                }
                else
                {
                    position.x = -20;
                }
                pos.position += position * i;
                GameObject ally = Instantiate(objects);
                ally.SetActive(false);
                
                ally.transform.localPosition = pos.position;
                ally.transform.localRotation = pos.rotation;

                EnemyController controller = ally.AddComponent<EnemyController>();
                controller.target = target;
                
                ally.SetActive(true);
                await Task.Yield();
            }
        }
    }
}