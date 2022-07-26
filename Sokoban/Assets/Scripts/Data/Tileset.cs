using System;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Tiles", menuName = "Data/Tileset", order = 2)]
    public class Tileset : ScriptableObject
    {
        public GameObject box;
        public GameObject target;
        public Tile[] tiles;
    }
    [Serializable]
    public class Tile
    {
        public int id;
        public TileType type;
        public GameObject prefab;
    }
    public enum TileType
    {
        Block,
        Stair
    }
}