using System;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class ColorBlock_ColorConfig
    {
        [SerializeField] private Color _color;
        [SerializeField] private string _name;
        [SerializeField] private Material _material;

        public Color color { get { return _color; } }
        public string name { get { return _name; } }
        public Material material { get { return _material; } }
    }
}
