using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map.Tiles
{
    public class Stair : BaseTile
    {
        public Vector3Int entryPointA;
        public Vector3Int entryPointB;

        // ---------------------------TESTING
        public GameObject[] selectedObject;
        public Color colorNormal;
        public Color colorSelected;
        // ---------------------------TESTING

        protected override void OnHighlighted(bool isHighlighted)
        {
            // TESTING
            Array.ForEach(selectedObject, x => x.GetComponent<Renderer>().material.color = isHighlighted ? colorSelected : colorNormal);
        }
    }
}
