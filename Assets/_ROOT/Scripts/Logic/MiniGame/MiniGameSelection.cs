using LFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class MiniGameSelection : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Transform _itemRoot;

        [Title("Config")]
        [SerializeField] private GameObject _itemPrefab;

        private void Start()
        {
            if (MiniGameStatic.current != null)
                LogHelper.ExitMiniGame(MiniGameStatic.current);

            MiniGameStatic.current = null;

            for (int i = 0; i < FactoryMiniGame.configs.Length; i++)
            {
                MiniGameSelectionItem item = _itemPrefab.Create(_itemRoot, false).GetComponent<MiniGameSelectionItem>();

                item.Construct(FactoryMiniGame.configs[i]);
            }
        }
    }
}
