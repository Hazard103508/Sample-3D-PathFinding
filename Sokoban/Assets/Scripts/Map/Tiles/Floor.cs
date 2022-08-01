using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map.Tiles
{
    public class Floor : BaseTile
    {
        // ---------------------------TESTING
        public GameObject grass;
        public Color colorNormal;
        public Color colorSelected;
        // ---------------------------TESTING

        protected override void OnHighlighted(bool isHighlighted)
        {
            if (grass) // -- TESTING...
                grass.GetComponent<Renderer>().material.color = isHighlighted ? colorSelected : colorNormal;
        }
    }
}