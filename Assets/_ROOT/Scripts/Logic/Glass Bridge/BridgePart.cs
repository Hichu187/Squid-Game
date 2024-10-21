using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BridgePart : MonoBehaviour
    {
        [SerializeField] bool isCollider;
        [SerializeField] Renderer renderer;

        [SerializeField] Material green;
        public bool isBreak;

        [Button]
        public void SetCollider()
        {
            isCollider = true;
            this.GetComponent<Collider>().isTrigger = false;
        }

        public void Hint()
        {
            StartCoroutine(ChangeMaterial());
        }
        IEnumerator ChangeMaterial()
        {
            Color initialColor = renderer.material.color;
            Color targetColor = Color.green;
            targetColor.a = 0.75f;

            float duration = 0.75f;
            float elapsedTime = 0f;

            renderer.material.color = targetColor;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);

                renderer.material.color = Color.Lerp(targetColor, initialColor, t);

                yield return null;
            }
            renderer.material.color = initialColor;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Break();
            }
        }

        public void Break()
        {
            renderer.gameObject.SetActive(false);
            isBreak = true;
        }
    }
}
