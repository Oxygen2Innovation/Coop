using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tst : MonoBehaviour
{
    public float Speed = 10;

    void FixedUpdate()
    {
        transform.localPosition += Speed * Time.fixedDeltaTime * transform.forward;
    }
}
