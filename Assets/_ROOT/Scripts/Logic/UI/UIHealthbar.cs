using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UIHealthbar : MonoBehaviour
    {
        [SerializeField] Image fill;

        float fullHp;

        float curHp;
        public void InitHealthBar(int full)
        {

            fill.fillAmount = 1;
            fullHp = full;
            curHp = fullHp;
        }

        public void UpdateHealthBar(int hp)
        {
            fill.DOFillAmount((float)hp / fullHp, 0.1f);
        }
    }
}
