using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class BladeBall_UI_SkillButton : MonoBehaviour
    {
        Image _image;

        private void Awake()
        {
            StaticBus<Event_BladeBall_Skill>.Subscribe(Skill);
            _image = GetComponent<Image>();
        }

        private void OnDestroy()
        {
            StaticBus<Event_BladeBall_Skill>.Unsubscribe(Skill);
        }

        private void Start()
        {
            
        }

        IEnumerator ReduceFillAmountOverTime(float duration)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                _image.fillAmount = Mathf.Lerp(1f, 0f, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            _image.fillAmount = 0f;
        }

        void Skill(Event_BladeBall_Skill e)
        {
            _image.fillAmount = 1;
            StartCoroutine(ReduceFillAmountOverTime(e.cooldown));
        }
    }
}
