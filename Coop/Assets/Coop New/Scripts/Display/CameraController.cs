using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Display
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [Range(0, 1)] public float CameraSpring = 0.96f;
        public Camera Camera;
        public List<GameObject> targets;
        int i = 0;
        private void Start()
        {
            Camera = GetComponent<Camera>();
        }
        private void FixedUpdate()
        {
            if (targets[i] == null)
            {
                if(i >= targets.Count-1)
                {
                    return;
                }
                else
                {
                    i++;
                }
            }
            Vector3 cameraTargetPosition = targets[i].transform.position + targets[i].transform.forward * -8f + new Vector3(0f, 3f, 0f);
            var cameraTransform = Camera.transform;

            cameraTransform.position = cameraTransform.position * CameraSpring + cameraTargetPosition * (1 - CameraSpring);
            Camera.transform.LookAt(targets[i].transform);
        }
    }
}