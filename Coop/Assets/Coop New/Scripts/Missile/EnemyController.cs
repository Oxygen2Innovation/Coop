using Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 10;
    public Transform target;
    public GameObject radar;
    EnemyRadar radarEnemy;
    public bool isActive = true;
    private void Start()
    {
        radar = transform.Find("Radar").gameObject;
        radarEnemy = radar.GetComponent<EnemyRadar>();
        radarEnemy.controller = this;
        radar.SetActive(true);
    }
    void FixedUpdate()
    {
        if(target == null)
        {
            transform.localPosition += speed * Time.deltaTime * transform.forward;
            return;
        }
        transform.localPosition = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
        transform.LookAt(target);
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
