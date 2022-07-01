using Assets.Coop_New.Scripts.Display;
using Display;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour
{
    [SerializeField] Camera Camera;
    [SerializeField] GameObject planePrefeb;
    [SerializeField] GameObject enemyPrefeb;
    [SerializeField] GameObject basePrefeb;
    [SerializeField] GameObject[] spawnLocations;
    //[HideInInspector] 
    public PlanesData planes;
    public float planeSpeed;
    public bool Joystick;
    int positionLength = 0;
    // Start is called before the first frame update
    void Start()
    {
        planes = GetComponent<PlanesData>();
        positionLength = spawnLocations.Length;
        int planeLocation = Random.Range(0, positionLength);
        SpawnPlane(planeLocation);
        SpawnBase(planeLocation);
    }

    private void SpawnBase(int planeLocation)
    {
        int baseLocation = Random.Range(0, positionLength);
        if(planeLocation == baseLocation)
        {
            SpawnBase(planeLocation);
            return;
        }
        else
        {
            GameObject basePosition = Instantiate(basePrefeb);
            basePosition.transform.localPosition = spawnLocations[baseLocation].transform.localPosition;
            basePosition.transform.localRotation = spawnLocations[baseLocation].transform.localRotation;
            basePosition.transform.parent = this.transform;
            Camera.GetComponent<BasePointer>().target = basePosition.transform;
            Camera.GetComponent<BasePointer>().enabled = true;
            planes.Base = GameObject.Find("Office building").gameObject;
            SpawnEnemyPlane(baseLocation);
        }
    }
    void SpawnEnemyPlane(int count)
    {
        int numberOfspawn = 3;
        for (int i = 0; i < numberOfspawn; i++)
        {
            Vector3 position = Vector3.zero;
            position.z = -10;
            if (i / 2 == 0)
            {
                position.x = 20;
            }
            else
            {
                position.x = -20;
            }
            
            spawnLocations[count].transform.position += position * i;
            GameObject ally = Instantiate(enemyPrefeb);
            ally.SetActive(false);

            ally.transform.localPosition = spawnLocations[count].transform.position + new Vector3(0,100,0);
            ally.transform.localRotation = spawnLocations[count].transform.rotation;

            ally.AddComponent<EnemyController>();

            planes.enemyPlanes.Add(ally);
        }
        
    }
    void SpawnPlane(int location)
    {
        int numberOfspawn = 3;
        for (int i = 0; i < numberOfspawn; i++)
        {
            Vector3 newPos;

            GameObject playerPlane = Instantiate(planePrefeb);

            playerPlane.SetActive(false);

            playerPlane.transform.localPosition = spawnLocations[location].transform.localPosition + new Vector3(0, 50, 0);
            playerPlane.transform.localRotation = spawnLocations[location].transform.localRotation;
            playerPlane.transform.parent = this.transform;
            PlaneController playerPlaneControler = playerPlane.AddComponent<PlaneController>();
            playerPlaneControler.isKeyboad = !Joystick;
            playerPlaneControler._transform = playerPlane.transform;
            playerPlaneControler._rigidbody = playerPlane.GetComponent<Rigidbody>();
            playerPlaneControler.Target = basePrefeb.transform;
            if(i % 2 == 0)
            {
                newPos = new Vector3(10, 0, -10);
            }
            else
            {
                newPos = new Vector3(-20, 0, -20);
            }
            playerPlane.transform.position += newPos * i;

            planes.planes.Add(playerPlane);
            playerPlane.SetActive(true);
            Camera.GetComponent<CameraController>().targets.Add(playerPlane);
        }
        Camera.GetComponent<CameraController>().enabled = true;
    }
}
