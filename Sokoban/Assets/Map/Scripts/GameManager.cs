using Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class GameManager : MonoBehaviour
    {
        #region Objects
        [SerializeField] private Map map;
        #endregion

        #region Properties
        /// <summary>
        /// Personaje jugable
        /// </summary>
        private Character MainCharacter { get; set; }
        /// <summary>
        /// Estado del juego en curso
        /// </summary>
        private GameStates State { get; set; }
        #endregion

        #region Unity Methods
        private void Awake()
        {
            this.State = GameStates.Loading;
            map.Loaded.AddListener(OnMapLoaded);
        }
        private void Update()
        {
            Move_Character();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Al cargarse el mapa actualiza el estado del juego
        /// </summary>
        /// <param name="character"></param>
        private void OnMapLoaded(Character character)
        {
            this.MainCharacter = character;
            this.MainCharacter.Moved.AddListener(OnCharacterMoved);

            State = GameStates.Idle;
        }
        /// <summary>
        /// Al completarse el movimiento del personaje cambia el estado del juego
        /// </summary>
        private void OnCharacterMoved()
        {
            State = GameStates.Idle;
        }
        private void Move_Character()
        {
            if (this.State == GameStates.Idle)
            {
                Vector3 direction =
                    Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) ? Vector3.right :
                    Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A) ? Vector3.left :
                    Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W) ? Vector3.forward :
                    Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S) ? Vector3.back :
                    Vector3.zero;

                if (direction == Vector3.zero)
                    return;

                var position = MainCharacter.transform.position + direction;
                if (this.map.IsEmptyPosition(position.ToVector3Int()))
                {
                    this.State = GameStates.Moving;
                    MainCharacter.Move(direction);
                    return;
                }

                var box = this.map.GetBox(position);
                position += direction;
                if (box != null && this.map.IsEmptyPosition(position.ToVector3Int()))
                {
                    this.State = GameStates.Moving;
                    MainCharacter.Move(direction);
                    box.Move(direction);
                    return;
                }
            }
        }

        #endregion

        #region Structures
        private enum GameStates
        {
            Loading,
            Idle,
            Moving
        }
        #endregion
    }
}