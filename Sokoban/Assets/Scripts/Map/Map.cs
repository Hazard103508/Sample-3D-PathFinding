using Extensions;
using Map.Items;
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
        [SerializeField] private Character player;
        [SerializeField] private Data.Map mapData;
        [SerializeField] private Data.Tileset tileset;

        private PathFinding.PathFinder _pathFinder;
        private List<Box> _boxes;
        private Dictionary<Vector3, Tiles.BaseTile> _tiles;
        private Tiles.BaseTile[] _highlightTiles;

        [HideInInspector] public UnityEvent<Character> Loaded;
        #endregion

        void Start()
        {
            Load_Map();
            Load_Boxes();
            Load_Targets();
            Load_Character();

            Loaded.Invoke(player);
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
                !_boxes.Any(b => b.transform.position.x == location.x && b.transform.position.z == location.z) &&
                this.mapData.getTile(location.x, location.y, location.z) == 0;
        }
        public Box GetBox(Vector3 location)
        {
            return _boxes.FirstOrDefault(b => b.transform.position.x == location.x && b.transform.position.z == location.z);
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

            _tiles = new Dictionary<Vector3, Tiles.BaseTile>();
            bool[,,] obstacleMatrix = new bool[this.mapData.size.width, this.mapData.size.height,  this.mapData.size.depth];

            for (int y = 0; y < this.mapData.size.height; y++)
                for (int x = 0; x < this.mapData.size.width; x++)
                    for (int z = 0; z < this.mapData.size.depth; z++)
                    {
                        int tileIndex = this.mapData.getTile(x, y, z);
                        if (tileIndex != 0)
                        {
                            var _tileData = tileset.tiles[tileIndex - 1];
                            GameObject obj = Instantiate(_tileData.prefab, folder.transform);
                            obj.transform.position = new Vector3(x, y, z);
                            obj.transform.Rotate(_tileData.rotation);
                            var _tile = obj.GetComponent<Tiles.BaseTile>();
                            _tiles.Add(_tile.transform.position, _tile);
                            obstacleMatrix[(int)_tile.transform.position.x, (int)_tile.transform.position.y, (int)_tile.transform.position.z] = true;

                            //_tile.onSelected.AddListener(OnTileSelected);
                            _tile.onMouseEnter.AddListener(onTileMouseEnter);
                            _tile.onMouseExit.AddListener(onTileMouseExit);
                        }
                    }

            _pathFinder = new PathFinding.PathFinder(obstacleMatrix);
        }
        /// <summary>
        /// Carga las cajas en el mapa
        /// </summary>
        private void Load_Boxes()
        {
            _boxes = new List<Box>();
            GameObject folder = new GameObject("Boxes");
            folder.transform.SetParent(transform);

            Array.ForEach(this.mapData.boxLocations, loc =>
            {
                GameObject obj = Instantiate(tileset.box, folder.transform);
                obj.transform.position += loc.ToVector3();
                _boxes.Add(obj.GetComponent<Box>());
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
                GameObject obj = Instantiate(tileset.target, folder.transform);
                obj.transform.position += loc.ToVector3();
            });
        }
        /// <summary>
        /// Carga los objetivos del mapa
        /// </summary>
        private void Load_Character()
        {
            player.transform.position += this.mapData.characterLocation.ToVector3();
        }
        #endregion

        #region Tiles Methods
        private void OnTileSelected(Vector3 tilePosition)
        {
            //var targetLocation = tilePosition + Vector3.up;
            //var _path = _pathFinder.FindPath(this.player.transform.position.ToVector3Int(), targetLocation.ToVector3Int());
            ////_path.ForEach(location => print(location)); //_tiles[location].Highlighted = true);
            //_path.ForEach(location => _tiles[location + Vector3.down].Highlighted = true);
        }
        private void onTileMouseEnter(Tiles.BaseTile tile)
        {
            var targetLocation = tile.transform.position + Vector3.up; // el tile seleccionado esta al nivel del suelo, se le aumenta un y+1 para validar que el persona pueda pararse sobre dicho tile
            var _paths = _pathFinder.FindPath(this.player.transform.position.ToVector3Int(), targetLocation.ToVector3Int());
            HighlightPath(_paths);
        }
        private void onTileMouseExit(Tiles.BaseTile tile)
        {
            Array.ForEach(_highlightTiles, tile => tile.Highlighted = false);
            _highlightTiles = null;
        }
        private void HighlightPath(List<Vector3> paths)
        {
            _highlightTiles = new Tiles.BaseTile[paths.Count];
            for (int i = 0; i < _highlightTiles.Length; i++)
            {
                _highlightTiles[i] = _tiles[paths[i] + Vector3.down];
                _highlightTiles[i].Highlighted = true;
            }
        }
        #endregion
    }
}

