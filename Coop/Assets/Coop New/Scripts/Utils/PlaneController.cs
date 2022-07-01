using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public Transform _transform;
    public Rigidbody _rigidbody;
    public Transform Target;
    public AircraftControler aircraftControler;

    public float MinThrust = 600f;
    public float MaxThrust = 1200f;
    float _currentThrust;
    public float ThrustIncreaseSpeed = 400f;

    float _deltaPitch;
    public float PitchIncreaseSpeed = 300f;

    float _deltaRoll;
    public float RollIncreaseSpeed = 300f;
    
    float _deltaYaw;
    public float YawIncreaseSpeed = 300f;

    public float thustPower = 500f;

    public bool isKeyboad;
    public bool isActive = true;

    

    float d1, d2, tmp; int count,previousCount; bool launched;

    Queue<MissilePositionLaunch>[] launchers;
    MissilePositionLaunch[] allLaunchers;
    public GameObject radar;
    PlanesData data;
    void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();

        launchers = new Queue<MissilePositionLaunch>[4];
        for (int i = 0; i < 4; i++)
            launchers[i] = new Queue<MissilePositionLaunch>();

        if(aircraftControler == null)
        {
            aircraftControler = GetComponent<AircraftControler>();
        }
    }
    private void Start()
    {
        allLaunchers = GetComponentsInChildren<MissilePositionLaunch>();
        
        foreach (MissilePositionLaunch launcher in allLaunchers)
        {
            if (launcher.name.StartsWith("AIM-9"))
            {
                launchers[0].Enqueue(launcher);
            }

            else if (launcher.name.StartsWith("AIM-120"))
            {
                launchers[1].Enqueue(launcher);
            }

            else if (launcher.name.StartsWith("GBU-16"))
            {
                launchers[2].Enqueue(launcher);
            }
            else if (launcher.name.StartsWith("Gun"))
            {
                launchers[3].Enqueue(launcher);
            }
        }
        if (isKeyboad == true) return;
        aircraftControler.enabled = true;

        radar = transform.Find("Radar").gameObject;
        radar.SetActive(true);

        data = GameObject.Find("Environment").GetComponent<PlanesData>();
    }
    void Update()
    {
        //run
        if (isKeyboad)
        {
            var thrustDelta = 0f;
            if (Input.GetKey(KeyCode.Space))
            {
                thrustDelta += ThrustIncreaseSpeed;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                thrustDelta -= ThrustIncreaseSpeed;
            }
            _currentThrust += thrustDelta * Time.deltaTime;
            _currentThrust = Mathf.Clamp(_currentThrust, MinThrust, MaxThrust);

            _deltaPitch = 0f;
            if (Input.GetKey(KeyCode.S))
            {
                _deltaPitch -= PitchIncreaseSpeed;
            }

            if (Input.GetKey(KeyCode.W))
            {
                _deltaPitch += PitchIncreaseSpeed;
            }
            _deltaPitch *= Time.deltaTime;

            _deltaRoll = 0f;
            if (Input.GetKey(KeyCode.A))
            {
                _deltaRoll += RollIncreaseSpeed;
            }

            if (Input.GetKey(KeyCode.D))
            {
                _deltaRoll -= RollIncreaseSpeed;
            }
            _deltaRoll *= Time.deltaTime;
            
            

            //fire
            if (Input.GetKeyDown(KeyCode.B))
            {
                FireWeapons(0);
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                FireWeapons(1);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                FireWeapons(2);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                FireWeapons(3);
            }
        }
        else
        {
            _deltaPitch = _deltaRoll = 0f;

            _currentThrust = (aircraftControler.speed * thustPower);

            _deltaPitch = (aircraftControler.up + aircraftControler.down);

            _deltaRoll = -(aircraftControler.right + aircraftControler.left);
        }
        _deltaYaw = 0f;
        if (Input.GetKey(KeyCode.E))
        {
            _deltaYaw += YawIncreaseSpeed;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            _deltaYaw -= YawIncreaseSpeed;
        }
        _deltaYaw *= Time.deltaTime;
        UpdateAmmoCounters();
    }

    public void FireWeapons(int v)
    {
        FireWeapon(v, previousCount);
    }

    private void UpdateAmmoCounters()
    {
        // Update the ammo counters.
        int AIM_9 = 0;
        int AIM_120 = 0;
        int GBU_16 = 0;
        int Gun = 0;
        int GunMagazine = 0;
        // This whole method is pretty inefficient, especially because it's in the update, but
        // this is just for the sake of demo.
        foreach (MissilePositionLaunch launcher in allLaunchers)
        {
            if (launcher.name.StartsWith("AIM-9"))
                AIM_9 += launcher.missileCount;
            else if (launcher.name.StartsWith("AIM-120"))
                AIM_120 += launcher.missileCount;
            else if (launcher.name.StartsWith("GBU-16"))
            {
                GBU_16 += launcher.missileCount;
            }
            else if (launcher.name.StartsWith("Gun"))
            {
                Gun += launcher.missileCount;
                GunMagazine += launcher.MagazineCount;
            }
        }
    }
    private void FireWeapon(int LauncherGroup, int missile)
    {
        if (launchers[LauncherGroup].Count > 0)
        {
            MissilePositionLaunch temp = launchers[LauncherGroup].Dequeue();
            
            if (LauncherGroup != 3)
            {
                d1 = d2 = tmp = 0;count = 0;
                if (data.enemyPlanes[missile] == null)
                {
                    data.enemyPlanes.RemoveAt(missile);
                }
                for (int i=0;  i < data.enemyPlanes.Count; i++)
                {
                    tmp = Vector3.Distance(transform.position, data.enemyPlanes[i].transform.position);
                    if(d1 > tmp)
                    {
                        d1 = tmp;
                    }
                    d2 = Vector3.Distance(transform.position, data.Base.transform.position);
                    count = i;
                }
                if (d1 < d2)
                {
                    //enemy closer
                    Target = data.enemyPlanes[count-1].transform;
                    previousCount = count;
                }
                else
                {
                    Target = data.Base.transform;
                }
            }
            temp.Launch(Target, GetComponent<Rigidbody>().velocity);
            launchers[LauncherGroup].Enqueue(temp);
        }
    }



    void FixedUpdate()
    {
        var localRotation = _transform.localRotation;
        localRotation *= Quaternion.Euler(0f, 0f, _deltaRoll);
        localRotation *= Quaternion.Euler(_deltaPitch, 0f, 0f);
        localRotation *= Quaternion.Euler(0f, _deltaYaw,  0f);
        _transform.localRotation = localRotation;
        _rigidbody.velocity = _transform.forward * (_currentThrust * Time.fixedDeltaTime);

        if (isActive)
        {
            radar.SetActive(false);
            isActive = false;
        }
        else
        {
            isActive = false;
            Invoke(nameof(ActiveRadar), 3f);
        }
    }
    private void ActiveRadar()
    {
        isActive = true;
        radar.SetActive(true);
    }
}