using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Map.Items
{
    public class BaseItem : MonoBehaviour
    {
        #region Properties
        public bool IsMoving { get; protected set; }
        #endregion

        //#region Events
        //public UnityEvent Moved;
        //#endregion
        //
        //#region Public Methods
        ///// <summary>
        ///// Mueve el personaje en la direccion indicada
        ///// </summary>
        ///// <param name="direction">Direccion a desplazar</param>
        //public void Move(Vector3 direction)
        //{
        //    StartCoroutine(Move_CO(direction));
        //}
        //#endregion
        //
        //#region Private Methods
        ///// <summary>
        ///// Mueve el personaje en la direccion indicada
        ///// </summary>
        ///// <param name="direction">Direccion a desplazar</param>
        //private IEnumerator Move_CO(Vector3 direction)
        //{
        //    float timer = 0;
        //    float totalTime = 0.5f;
        //    Vector3 startPos = transform.position;
        //
        //    while (timer < totalTime)
        //    {
        //        timer += Time.deltaTime;
        //
        //        float factor = Mathf.Min(timer / totalTime, 1f);
        //        transform.position = startPos + direction * factor;
        //
        //        yield return null;
        //    }
        //
        //    Moved.Invoke(); // desencadena el evento indicando que termino de caminar
        //}
        //#endregion
    }
}