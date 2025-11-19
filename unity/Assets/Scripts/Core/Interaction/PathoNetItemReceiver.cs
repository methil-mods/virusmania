using Core.Item.Holder;
using Core.Player;
using Framework.Extensions;
using UnityEngine;

namespace Core.Interaction
{
    public class PathoNetItemReceiver : ItemHolderInteractable
    {
        public Animator flapAnimator;
        
        public override void Interact(PlayerController playerController)
        {
            PlayerInteraction playerInteraction = playerController.updatables.FirstOfType<PlayerInteraction>();
            if (playerInteraction == null) return;

            if (!playerInteraction.HasItem)
            {
                if (HoldingItems.Count > 0)
                {
                    HoldItem itemToGive = HoldingItems[HoldingItems.Count - 1];
                    bool givedItem = playerInteraction.GiveItem(itemToGive);
                    if (givedItem)
                        RemoveItem(itemToGive);
                }
            }
        }
        
        public override void AddItem(HoldItem holdItem)
        {
            if (holdItem == null || holdItem.Item == null || !CanAddItem()) return;
            
            TriggerFlapAnimation();
            
            HoldingItems.Insert(0, holdItem);

            if (holdItem.Item.itemPrefab != null)
            {
                Transform parent = itemParent != null ? itemParent : transform;
                GameObject spawned = Instantiate(holdItem.Item.itemPrefab, parent);
                spawned.AddComponent<Rigidbody>();
                spawnedPrefabs.Insert(0, spawned);
                UpdateItemPosition(spawned);
            }

            OnItemAdded?.Invoke(holdItem);
            OnItemsChanged?.Invoke();
        }

        public void TriggerFlapAnimation()
        {
            flapAnimator.SetTrigger("Open");
        }

        private void UpdateItemPosition(GameObject itemPrefab)
        {
            if (spawnedPrefabs == null || spawnedPrefabs.Count == 0) return;
            int count = spawnedPrefabs.Count;
            Transform parent = itemParent != null ? itemParent : transform;
            Vector3 basePos = parent.position + itemOffset;

            itemPrefab.transform.position = basePos;
            itemPrefab.transform.LookAt(basePos);
        }
        
        protected override void UpdateItemPositions()
        {
            return;
        }
    }
}