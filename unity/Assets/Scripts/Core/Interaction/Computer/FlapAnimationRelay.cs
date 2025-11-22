using Core.SFX;
using UnityEngine;

namespace Core.Interaction
{
    public class FlapAnimationRelay : MonoBehaviour
    {
        public void PlaySoundOpenFlap()
        {
            SFXController.Instance.PlayInteraction(SFXDatabase.Instance.flapOpenClip);
        }
    }
}