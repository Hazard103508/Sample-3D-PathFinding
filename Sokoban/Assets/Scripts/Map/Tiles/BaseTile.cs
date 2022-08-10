using UnityEngine;
using UnityEngine.Events;

namespace Map.Tiles
{
    public abstract class BaseTile : MonoBehaviour
    {
        #region Objects
        private bool _isHighlighted;
        #endregion

        #region Events
        [HideInInspector] public UnityEvent<Vector3> onSelected;
        [HideInInspector] public UnityEvent<Tiles.BaseTile> onMouseExit;
        [HideInInspector] public UnityEvent<Tiles.BaseTile> onMouseEnter;
        #endregion

        #region Properties
        public bool Highlighted
        {
            get => _isHighlighted;
            set
            {
                _isHighlighted = value;
                OnHighlighted(value);
            }
        }
        #endregion

        #region Unity Methods
        private void OnMouseEnter()
        {
            onMouseEnter.Invoke(this);
        }
        private void OnMouseExit()
        {
            onMouseExit.Invoke(this);
        }
        private void OnMouseDown()
        {
            onSelected.Invoke(this.transform.position);
        }
        #endregion

        #region Protected Methods
        protected virtual void OnHighlighted(bool isHighlighted)
        {
        }
        #endregion
    }
}