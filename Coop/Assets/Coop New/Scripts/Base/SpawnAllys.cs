using System.Collections;
using UnityEngine;

namespace Base
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

                //PlaneController allyControler = ally.AddComponent<PlaneController>();
                //allyControler._transform = ally.transform;
                //allyControler._rigidbody = ally.GetComponent<Rigidbody>();
                Tst tst = ally.AddComponent<Tst>();
                ally.GetComponent<AircraftControler>().enabled = true;
                ally.SetActive(true);
            }
            //yield return null;
        }
    }
}