using LFramework;
using UnityEngine;

namespace Game
{
    public class TowerOfHell_Wall : MonoBehaviour
    {
        public void SetHeight(float height)
        {
            Transform target = transform.GetChild(0);

            MeshFilter renderer = target.GetComponent<MeshFilter>();

            target.SetScaleY(height / renderer.sharedMesh.bounds.size.y);
        }
    }
}
