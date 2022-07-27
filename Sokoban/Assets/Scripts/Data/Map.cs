using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Map", menuName = "Data/Map", order = 1)]
    public class Map : ScriptableObject
    {
        public Map()
        {
            size = new Size();
        }

        public Size size;
        public int[] tiles;
        public Vector3Int[] boxLocations;
        public Vector3Int[] targetLocations;
        public Vector3Int characterLocation;

        public int getTile(int x, int y, int z)
        {
            int index = (z * size.width) + x + (size.width * size.depth * y);
            return tiles[index];
        }
    }
}