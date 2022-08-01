using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

namespace Map.PathFinding
{
    public class PathFinder
    {
        #region Objects
        private Directions[] surrounding;
        #endregion

        #region Constructor
        public PathFinder(bool[,,] matrix)
        {
            this.Matrix = matrix;
            this.Grid_Size = new Vector3Int(matrix.GetLength(0), matrix.GetLength(1), matrix.GetLength(2));

            //Direcciones que puede moverse el personaje
            surrounding = new Directions[]
            {
                Directions.Back,
                Directions.Forward,
                Directions.Left,
                Directions.Right,
            };
        }
        #endregion

        #region Parameters
        /// <summary>
        /// Matriz con las celdas a analizar
        /// </summary>
        public bool[,,] Matrix { get; set; }
        /// <summary>
        /// Tamaño de la grilla a analizar
        /// </summary>
        private Vector3Int Grid_Size { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// metodo que obtiene el mejor camino para llegar de un punto a otro
        /// </summary>
        /// <param name="start">Punto de partida del recorrido</param>
        /// <param name="end">Punto final del recorrido</param>
        /// <returns></returns>        
        public List<Vector3> FindPath(Vector3Int start, Vector3Int end)
        {
            var _path = new List<Vector3>();

            if (IsTileBlocked(end)) // si el punto destino esta bloqueado no analiza el recorrido
                return _path;

            if (end == start)
                return _path; // si el punto de partida es igual al punto destino no se analiza el recorrido

            bool[,,] lstBlock = new bool[Grid_Size.x, Grid_Size.y, Grid_Size.z]; // crea una matriz del mismo tamaño que la original para macar las celdas analizadas

            var node = FindPathReversed(start, end); // realiza el analisis de recorrido

            if (node != null)
                while (node != null)
                {
                    _path.Insert(0, node.Position); // recorre los nodos anidados para obtener la lista final de coordenadas
                    node = node.Next;
                }

            if (_path.Any())
                _path.RemoveAt(0); // quita el punto inicial (punto donde se encuentra el personaje)

            return _path;
        }
        /// <summary>
        /// metodo que obtiene el mejor camino para llegar de un punto a otro
        /// </summary>
        /// <param name="start">Punto de partida del recorrido</param>
        /// <param name="end">Punto final del recorrido</param>
        /// <returns></returns>  
        private SearchNode FindPathReversed(Vector3Int start, Vector3Int end)
        {
            SearchNode startNode = new SearchNode(start, 0, 0, null); // crea el nodo del punto analizado
            bool[,,] lstBlock = new bool[Grid_Size.x, Grid_Size.y, Grid_Size.z]; // crea una matriz del mismo tamaño que la original para macar las celdas analizadas

            MinHeap openList = new MinHeap(); // clase que analiza los nodos cercanos y determina cual posee el menor costo hacia el destino
            openList.Add(startNode);

            while (openList.HasNext())
            {
                SearchNode current = openList.ExtractFirst();

                float distance = Vector3.Distance(current.Position, end);
                if (distance == 1) // si la distancia es 1, se asume que el destino se encuentra en una celda a la izquierda, derecha, arriba o abajo
                    return new SearchNode(end, current.PathCost + distance, current.Cost + distance, current);

                // recorre todas las direcciones disponibles
                for (int i = 0; i < surrounding.Length; i++)
                {
                    Directions surr = surrounding[i];
                    Vector3Int location = current.Position + surr.Direction;  //new Vector3(current.Position.X + surr.Point.X, current.Position.Y + surr.Point.Y);

                    if (IsOutofRange(location) || lstBlock[location.x, location.y, location.z])
                        continue; // si los destinos estan fuera de rango los continua con el analisis de las demas celdas cercanas

                    // valida si la posicion destino esta bloqueada
                    bool _detinoBlock = IsTileBlocked(location);
                    bool _side2 = IsTileBlocked(new Vector3Int(current.Position.x, current.Position.y + surr.Direction.y)); // valida las celdas laterales en caso de realizar un desplazamiento en diagonal
                    bool _side1 = IsTileBlocked(new Vector3Int(current.Position.x + surr.Direction.x, current.Position.y));

                    if (!_detinoBlock && !_side2 && !_side1)
                    {
                        float pathCost = current.PathCost + surr.Cost;
                        float cost = pathCost + Vector3.Distance(location, end);
                        SearchNode node = new SearchNode(location, cost, pathCost, current);
                        openList.Add(node);

                        lstBlock[location.x, location.y, location.z] = true; // deshabilita las celdas ya recorridas
                    }

                    if (_detinoBlock)
                        lstBlock[location.x, location.y, location.z] = true; // deshabilita las celdas ya recorridas
                }
            }
            return null; //no path found
        }
        /// <summary>
        /// Determina si la coordenada indicada esta bloqueada para el desplazamiento del elemento
        /// </summary>
        /// <param name="location">Coordenada a validar</param>
        /// <returns></returns>
        private bool IsTileBlocked(Vector3Int location)
        {
            return IsOutofRange(location) || this.Matrix[location.x, location.y, location.z];
        }
        /// <summary>
        /// Determina si la coordenada indicada esta fuera de rango
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private bool IsOutofRange(Vector3Int location)
        {
            return
                location.x < 0 ||
                location.x >= Grid_Size.x ||
                location.y < 0 ||
                location.y >= Grid_Size.y ||
                location.z < 0 ||
                location.z >= Grid_Size.z;
        }
        #endregion

        internal class Directions
        {
            private Directions(Vector3Int vector)
            {
                Direction = new Vector3Int(vector.x, vector.y, vector.z);
                Cost = (float)Math.Sqrt(Math.Abs(vector.x) + Math.Abs(vector.y)); // aplico pitagoras para obtener la distancia entre 2 puntos (valor absoluto en este caso)
            }

            public Vector3Int Direction;
            public float Cost;

            public static Directions Left { get => new Directions(Vector3Int.left); }
            public static Directions Right { get => new Directions(Vector3Int.right); }
            public static Directions Forward { get => new Directions(Vector3Int.forward); }
            public static Directions Back { get => new Directions(Vector3Int.back); }
            public static Directions Up { get => new Directions(Vector3Int.up); }
            public static Directions Down { get => new Directions(Vector3Int.down); }
        }
    }
}
