using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Map
{
    public class Map : MonoBehaviour
    {
        #region Objects
        [SerializeField] private Prefabs prefabs;
        private Character character;
        private List<Box> boxes;

        public MapLoadEvent Loaded = new MapLoadEvent();
        #endregion

        #region Properties
        public Data.Map Data { get; set; }
        #endregion

        void Start()
        {
            Data = new Data.Map();

            Load_Map();
            Load_Boxes();
            Load_Targets();
            Load_Characters();

            Loaded.Invoke(character);
        }

        // Update is called once per frame
        void Update()
        {

        }

        #region Public Methods
        public bool IsEmptyPosition(Vector3 location)
        {
            return
                location.x >= 0 &&
                location.x < this.Data.Width &&
                location.z >= 0 &&
                location.z < this.Data.Depth &&
                !boxes.Any(b => b.transform.position.x == location.x && b.transform.position.z == location.z);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Carga el mapa
        /// </summary>
        private void Load_Map()
        {
            GameObject folder = new GameObject("Tiles");
            folder.transform.SetParent(transform);

            for (int h = 0; h < this.Data.Height; h++)
                for (int w = 0; w < this.Data.Width; w++)
                    for (int d = 0; d < this.Data.Depth; d++)
                    {
                        int tileIndex = this.Data.Tiles[h, w, d];
                        if (tileIndex != 0)
                        {
                            GameObject tile = Instantiate(prefabs.tiles[tileIndex - 1], folder.transform);
                            tile.transform.position = new Vector3(w, h, d);
                        }
                    }
        }
        /// <summary>
        /// Carga las cajas en el mapa
        /// </summary>
        private void Load_Boxes()
        {
            boxes = new List<Box>();
            GameObject folder = new GameObject("Boxes");
            folder.transform.SetParent(transform);

            Array.ForEach(this.Data.BoxLocations, loc =>
            {
                GameObject obj = Instantiate(prefabs.box, folder.transform);
                obj.transform.position = new Vector3(loc.x + obj.transform.position.x, loc.y + obj.transform.position.y, loc.z + obj.transform.position.z);
                boxes.Add(obj.GetComponent<Box>());
            });
        }
        /// <summary>
        /// Carga los objetivos del mapa
        /// </summary>
        private void Load_Targets()
        {
            GameObject folder = new GameObject("Targets");
            folder.transform.SetParent(transform);

            Array.ForEach(this.Data.TargetLocations, loc =>
            {
                GameObject obj = Instantiate(prefabs.target, folder.transform);
                obj.transform.position = new Vector3(loc.x + obj.transform.position.x, loc.y + obj.transform.position.y, loc.z + obj.transform.position.z);
            });
        }
        /// <summary>
        /// Carga los objetivos del mapa
        /// </summary>
        private void Load_Characters()
        {
            GameObject folder = new GameObject("Characters");
            folder.transform.SetParent(transform);

            GameObject obj = Instantiate(prefabs.character, folder.transform);
            obj.transform.position = new Vector3(this.Data.CharacterLocations.x + obj.transform.position.x, this.Data.CharacterLocations.y + obj.transform.position.y, this.Data.CharacterLocations.z + obj.transform.position.z);
            character = obj.GetComponent<Character>();

        }
        #endregion

        #region Structures
        [Serializable]
        public class Prefabs
        {
            public GameObject[] tiles;
            public GameObject box;
            public GameObject target;
            public GameObject character;
        }
        public class MapLoadEvent : UnityEvent<Character>
        {
        }
        #endregion
    }
}
