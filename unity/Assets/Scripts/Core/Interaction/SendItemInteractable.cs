using System;
using System.Linq;
using Core.Brief;
using Core.Player;
using Framework.Extensions;
using UnityEngine;
using Core.Item.Holder;
using Core.Timer;

namespace Core.Interaction
{
    public class SendItemInteractable : ItemHolderInteractable
    {
        [SerializeField] private Animator threadMillAnimator;
        [SerializeField] private Vector3 objectDestination;
        [SerializeField] private Renderer alarmRenderer;
        [SerializeField] private Light alarmLight;

        public override void Start()
        {
            base.Start();
            alarmRenderer.material = new Material(alarmRenderer.material);
            alarmLight.intensity = 0;
            alarmLight.color = Color.white;
            alarmRenderer.material.color = Color.white;
        }
        
        public override void InInteractZone(PlayerController playerController)
        {
            PlayerInteraction playerInteraction = playerController.updatables.FirstOfType<PlayerInteraction>();
            if (playerInteraction == null) return;
            
            if (playerInteraction.HasItem)
            {
                if (this.HoldingItems.Count < maxHoldableItems) base.InInteractZone(playerController);
            } else if (this.HoldingItems.Count > 0)
            {
                base.InInteractZone(playerController);
            }
        }

        public void SendItem()
        {
            HoldItem holdItem = HoldingItems.Count > 0 ? HoldingItems.First() : null;
            threadMillAnimator.SetBool("Roll", true);
            LeanTween.delayedCall(2.084f, () =>
            {
                threadMillAnimator.SetBool("Roll", false);
            });

            if (holdItem == null) return;

            LeanTween.moveLocalX(spawnedPrefabs.First(), objectDestination.x, 1.6f).setOnComplete(() =>
            {
                RemoveItem(holdItem);
            });

            if (BriefController.Instance.TryToCompleteBrief(holdItem))
            {
                alarmRenderer.material.color = Color.green;
                StartBlink(Color.green);
                TimerController.Instance.StopTimer();
                Debug.Log("Send Item -> Validate brief !");
            }
            else
            {
                alarmRenderer.material.color = Color.red;
                StartBlink(Color.red);
                Debug.Log("Send Item -> Brief not validated...");
            }
        }

        private void StartBlink(Color color)
        {
            LeanTween.cancel(gameObject);

            alarmLight.color = color;

            int flashCount = 6;
            float flashTime = 0.15f;
            float maxIntensity = 5f;

            LTSeq seq = LeanTween.sequence();

            for (int i = 0; i < flashCount; i++)
            {
                seq.append(
                    LeanTween.value(gameObject, 0f, maxIntensity, flashTime)
                        .setOnUpdate(v => alarmLight.intensity = v)
                );

                seq.append(
                    LeanTween.value(gameObject, maxIntensity, 0f, flashTime)
                        .setOnUpdate(v => alarmLight.intensity = v)
                );
            }

            seq.append(() => alarmLight.intensity = 0);
            seq.append(() => alarmLight.color = Color.white);
            seq.append(() => alarmRenderer.material.color = Color.white);
        }

        public override void InteractHold(PlayerController playerController)
        {
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Vector3 objectRelativeDestination = (itemParent != null ? itemParent.position : transform.position) + objectDestination;
            Gizmos.color = Color.brown;
            Gizmos.DrawSphere(objectRelativeDestination, 0.05f);
        }
    }
}
