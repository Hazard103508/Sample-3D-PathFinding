using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Map.PathFinding
{
    /// <summary>
    /// Nodo utilizado en calculo de recorrido
    /// </summary>
    public class SearchNode
    {
        #region Constructor
        public SearchNode(Vector3Int position, float cost, float pathCost, SearchNode next)
        {
            this.Position = position;
            this.Cost = cost;
            this.PathCost = pathCost;
            this.Next = next;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Posicion del nodo dentro de la grilla
        /// </summary>
        public Vector3Int Position { get; set; }
        public float Cost { get; set; }
        public float PathCost { get; set; }
        public SearchNode Next { get; set; }
        public SearchNode NextListElem { get; set; }
        #endregion
    }
}
