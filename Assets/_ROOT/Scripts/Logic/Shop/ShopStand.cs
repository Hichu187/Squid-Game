using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class ShopStand : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField] private Transform _itemRoot;

        private ShopStandPurchase _purchase;

        private void Awake()
        {
            _purchase = GetComponentInChildren<ShopStandPurchase>();
            _purchase.eventConfirm += Purchase_EventConfirm;
        }

        private void Purchase_EventConfirm()
        {

        }
    }
}
