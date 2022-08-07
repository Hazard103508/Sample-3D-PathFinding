using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class FollowPlayer : MonoBehaviour
    {
        public global::Map.Items.Character character;

        void Update()
        {
            transform.position = character.transform.position;
            Rotate_Camera();
        }

        private void Rotate_Camera()
        {
            float _rotation = 90;
            if (Input.GetKey(KeyCode.D))
                this.transform.Rotate(Vector3.up * _rotation * Time.deltaTime, Space.World);
            else if (Input.GetKey(KeyCode.A))
                this.transform.Rotate(Vector3.up * -_rotation * Time.deltaTime, Space.World);

        }
    }
}