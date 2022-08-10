using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class CameraHandler : MonoBehaviour
    {
        private float speed = 3.5f;

        void Update()
        {
            if (Input.GetMouseButton(1))
                transform.Rotate(Vector3.up * -Input.GetAxis("Mouse X") * speed, Space.World);
        }
    }
}