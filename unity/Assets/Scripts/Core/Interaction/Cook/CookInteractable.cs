using Core.Item;
using Core.Item.Cook;
using UnityEngine;
using Core.Item.Holder;
using Core.SFX;

namespace Core.Interaction
{
    public class CookInteractable : ItemHolderInteractable
    {
        [Header("Cooking System")]
        [SerializeField] private float cookTime = 5f;
        [SerializeField] private Animator bainMarieAnimator;
        [SerializeField] private ParticleSystem bainMarieParticle;

        private float cookTimer = 0f;
        private bool isCooking = false;
        private HoldItem currentItem;

        public override void Start()
        {
            bainMarieParticle.Stop();
            maxHoldableItems = 1;
            base.Start();

            OnItemAdded += (StartCooking);
            OnItemRemoved += (StopCooking);

            InInteraction += () =>
            {
                bainMarieAnimator.SetBool("Opened", true);
            };
            OutInteraction += () =>
            {
                bainMarieAnimator.SetBool("Opened", false);
            };
        }

        public void PlayOpenClip()
        {
            SFXController.Instance.PlayInteraction(SFXDatabase.instance.openCookClip);
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
            bainMarieParticle.Play();
            cookTimer = 0f;
        }

        private void StopCooking(HoldItem item)
        {
            isCooking = false;
            cookTimer = 0f;
            bainMarieParticle.Stop();
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
