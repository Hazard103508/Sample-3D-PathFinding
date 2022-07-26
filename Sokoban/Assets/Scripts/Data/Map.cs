using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class Map
    {
        public Map()
        {
            this.Height = 1;
            this.Width = 10;
            this.Depth = 6;

            this.Tiles = new int[this.Height, this.Width, this.Depth];

            for (int x = 0; x < this.Width; x++)
                for (int y = 0; y < this.Depth; y++)
                {
                    Tiles[0, x, y] = 1;
                }

            BoxLocations = new Vector3Int[]
            {
                new Vector3Int(1,1,1),
                new Vector3Int(8,1,4),
            };

            TargetLocations = new Vector3Int[]
             {
                new Vector3Int(0,1,0),
                new Vector3Int(9,1,0),
             };

            CharacterLocations = new Vector3Int(5, 1, 0);
        }

        public int Height { get; set; }
        public int Width { get; set; }
        public int Depth { get; set; }
        public int[,,] Tiles { get; set; }

        public Vector3Int[] BoxLocations { get; set; }
        public Vector3Int[] TargetLocations { get; set; }
        public Vector3Int CharacterLocations { get; set; }
    }
}