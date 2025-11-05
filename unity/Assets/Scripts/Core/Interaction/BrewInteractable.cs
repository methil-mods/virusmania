using System.Collections.Generic;
using Core.Item;
using Core.Item.Merge;
using Core.Player;
using Framework.Extensions;
using UnityEngine;

namespace Core.Interaction
{
    public class BrewInteractable : MonoBehaviour, IInteractable
    {
        [Header("Configuration")]
        [SerializeField] private int maxHoldableItems = 3;
        [SerializeField] private List<Item.Item> startItems;

        [Header("Fusion System")]
        [SerializeField] private float mergeHoldTime = 5f;   // Durée à maintenir
        [SerializeField] private float cooldownSpeed = 1f;   // Vitesse de décrémentation du timer
        [SerializeField] private float holdReleaseDelay = 0.1f; // Délai max entre 2 appels de InteractHold

        [Header("Runtime")]
        public List<HoldItem> HoldingItems = new List<HoldItem>();

        private float holdTimer = 0f;
        private bool isBeingHeld = false;
        private float lastHoldTime = -999f;

        private void Start()
        {
            if (startItems != null && startItems.Count > 0)
            {
                foreach (var item in startItems)
                {
                    if (item != null && HoldingItems.Count < maxHoldableItems)
                        HoldingItems.Add(item.GetHoldItem());
                }
            }
        }

        private void Update()
        {
            // Détecte si le joueur a "relâché" (aucun appel récent à InteractHold)
            if (Time.time - lastHoldTime > holdReleaseDelay)
                isBeingHeld = false;

            // Si on ne maintient pas, le compteur redescend tout seul
            if (!isBeingHeld && holdTimer > 0f)
            {
                holdTimer = Mathf.Max(0f, holdTimer - Time.deltaTime * cooldownSpeed);
            }
        }

        public void Interact(PlayerController playerController)
        {
            PlayerInteraction playerInteraction = playerController.updatables.FirstOfType<PlayerInteraction>();
            if (playerInteraction == null) return;

            if (playerInteraction.HasItem)
            {
                if (HoldingItems.Count < maxHoldableItems)
                {
                    HoldItem removedItem = playerInteraction.RemoveItem();
                    if (removedItem != null)
                        HoldingItems.Add(removedItem);
                }
                else
                    Debug.Log("Le support est plein !");
            }
            else
            {
                if (HoldingItems.Count > 0)
                {
                    HoldItem itemToGive = HoldingItems[^1];
                    bool given = playerInteraction.GiveItem(itemToGive);
                    if (given)
                        HoldingItems.RemoveAt(HoldingItems.Count - 1);
                }
            }
        }

        public void InteractHold(PlayerController playerController)
        {
            // Marque qu'on tient toujours le bouton
            isBeingHeld = true;
            lastHoldTime = Time.time;

            if (HoldingItems.Count < 2)
            {
                holdTimer = 0f; // Pas assez d’items
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
                Debug.Log($"Fusion réussie : {mergedItem.itemName} !");
                HoldingItems.Clear();
                HoldingItems.Add(mergedItem.GetHoldItem());
            }
            else
            {
                Debug.Log("Aucune recette valide trouvée.");
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (HoldingItems != null && HoldingItems.Count > 0)
            {
                Vector3 basePos = transform.position + Vector3.up * 2f;
                GUIStyle style = new GUIStyle
                {
                    normal = new GUIStyleState { textColor = Color.yellow },
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Bold
                };

                for (int i = 0; i < HoldingItems.Count; i++)
                {
                    var holdItem = HoldingItems[i];
                    if (holdItem?.Item != null)
                    {
                        Vector3 pos = basePos + Vector3.up * (0.3f * i);
                        UnityEditor.Handles.Label(pos, holdItem.Item.itemName, style);
                    }
                }
            }

            if (Application.isPlaying && HoldingItems.Count >= 2)
            {
                float progress = Mathf.Clamp01(holdTimer / mergeHoldTime);
                UnityEditor.Handles.Label(transform.position + Vector3.up * 3f,
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
