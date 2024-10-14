using LFramework;
using UnityEngine;

namespace Game
{
    public class BGMStarter : MonoBehaviour
    {
        [SerializeField] private AudioConfig _bgm;

        private void Start()
        {
            BGMHelper.Play(_bgm);
        }
    }
}
