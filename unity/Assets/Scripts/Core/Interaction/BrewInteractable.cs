using System.Collections.Generic;
using Core.Item;
using Core.Item.Merge;
using Core.Player;
using UnityEngine;

namespace Core.Interaction
{
    public class BrewInteractable : ItemHolderInteractable
    {
        [Header("Fusion System")]
        [SerializeField] private float mergeHoldTime = 5f;
        [SerializeField] private float cooldownSpeed = 1f;
        [SerializeField] private float holdReleaseDelay = 0.1f;

        private float holdTimer = 0f;
        private bool isBeingHeld = false;
        private float lastHoldTime = -999f;

        protected void Start()
        {
            base.Start();

            OnItemAdded += (_ => ResetFusion());
            OnItemRemoved += (_ => ResetFusion());
        }

        private void Update()
        {
            if (Time.time - lastHoldTime > holdReleaseDelay)
                isBeingHeld = false;

            if (!isBeingHeld && holdTimer > 0f)
                holdTimer = Mathf.Max(0f, holdTimer - Time.deltaTime * cooldownSpeed);
        }

        public override void InteractHold(PlayerController playerController)
        {
            isBeingHeld = true;
            lastHoldTime = Time.time;

            if (HoldingItems.Count < 2)
            {
                holdTimer = 0f;
                return;
            }

            holdTimer += Time.deltaTime;

            if (holdTimer >= mergeHoldTime)
            {
                TryMergeItems();
                holdTimer = 0f;
                isBeingHeld = false;
            }
        }

        private void TryMergeItems()
        {
            Item.Item[] itemsToMerge = HoldingItems.ConvertAll(h => h.Item).ToArray();
            Item.Item mergedItem = MergeUtils.TryMerge(itemsToMerge);

            if (mergedItem != null)
            {
                foreach (var h in new List<HoldItem>(HoldingItems))
                    RemoveItem(h);

                AddItem(mergedItem.GetHoldItem());
                ResetFusion();
            }
        }

        private void ResetFusion()
        {
            holdTimer = 0f;
            isBeingHeld = false;
            lastHoldTime = -999f;
        }

#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (Application.isPlaying && HoldingItems.Count >= 2)
            {
                float progress = Mathf.Clamp01(holdTimer / mergeHoldTime);
                UnityEditor.Handles.Label(transform.position + Vector3.up * 3.5f,
                    $"Fusion: {(progress * 100f):F0}%", new GUIStyle
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
