using System;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public Transform _transform;
    public Rigidbody _rigidbody;
    public Transform target;
    public AircraftControler aircraftControler;
    public Camera Camera;
    public Transform CameraTarget;
    [Range(0, 1)] public float CameraSpring = 0.96f;

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

    public bool isKeyboad = true;
    Queue<MissilePositionLaunch>[] launchers;
    MissilePositionLaunch[] allLaunchers;
    

    void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
        if(Camera != null)
        {
            Camera.transform.SetParent(null);
        }

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
            print(launcher.name);
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
            
            _deltaYaw= 0f;
            if (Input.GetKey(KeyCode.E))
            {
                _deltaYaw += YawIncreaseSpeed;
            }

            if (Input.GetKey(KeyCode.Q))
            {
                _deltaYaw -= YawIncreaseSpeed;
            }
            _deltaYaw *= Time.deltaTime;

            //fire
            if (Input.GetKeyDown(KeyCode.B))
            {
                FireWeapon(0);
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                FireWeapon(1);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                FireWeapon(2);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                FireWeapon(3);
            }
        }
        else
        {
            _deltaPitch = _deltaRoll = 0f;

            _currentThrust = (aircraftControler.speed * thustPower);

            _deltaPitch = (aircraftControler.up + aircraftControler.down);

            _deltaRoll = -(aircraftControler.right + aircraftControler.left);
        }
        

        
        
        UpdateAmmoCounters();
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

    public void FireWeapon(int LauncherGroup)
    {
        if (launchers[LauncherGroup].Count > 0)
        {
            MissilePositionLaunch temp = launchers[LauncherGroup].Dequeue();
            temp.Launch(target, GetComponent<Rigidbody>().velocity);
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

        if (Camera == null) return;
        Vector3 cameraTargetPosition = _transform.position + _transform.forward * -8f + new Vector3(0f, 3f, 0f);
        var cameraTransform = Camera.transform;

        cameraTransform.position = cameraTransform.position * CameraSpring + cameraTargetPosition * (1 - CameraSpring);
        Camera.transform.LookAt(CameraTarget);
    }
}