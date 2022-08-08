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
        public void Move(Vector3Int direction, bool isOnStair, bool nextIsDownStair, bool nextIsUpStair)
        {
            base.IsMoving = true;

            animator.SetFloat("x", direction.x);
            animator.SetFloat("z", direction.z);

            string trigger =
                !isOnStair && nextIsDownStair ? "MoveStartDown" :
                isOnStair && !nextIsDownStair ? "MoveEndDown" :
                "MoveNormal";

            animator.SetTrigger(trigger);

        }
        public void Move_Competed() => base.IsMoving = false;
        #endregion
    }
}
