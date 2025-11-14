using System.Collections.Generic;
using System.Linq;
using Core.Analysis;
using Core.Item;
using Core.Item.Merge;
using Core.Player;
using UnityEngine;
using UnityEngine.Splines.Interpolators;
using UnityEngine.UI;

namespace Core.Interaction
{
    public class AnalysisInteractable : ItemHolderInteractable
    {
        [Header("Analysis System")]
        [SerializeField] private float mergeHoldTime = 5f;
        [SerializeField] private float cooldownSpeed = 1f;
        [SerializeField] private float holdReleaseDelay = 0.1f;

        [SerializeField]
        private Animator analysisAnimator;
        public Image holdInteractImage;
        
        private float lastHoldTime = -999f;
        private float holdTimer = 0f;
        private bool isBeingHeld = false;
        private bool isSliderVisible = false;

        public override void Start()
        {
            base.Start();

            holdInteractImage.material = new Material(holdInteractImage.material);
            
            OnItemAdded += (_ => ResetAnalysis());
            OnItemRemoved += (_ => ResetAnalysis());
            
            holdInteractImage.GetComponent<RectTransform>().localScale = Vector2.zero;
        }

        private void Update()
        {
            if (Time.time - lastHoldTime > holdReleaseDelay)
                isBeingHeld = false;

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
            
            analysisAnimator.SetBool("IsInteracting", false);
        }

        public override void InteractHold(PlayerController playerController)
        {
            isBeingHeld = true;
            analysisAnimator.SetBool("IsInteracting", true);
            lastHoldTime = Time.time;

            if (HoldingItems.Count < 1)
            {
                holdTimer = 0f;
                return;
            }

            holdTimer += Time.deltaTime;

            if (holdTimer >= mergeHoldTime)
            {
                AnalyzeItem();
                holdTimer = 0f;
                isBeingHeld = false;
            }
        }

        private void AnalyzeItem()
        {
            Debug.Log("Analyzing item");
            AnalysisInterface.Instance.ShowAnalysis(HoldingItems.First());
        }

        private void ResetAnalysis()
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
