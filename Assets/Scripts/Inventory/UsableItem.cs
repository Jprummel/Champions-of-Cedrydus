using System.Collections;
using UI;
using UnityEngine;
using Utility;
namespace Inventory
{
	public class UsableItem : MonoBehaviour 
	{
        public delegate void ShowSpinnerAction();
        public static event ShowSpinnerAction OnShowSpinner;

        public delegate void CloseInventoryAction();
        public static event CloseInventoryAction OnCloseInventory;

        public delegate void UseItemAction();
        public static event UseItemAction OnUseItem;

        public delegate void ExecuteEffectAction();
        public static event ExecuteEffectAction OnExecuteEffect;

        protected TurnManager _turnManager;
        protected Character _user;

        protected string _itemDescription;

        [HideInInspector] public Transform FormerParent;

        public string ItemName;

        private void Awake()
        {
            _turnManager = GameObject.FindWithTag(InlineStrings.TURNMANAGERTAG).GetComponent<TurnManager>();
        }

        public virtual void UseItem()
        {
            PlayerTarget._UsableItem = this;
            CloseInventory();
            if (OnUseItem != null)
                OnUseItem();
        }

        public virtual void ExecuteUsableEffect()
        {
            _user = _turnManager.ActivePlayerCharacter;
            if (OnExecuteEffect != null)
                OnExecuteEffect();
        }

        public virtual void CloseInventory()
        {
            FormerParent = transform.parent;
            transform.SetParent(null);
            if(OnCloseInventory != null)
                OnCloseInventory();
        }

        public virtual void EnableSpinner()
        {
            if (CameraFollowPlayer.OnChangeTarget != null)
                CameraFollowPlayer.OnChangeTarget(_turnManager.ActivePlayer.transform);

            if (OnShowSpinner != null)
                OnShowSpinner();

        }

        public virtual void RemoveItemFromInventory()
        {
            StartCoroutine(RemoveDelay());
        }

        private IEnumerator RemoveDelay()
        {
            yield return new WaitForEndOfFrame();
            _turnManager.ActivePlayerCharacter._PlayerInventory.RemoveInventoryItem(ItemName);
            PlayerTarget._UsableItem = null;
            Destroy(gameObject);
        }
	}
}