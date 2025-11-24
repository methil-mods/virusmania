using UnityEngine;
using Framework.Controller;

namespace Core.Cursor
{
    public class CursorController : BaseController<CursorController>
    {
        const int width = 32;
        const int height = 32;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);

            var src = CursorDatabase.Instance.cursorTexture;
            var tex = BilinearScale(src, width, height);

            UnityEngine.Cursor.SetCursor(tex, CursorDatabase.Instance.hotspot, CursorDatabase.Instance.cursorMode);
        }

        static Texture2D BilinearScale(Texture2D texture, int w, int h)
        {
            var result = new Texture2D(w, h, TextureFormat.ARGB32, false);
            var px = texture.GetPixels();
            float xRatio = (texture.width - 1f) / w;
            float yRatio = (texture.height - 1f) / h;

            for (int y = 0; y < h; y++)
            {
                float yy = y * yRatio;
                int yFloor = (int)yy;
                int yCeil = Mathf.Min(yFloor + 1, texture.height - 1);
                float yLerp = yy - yFloor;

                for (int x = 0; x < w; x++)
                {
                    float xx = x * xRatio;
                    int xFloor = (int)xx;
                    int xCeil = Mathf.Min(xFloor + 1, texture.width - 1);
                    float xLerp = xx - xFloor;

                    Color bl = px[yFloor * texture.width + xFloor];
                    Color br = px[yFloor * texture.width + xCeil];
                    Color tl = px[yCeil * texture.width + xFloor];
                    Color tr = px[yCeil * texture.width + xCeil];

                    Color top = Color.Lerp(tl, tr, xLerp);
                    Color bottom = Color.Lerp(bl, br, xLerp);
                    Color pixel = Color.Lerp(bottom, top, yLerp);

                    result.SetPixel(x, y, pixel);
                }
            }

            result.Apply();
            return result;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            var obj = new GameObject("CustomCursor");
            obj.AddComponent<CursorController>();
        }
    }
}
