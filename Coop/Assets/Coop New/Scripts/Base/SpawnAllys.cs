using System.Collections;
using UnityEngine;

namespace Assets.Coop_New.Scripts.Base
{
    public class SpawnAllys : MonoBehaviour
    {
        public void Spawn(GameObject objects,Transform pos,int count)
        {
            for(int i = 0; i < count; i++)
            {
                Vector3 position = Vector3.zero;
                position.z = -10;
                if(i/2 == 0)
                {
                    position.x = 10;
                }
                else
                {
                    position.x = -10;
                }
                pos.position += position * i;
                GameObject ally = Instantiate(objects);
                ally.transform.localPosition = pos.position;
                ally.transform.localRotation = pos.rotation;
            }
            
        }
    }
}