using Assets.Coop_New.Scripts.Display;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour
{
    [SerializeField] Camera Camera;
    [SerializeField] GameObject planePrefeb;
    [SerializeField] GameObject basePrefeb;
    [SerializeField] GameObject[] spawnLocations;

    public float planeSpeed;

    int positionLength = 0;
    // Start is called before the first frame update
    void Start()
    {
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
            Camera.GetComponent<MissionWaypoint>().target = basePosition.transform;
            Camera.GetComponent<MissionWaypoint>().enabled = true;
        }
    }

    void SpawnPlane(int location)
    {
        int numberOfspawn = 3;
        for (int i = 0; i < numberOfspawn; i++)
        {

            GameObject playerPlane = Instantiate(planePrefeb);

            playerPlane.SetActive(false);

            playerPlane.transform.localPosition = spawnLocations[location].transform.localPosition + new Vector3(0, 50, 0);
            playerPlane.transform.localRotation = spawnLocations[location].transform.localRotation;
            playerPlane.transform.parent = this.transform;
            PlaneController playerPlaneControler = playerPlane.AddComponent<PlaneController>();
            playerPlaneControler._transform = playerPlane.transform;
            playerPlaneControler._rigidbody = playerPlane.GetComponent<Rigidbody>();
            playerPlaneControler.target = basePrefeb.transform;
            if (i == 0)
            {
                playerPlaneControler.Camera = Camera;
                playerPlaneControler.CameraTarget = playerPlane.transform;
            }
            if(i == 1)
            {
                playerPlane.transform.position += new Vector3(-20, 0, -20);
            }
            if(i == 2)
            {
                playerPlane.transform.position += new Vector3(20, 0, -20);
            }

            playerPlane.SetActive(true);
        }
    }
}
