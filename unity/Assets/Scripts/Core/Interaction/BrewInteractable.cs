using System.Collections.Generic;
using Core.Item;
using Core.Item.Merge;
using Core.Player;
using UnityEngine;
using Core.Item.Holder;
using Framework.Extensions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Core.Interaction
{
    public class BrewInteractable : ItemHolderInteractable
    {
        [Header("Fusion System")]
        [SerializeField] private float mergeHoldTime = 5f;
        [SerializeField] private float cooldownSpeed = 1f;
        [SerializeField] private float holdReleaseDelay = 0.1f;
        [SerializeField] private Animator mixingTableAnimator;

        public Image holdInteractImage;

        private float holdTimer = 0f;
        private bool isBeingHeld = false;
        private float lastHoldTime = -999f;
        private bool isSliderVisible = false;

        public UnityAction<Item.Item> onItemMerged;

        public override void Start()
        {
            base.Start();

            OnItemAdded += (_ => ResetFusion());
            OnItemRemoved += (_ => ResetFusion());
            
            holdInteractImage.GetComponent<RectTransform>().localScale = Vector2.zero;
        }

        private void Update()
        {
            if (Time.time - lastHoldTime > holdReleaseDelay)
            {
                isBeingHeld = false;
                mixingTableAnimator.SetBool("IsWorking", false);
            }

            if (!isBeingHeld && holdTimer > 0f)
                holdTimer = Mathf.Max(0f, holdTimer - Time.deltaTime * cooldownSpeed);
            
            if (holdTimer <= 0.3f && isSliderVisible)
            {
                isSliderVisible = false;
                LeanTween.cancel(holdInteractImage.gameObject);
                LeanTween.scale(holdInteractImage.GetComponent<RectTransform>(), Vector3.zero, 0.4f)
                    .setEase(LeanTweenType.easeInBack);
            }
            else if (holdTimer > 0.3f && !isSliderVisible)
            {
                isSliderVisible = true;
                LeanTween.cancel(holdInteractImage.gameObject);
                LeanTween.scale(holdInteractImage.GetComponent<RectTransform>(), Vector3.one, 0.4f)
                    .setEase(LeanTweenType.easeOutBack);
            }
            
            holdInteractImage.material.SetFloat("_InnerFillAmount", Mathf.Lerp(
                holdInteractImage.material.GetFloat("_InnerFillAmount"), holdTimer / mergeHoldTime * 100 / 100, .1f
            ));
        }

        public override void InInteractZone(PlayerController playerController)
        {
            if (this.HoldingItems.Count == maxHoldableItems) base.InInteractZone(playerController);
            
            PlayerInteraction playerInteraction = playerController.updatables.FirstOfType<PlayerInteraction>();
            if (playerInteraction == null) return;
            
            if (playerInteraction.HasItem)
            {
                base.InInteractZone(playerController);
            }
            else
            {
                if (this.HoldingItems.Count > 0) base.InInteractZone(playerController);
            }
        }

        public override void InteractHold(PlayerController playerController)
        {
            Item.Item[] itemsToMerge = HoldingItems.ConvertAll(h => h.Item).ToArray();
            if (MergeUtils.CanMerge(itemsToMerge) == false) return;
            
            isBeingHeld = true;
            mixingTableAnimator.SetBool("IsWorking", true);
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
                mixingTableAnimator.SetBool("IsWorking", false);
            }
        }

        private void TryMergeItems()
        {
            Item.Item[] itemsToMerge = HoldingItems.ConvertAll(h => h.Item).ToArray();
            HoldItem mergedHoldItem = MergeUtils.TryMerge(itemsToMerge);

            if (mergedHoldItem != null)
            {
                foreach (var h in new List<HoldItem>(HoldingItems))
                    RemoveItem(h);
                
                onItemMerged?.Invoke(mergedHoldItem.Item);
                AddItem(mergedHoldItem);
                ResetFusion();
            }
        }

        private void ResetFusion()
        {
            holdTimer = 0f;
            isBeingHeld = false;
            mixingTableAnimator.SetBool("IsWorking", false);
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
