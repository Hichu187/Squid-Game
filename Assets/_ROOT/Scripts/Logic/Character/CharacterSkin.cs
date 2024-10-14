using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class CharacterSkin : MonoCached
    {
        [Title("Reference")]
        [SerializeField] private GameObject _objJetPack;
        [SerializeField] private GameObject[] _objShoes;

        public GameObject objJetPack { get { return _objJetPack; } }

        public GameObject[] objShoes { get { return _objShoes; } }
    }
}
