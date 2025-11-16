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
        [SerializeField]
        private Animator threadMillAnimator;
        
        public void SendItem()
        {
            HoldItem holdItem = HoldingItems.Count > 0 ? HoldingItems.First() : null;
            threadMillAnimator.SetBool("Roll", true);
            LeanTween.delayedCall(2.084f, () => { 
                threadMillAnimator.SetBool("Roll", false);
                
            });
            if (BriefController.Instance.TryToCompleteBrief(holdItem))
            {
                TimerController.Instance.StopTimer();
                Debug.Log("Send Item -> Validate brief !");
            }
            else
            {
                Debug.Log("Send Item -> Brief not validated...");
            }
        }

        public override void InteractHold(PlayerController playerController)
        {
            // Debug.Log("Interacting hold with " + gameObject.name);
        }
    }
}
