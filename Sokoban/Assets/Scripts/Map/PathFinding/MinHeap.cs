using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.PathFinding
{
    public class MinHeap
    {
        // Nodo inicial de la busqueda
        private SearchNode listHead;

        public bool HasNext()
        {
            return listHead != null;
        }

        public void Add(SearchNode item)
        {
            if (listHead == null)
                listHead = item; // se asigna
            else if (listHead.Next == null && item.Cost <= listHead.Cost)
            {
                item.NextListElem = listHead;
                listHead = item;
            }
            else
            {
                // Nodos opconal de recorrido
                SearchNode ptr = listHead;
                while (ptr.NextListElem != null && ptr.NextListElem.Cost < item.Cost)
                    ptr = ptr.NextListElem;
                item.NextListElem = ptr.NextListElem;
                ptr.NextListElem = item;
            }
        }

        public SearchNode ExtractFirst()
        {
            SearchNode result = listHead;
            listHead = listHead.NextListElem;
            return result;
        }
    }
}
