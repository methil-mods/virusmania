using UnityEngine;

namespace Core.Utils
{
    public static class ScreenLightUtils
    {
        public static void AnimateShaderOnInteraction(GameObject go, Material screenMaterial)
        {
            LeanTween.value(go, f =>
            {
                var vector = screenMaterial.GetVector("_TurnWhiteEffect");
                if (vector.y < 0.05f) vector.y = 0.05f;
                screenMaterial.SetVector("_TurnWhiteEffect", new Vector2(f, vector.y));
            }, screenMaterial.GetVector("_TurnWhiteEffect").x, 1f, 0.4f).setEaseInOutCirc();

            LeanTween.value(go, f =>
            {
                var vector = screenMaterial.GetVector("_TurnWhiteEffect");
                screenMaterial.SetVector("_TurnWhiteEffect", new Vector2(vector.x, f));
            }, screenMaterial.GetVector("_TurnWhiteEffect").y, 1f, 1f).setDelay(0.5f).setEaseInOutCirc();
        }

        public static void AnimateShaderOutInteraction(GameObject gameObject, Material screenMaterial)
        {
            LeanTween.value(gameObject, f =>
            {
                var vector = screenMaterial.GetVector("_TurnWhiteEffect");
                if (vector.y < 0.05f) vector.y = 0.05f;
                screenMaterial.SetVector("_TurnWhiteEffect", new Vector2(f, vector.y));
            }, screenMaterial.GetVector("_TurnWhiteEffect").x, 0f, 0.6f).setDelay(0.5f).setEaseInOutCirc();

            LeanTween.value(gameObject, f =>
            {
                var vector = screenMaterial.GetVector("_TurnWhiteEffect");
                screenMaterial.SetVector("_TurnWhiteEffect", new Vector2(vector.x, f));
            }, screenMaterial.GetVector("_TurnWhiteEffect").y, 0.05f, 0.4f).setDelay(0.5f).setEaseInOutCirc();
        }
    }
}