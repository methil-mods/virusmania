using UnityEngine;

namespace Core.Interaction
{
    public class PathoNetItemReceiver : ItemHolderInteractable
    {
        public Animator flapAnimator;

        public void Start()
        {
            base.Start();
            
            OnItemAdded += (test) =>
            {
                TriggerFlapAnimation();
            };
        }

        public void TriggerFlapAnimation()
        {
            flapAnimator.SetTrigger("Open");
        }
    }
}