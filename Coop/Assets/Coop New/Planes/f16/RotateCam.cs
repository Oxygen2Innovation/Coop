using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
public class RotateCam : MonoBehaviour
{
    public GameObject plane;
    public float rotSpeed;
    public Animator takeOff_Landing;
    public bool isTakeOff = false;
    public bool isLanding = false;

    void Start(){
        takeOff_Landing = plane.GetComponent<Animator>();
    }
    void FixedUpdate(){
        transform.Rotate(rotSpeed*Time.deltaTime*Vector3.up);
        CheckAnimation();
    }
    void CheckAnimation()
    {
        if(isTakeOff){
            isTakeOff = !isTakeOff;
            takeOff_Landing.SetTrigger("TakeOff");
        }
        if(isLanding)
        {
            isLanding = !isLanding;
            takeOff_Landing.SetTrigger("Landing");
        }
    }
}
