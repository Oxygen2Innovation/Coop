using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AircraftControler : MonoBehaviour
{
    public PlaneController airCraftControler;
    public JoystickControler controler;
    public float left;
    public float right;
    public float up;
    public float down;
    public float speed;
    private void OnEnable()
    {
        controler.Enable();
    }
    private void OnDisable()
    {
        controler.Disable();
    }
    private void Awake()
    {
        controler = new JoystickControler();
        
    }
    private void Start()
    {
        if(airCraftControler == null)
        {
            airCraftControler = GetComponent<PlaneController>();
        }
        //move
        controler.AircraftMove.Thrust.performed += MoveThrust;
        controler.AircraftMove.LR.performed += MoveLeftRight;
        controler.AircraftMove.UD.performed += MoveUPDown;

        //fire
        controler.Fire.MissileA.started += _ => airCraftControler.FireWeapons(0);
        controler.Fire.MissileB.started += _ => airCraftControler.FireWeapons(1);
        controler.Fire.MissileC.started += _ => airCraftControler.FireWeapons(2);
        controler.Fire.Gun.started += _ => airCraftControler.FireWeapons(3);
    }

    #region Controle
    private void MoveThrust(InputAction.CallbackContext _thrust)
    {
        float inputdata = _thrust.ReadValue<float>();
        speed = inputdata + 1;
    }
    private void MoveLeftRight(InputAction.CallbackContext _rightLeft)
    {
        float inputdata = _rightLeft.ReadValue<float>();
        if (inputdata > 0)
        {
            right = (inputdata - 1);
            left = 0;
        }
        else
        {
            left = inputdata + 1;
            right = 0;
        }
    }
    private void MoveUPDown(InputAction.CallbackContext upDown)
    {
        float inputdata = upDown.ReadValue<float>();
        if (inputdata > 0)
        {
            up = (inputdata - 1);
            down = 0;
        }
        else
        {
            down = inputdata + 1;
            up = 0;
        }
    }
    #endregion
}
