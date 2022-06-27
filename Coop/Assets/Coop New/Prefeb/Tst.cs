using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tst : MonoBehaviour
{
    public float Speed = 10;

    void Update()
    {
        transform.position += Speed * Time.deltaTime * transform.forward;
    }
}
