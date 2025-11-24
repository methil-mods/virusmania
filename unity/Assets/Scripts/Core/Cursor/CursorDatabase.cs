using Framework.ScriptableObjects;
using UnityEngine;

namespace Core.Cursor
{
    [CreateAssetMenu(fileName = "CursorDatabase", menuName = "Cursor/CursorDatabase")]
    public class CursorDatabase : SingletonScriptableObject<CursorDatabase>
    {
        public Texture2D cursorTexture;
        public Vector2 hotspot;
        public CursorMode cursorMode = CursorMode.Auto;
    }
}