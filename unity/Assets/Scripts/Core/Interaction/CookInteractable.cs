using Core.Item;
using Core.Item.Cook;
using UnityEngine;

namespace Core.Interaction
{
    public class CookInteractable : ItemHolderInteractable
    {
        [Header("Cooking System")]
        [SerializeField] private float cookTime = 5f;

        private float cookTimer = 0f;
        private bool isCooking = false;
        private HoldItem currentItem;

        protected void Start()
        {
            maxHoldableItems = 1;
            base.Start();

            OnItemAdded += (StartCooking);
            OnItemRemoved += (StopCooking);
        }

        private void Update()
        {
            if (!isCooking || currentItem == null) return;

            var cooked = CookUtils.TryCook(currentItem.Item);
            if (cooked == null)
            {
                StopCooking(currentItem);
                return;
            }

            cookTimer += Time.deltaTime;

            if (cookTimer >= cookTime)
            {
                RemoveItem(currentItem);
                AddItem(cooked.GetHoldItem());
                StopCooking(currentItem);
            }
        }

        private void StartCooking(HoldItem item)
        {
            currentItem = item;
            isCooking = true;
            cookTimer = 0f;
        }

        private void StopCooking(HoldItem item)
        {
            isCooking = false;
            cookTimer = 0f;
            currentItem = null;
        }

#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (Application.isPlaying && isCooking && currentItem != null)
            {
                float progress = Mathf.Clamp01(cookTimer / cookTime);
                UnityEditor.Handles.Label(transform.position + Vector3.up * 3.5f,
                    $"Cooking: {(progress * 100f):F0}%", new GUIStyle
                    {
                        normal = new GUIStyleState { textColor = Color.cyan },
                        alignment = TextAnchor.MiddleCenter,
                        fontStyle = FontStyle.Italic
                    });
            }
        }
#endif
    }
}
