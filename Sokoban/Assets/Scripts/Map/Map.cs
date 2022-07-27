using Extensions;
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
        public Data.Map mapData;
        [SerializeField] private Prefabs prefabs;

        private Character character;
        private List<Box> boxes;

        [HideInInspector] public MapLoadEvent Loaded = new MapLoadEvent();
        #endregion

        #region Properties
        #endregion

        void Start()
        {
            Load_Map();
            Load_Boxes();
            Load_Targets();
            Load_Character();

            Loaded.Invoke(character);
        }

        // Update is called once per frame
        void Update()
        {

        }

        #region Public Methods
        public bool IsEmptyPosition(Vector3Int location)
        {
            return
                location.x >= 0 &&
                location.x < this.mapData.size.width &&
                location.z >= 0 &&
                location.z < this.mapData.size.depth &&
                !boxes.Any(b => b.transform.position.x == location.x && b.transform.position.z == location.z) &&
                this.mapData.getTile(location.x, location.y, location.z) == 0;
        }
        public Box GetBox(Vector3 location)
        {
            return boxes.FirstOrDefault(b => b.transform.position.x == location.x && b.transform.position.z == location.z);
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

            for (int y = 0; y < this.mapData.size.height; y++)
                for (int x = 0; x < this.mapData.size.width; x++)
                    for (int z = 0; z < this.mapData.size.depth; z++)
                    {
                        int tileIndex = this.mapData.getTile(x, y, z);
                        if (tileIndex != 0)
                        {
                            GameObject tile = Instantiate(prefabs.tiles[tileIndex - 1], folder.transform);
                            tile.transform.position = new Vector3(x, y, z);
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

            Array.ForEach(this.mapData.boxLocations, loc =>
            {
                GameObject obj = Instantiate(prefabs.box, folder.transform);
                obj.transform.position += loc.ToVector3();
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

            Array.ForEach(this.mapData.targetLocations, loc =>
            {
                GameObject obj = Instantiate(prefabs.target, folder.transform);
                obj.transform.position += loc.ToVector3();
            });
        }
        /// <summary>
        /// Carga los objetivos del mapa
        /// </summary>
        private void Load_Character()
        {
            GameObject folder = new GameObject("Characters");
            folder.transform.SetParent(transform);

            GameObject obj = Instantiate(prefabs.character, folder.transform);
            obj.transform.position += this.mapData.characterLocation.ToVector3();
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

