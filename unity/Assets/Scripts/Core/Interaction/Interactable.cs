using Core.Player;
using Core.Prefab;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Interaction
{
    public abstract class Interactable : MonoBehaviour
    {
        private GameObject _interactionIndicator;

        public UnityAction InInteraction;
        public UnityAction OutInteraction;

        public void Start()
        {
            if (PrefabDatabase.Instance.interactionIndicationPrefab != null)
            {
                Transform parentToAttach = transform.parent != null ? transform.parent : null;

                _interactionIndicator = Instantiate(
                    PrefabDatabase.Instance.interactionIndicationPrefab,
                    parentToAttach
                );

                _interactionIndicator.transform.position = transform.position + new Vector3(0f, 3.5f, 0.5f);
                _interactionIndicator.transform.rotation = Quaternion.identity;
                _interactionIndicator.transform.localScale = Vector3.zero;
                _interactionIndicator.SetActive(false);
            }
        }

        public virtual void InInteractZone()
        {
            InInteraction?.Invoke();
            if (_interactionIndicator != null)
            {
                LeanTween.cancel(_interactionIndicator);

                _interactionIndicator.SetActive(true);
                Vector3 parentScale = transform.parent != null ? transform.parent.lossyScale : Vector3.one;
                float desiredWorldScale = 0.0024f;
                Vector3 targetScale = new Vector3(
                    desiredWorldScale / parentScale.x,
                    desiredWorldScale / parentScale.y,
                    desiredWorldScale / parentScale.z
                );
                LeanTween.scale(_interactionIndicator, targetScale, 0.3f).setEaseOutBack();
            }
        }

        public void HideIndicator()
        {
            OutInteraction?.Invoke();
            if (_interactionIndicator != null && _interactionIndicator.activeSelf)
            {
                LeanTween.cancel(_interactionIndicator);

                LeanTween.scale(_interactionIndicator, Vector3.zero, 0.2f).setEaseInBack().setOnComplete(() =>
                {
                    _interactionIndicator.SetActive(false);
                });
            }
        }

        public virtual bool CanInteract(PlayerController playerController)
        {
            return true;
        }

        public abstract void Interact(PlayerController playerController);
        public abstract void InteractHold(PlayerController playerController);
    }
}
