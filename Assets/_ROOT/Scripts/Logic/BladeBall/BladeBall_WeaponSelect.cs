using DG.Tweening;
using LFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class BladeBall_WeaponSelect : MonoBehaviour
    {
        [SerializeField] UIPointerClick _btn_nothanks;
        [SerializeField] UIPointerClick _btn_option1;
        [SerializeField] UIPointerClick _btn_option2;
        [SerializeField] List<Sprite> sprites;
        [SerializeField] int a, b;

        [SerializeField] private AdsPlacement _adsPlacement;
        private BladeBall_Manager _manager;
        private void OnEnable()
        {
            _btn_nothanks.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack).ChangeStartValue(Vector3.zero).SetDelay(3f);
            _btn_option1.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack).ChangeStartValue(Vector3.zero).SetDelay(1f);
            _btn_option2.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack).ChangeStartValue(Vector3.zero).SetDelay(1f);
        }

        private void Awake()
        {
            _manager = FindObjectOfType<BladeBall_Manager>();

            Init();

            _btn_nothanks.eventDown += NoThankClick;
            _btn_option1.eventDown += SelectOption1;
            _btn_option2.eventDown += SelectOption2;
        }
        private void Init()
        {
            GenerateTwoDistinctNumbers(out a,out b);

            _btn_option1.transform.GetChild(1).GetComponent<Image>().sprite = sprites[a];   
            _btn_option2.transform.GetChild(1).GetComponent<Image>().sprite = sprites[b];
        }

        void SelectOption1()
        {
            AdsHelper.ShowRewarded((isSuccess) =>
            {
                if (!isSuccess)
                    return;

                StaticBus<Event_BladeBall_SelectWeapon>.Post(new Event_BladeBall_SelectWeapon(a));
                _manager.StartGame();
                PopOut();
            }, _adsPlacement);

        }
        void SelectOption2()
        {
            AdsHelper.ShowRewarded((isSuccess) =>
            {
                if (!isSuccess)
                    return;

                StaticBus<Event_BladeBall_SelectWeapon>.Post(new Event_BladeBall_SelectWeapon(b));
                _manager.StartGame();
                PopOut();
            }, _adsPlacement);

        }
        void GenerateTwoDistinctNumbers(out int num1, out int num2)
        {
            num1 = Random.Range(1, 8);
            do
            {
                num2 = Random.Range(1, 8);
            } while (num2 == num1);
        }

        void NoThankClick()
        {
            PopOut();
            _manager.StartGame();
            AdsHelper.ShowInterstitial(AdsPlacement.BladeBall_Select_Weapon_Cancel);
        }

        void PopOut()
        {
            _btn_nothanks.transform.DOScale(0, 0.5f).SetEase(Ease.InBack);
            _btn_option1.transform.DOScale(0, 0.5f).SetEase(Ease.InBack);
            _btn_option2.transform.DOScale(0, 0.5f).SetEase(Ease.InBack).OnComplete(() => { this.gameObject.SetActive(false); });

        }
    }
}
