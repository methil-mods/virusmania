using Core.Item;
using Core.Item.Cook;
using Core.Player;
using Framework.Extensions;
using UnityEngine;

namespace Core.Interaction
{
    public class CookInteractable : MonoBehaviour, IInteractable
    {
        [Header("Configuration")]
        [SerializeField] private Item.Item startItem;

        [Header("Cooking System")]
        [SerializeField] private float cookTime = 5f;

        [Header("Runtime")]
        public HoldItem HoldingItem;

        private float cookTimer = 0f;
        private bool isCooking = false;

        private void Start()
        {
            if (startItem != null)
                HoldingItem = startItem.GetHoldItem();
        }

        private void Update()
        {
            if (HoldingItem == null)
            {
                cookTimer = 0f;
                isCooking = false;
                return;
            }

            Item.Item cooked = CookUtils.TryCook(HoldingItem.Item);
            if (cooked == null)
            {
                cookTimer = 0f;
                isCooking = false;
                return;
            }

            isCooking = true;
            cookTimer += Time.deltaTime;

            if (cookTimer >= cookTime)
            {
                HoldingItem = cooked.GetHoldItem();
                cookTimer = 0f;
                isCooking = false;
            }
        }

        public void Interact(PlayerController playerController)
        {
            PlayerInteraction playerInteraction = playerController.updatables.FirstOfType<PlayerInteraction>();
            if (playerInteraction == null) return;

            if (playerInteraction.HasItem)
            {
                if (HoldingItem == null)
                {
                    HoldItem removedItem = playerInteraction.RemoveItem();
                    if (removedItem != null)
                        HoldingItem = removedItem;
                }
            }
            else
            {
                if (HoldingItem != null)
                {
                    bool given = playerInteraction.GiveItem(HoldingItem);
                    if (given)
                    {
                        HoldingItem = null;
                        cookTimer = 0f;
                        isCooking = false;
                    }
                }
            }
        }

        public void InteractHold(PlayerController playerController)
        {
            return;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 titlePos = transform.position + Vector3.up * 3f;
            GUIStyle titleStyle = new GUIStyle
            {
                normal = new GUIStyleState { textColor = Color.cyan },
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            };
            UnityEditor.Handles.Label(titlePos, gameObject.name, titleStyle);

            if (HoldingItem != null && HoldingItem.Item != null)
            {
                Vector3 pos = transform.position + Vector3.up * 2f;
                GUIStyle style = new GUIStyle
                {
                    normal = new GUIStyleState { textColor = Color.yellow },
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Bold
                };
                UnityEditor.Handles.Label(pos, HoldingItem.Item.itemName, style);
            }

            if (Application.isPlaying && isCooking && HoldingItem != null)
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
