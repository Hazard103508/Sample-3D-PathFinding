using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Map.Items
{
    public class Character : BaseItem
    {
        #region Objects
        private Animator animator;
        #endregion

        #region Unity Methods
        private void Start()
        {
            animator = GetComponent<Animator>();
        }
        #endregion

        #region Public Methods
        public void Move(Vector3Int direction, bool isOnStair, bool nextIsStair)
        {
            base.IsMoving = true;

            float y =
                direction.y == -1 && isOnStair && !nextIsStair ? -3 : // termina de bajar la escalera
                direction.y == -1 && isOnStair && nextIsStair ? -2 : // sigue bajando la escalera
                direction.y == 0 && !isOnStair && nextIsStair ? -1 : // comienza a bajar la escalera
                direction.y == 1 && !isOnStair && nextIsStair ? 1 : // comienza a subur la escalera
                direction.y == 1 && isOnStair && nextIsStair ? 2 : // sigue subiendo la escalera
                direction.y == 0 && isOnStair && !nextIsStair ? 3 : // termina a subur la escalera
                0;

            animator.SetFloat("x", direction.x);
            animator.SetFloat("z", direction.z);
            animator.SetFloat("y", y);

            animator.SetTrigger("Walk");
        }
        public void Move_Competed() => base.IsMoving = false;
        #endregion
    }
}
