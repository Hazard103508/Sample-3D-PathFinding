using Extensions;
using Map.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Map
{
    public class Map : MonoBehaviour
    {
        #region Objects
        [SerializeField] private Character character;
        [SerializeField] private Data.Map mapData;
        [SerializeField] private Data.Tileset tileset;

        private PathFinding.PathFinder _pathFinder;
        private List<Box> _boxes;
        private Dictionary<Vector3, Tiles.BaseTile> _tiles;
        private List<Vector3Int> _path;
        private List<Tiles.BaseTile> _highlightTiles;
        private bool characterIsMoving;
        private Tiles.BaseTile _nextPathTile;

        [HideInInspector] public UnityEvent<Character> Loaded;
        #endregion

        #region Unity Methods
        private void Start()
        {
            Load_Map();
            //Load_Boxes();
            //Load_Targets();
            Load_Character();

            Loaded.Invoke(character);
        }
        private void Update()
        {

        }
        #endregion

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
            bool[,,] obstacleMatrix = new bool[this.mapData.size.width, this.mapData.size.height, this.mapData.size.depth];

            var _stairs = new Dictionary<Vector3Int, Tiles.Stair>();
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
                            var _tile = obj.GetComponent<Tiles.BaseTile>();
                            _tiles.Add(_tile.transform.position, _tile);

                            if (_tileData.type == Data.TileType.Stair)
                            {
                                var _position = new Vector3Int(x, y, z);
                                var _stair = obj.GetComponent<Tiles.Stair>();
                                _stair.entryPointA += _position;
                                _stair.entryPointB += _position;
                                _stairs.Add(_position, _stair);
                            }

                            _tile.onSelected.AddListener(OnTileSelected);
                            _tile.onMouseEnter.AddListener(onTileMouseEnter);
                            _tile.onMouseExit.AddListener(onTileMouseExit);

                            obstacleMatrix[x, y, z] = true;
                        }
                        else
                            obstacleMatrix[x, y, z] = y > 0 && !obstacleMatrix[x, y - 1, z]; // bloque las celdas que no tienen piso en el nivel inferior
                    }

            _pathFinder = new PathFinding.PathFinder(obstacleMatrix);
            _pathFinder.StairsLocations = _stairs;

            var _camera = GameObject.Find("CameraPivot");
            _camera.transform.position = new Vector3((float)this.mapData.size.width / 2, 0, (float)this.mapData.size.depth / 2);
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

                _pathFinder.SetObstacle(loc, true);
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
            character.transform.position += this.mapData.characterLocation.ToVector3();
        }
        #endregion

        #region Tiles Methods
        private void OnTileSelected(Vector3 tilePosition)
        {
            if (!characterIsMoving)
                StartCoroutine(Move_Character());
        }
        private void onTileMouseEnter(Tiles.BaseTile tile)
        {
            if (characterIsMoving)
            {
                _nextPathTile = tile;
                return;
            }

            Vector3Int targetLocation = tile.transform.position.ToVector3Int() + Vector3Int.up; // el tile seleccionado esta al nivel del suelo, se le aumenta un y+1 para validar que el persona pueda pararse sobre dicho tile
            _path = _pathFinder.FindPath(this.character.transform.position.ToVector3Int(), targetLocation);
            if (_path.Any())
                HighlightPath();

            if (!_path.Any())
            {
                _path = new List<Vector3Int> { targetLocation };
                HighlightPath();
                _path.Clear();
            }
        }
        private void onTileMouseExit(Tiles.BaseTile tile)
        {
            _nextPathTile = null;

            if (characterIsMoving)
                return;

            _highlightTiles.ForEach(tile =>
            {
                if (tile != null)
                    tile.Highlighted = false;
            });
            _highlightTiles = null;
        }
        private void HighlightPath()
        {
            if (_path == null || !_path.Any())
                return;

            _highlightTiles = new List<Tiles.BaseTile>();
            for (int i = 0; i < _path.Count; i++)
            {
                var location = _path[i] + Vector3.down;
                if (_tiles.ContainsKey(location))
                {
                    var _tile = _tiles[location];
                    _tile.Highlighted = true;
                    _highlightTiles.Add(_tile); // pinto los tiles del piso de abajo
                }
            }
        }
        #endregion

        #region Character Methods
        private IEnumerator Move_Character()
        {
            characterIsMoving = true;
            Vector3Int _lastLocation = this.character.transform.position.ToVector3Int();
            while (_path.Any())
            {
                var _characterPosition = _lastLocation;
                var _location = _path[0];
                var _direction = _location - _characterPosition;

                bool _isOnStair = _pathFinder.StairsLocations.ContainsKey(_characterPosition + Vector3Int.down);
                bool _isNextStair = _pathFinder.StairsLocations.ContainsKey(_location + Vector3Int.down);

                this.character.Move(_direction, _isOnStair, _isNextStair);

                _path.Remove(_location);
                yield return new WaitUntil(() => !this.character.IsMoving);

                this.character.transform.position = new Vector3(Mathf.Round(this.character.transform.position.x), (float)Math.Round(this.character.transform.position.y, 1), Mathf.Round(this.character.transform.position.z));
                _highlightTiles[0].Highlighted = false;
                _highlightTiles.RemoveAt(0);

                _lastLocation = _location;

            }
            characterIsMoving = false;

            if (_nextPathTile != null)
                onTileMouseEnter(_nextPathTile);
        }
        #endregion
    }
}

