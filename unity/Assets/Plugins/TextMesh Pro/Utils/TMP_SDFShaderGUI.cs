using UnityEngine;
using UnityEditor;

namespace Plugins.TextMesh_Pro.Utils
{
    public class TMP_SDFShaderGUI : ShaderGUI
    {
        private static class Styles
        {
            public static readonly GUIContent faceLabel = new GUIContent("Face");
            public static readonly GUIContent outlineLabel = new GUIContent("Outline");
            public static readonly GUIContent outline2Label = new GUIContent("Outline 2 (Secondary Border)");

            public static readonly GUIContent faceTex = new GUIContent("Face Texture", "Texture to apply to the face of the text");
            public static readonly GUIContent faceColor = new GUIContent("Face Color", "Color of the face");
            public static readonly GUIContent faceDilate = new GUIContent("Face Dilate", "Adjust the face thickness");
            public static readonly GUIContent faceUVSpeed = new GUIContent("Face UV Speed", "Scroll speed for face texture");

            public static readonly GUIContent outlineColor = new GUIContent("Outline Color", "Color of the outline");
            public static readonly GUIContent outlineTex = new GUIContent("Outline Texture", "Texture to apply to the outline");
            public static readonly GUIContent outlineWidth = new GUIContent("Outline Thickness", "Thickness of the outline");
            public static readonly GUIContent outlineSoftness = new GUIContent("Outline Softness", "Softness of the outline edge");
            public static readonly GUIContent outlineMode = new GUIContent("Outline Mode", "Choose between outer, inner, or both outline modes");
            public static readonly GUIContent outlineUVSpeed = new GUIContent("Outline UV Speed", "Scroll speed for outline texture");

            public static readonly GUIContent outline2Color = new GUIContent("Outline 2 Color", "Color of the second outline");
            public static readonly GUIContent outline2Tex = new GUIContent("Outline 2 Texture", "Texture to apply to the second outline");
            public static readonly GUIContent outline2Width = new GUIContent("Outline 2 Thickness", "Thickness of the second outline");
            public static readonly GUIContent outline2Softness = new GUIContent("Outline 2 Softness", "Softness of the second outline edge");
            public static readonly GUIContent outline2OffsetX = new GUIContent("Offset X", "Horizontal offset of the second outline");
            public static readonly GUIContent outline2OffsetY = new GUIContent("Offset Y", "Vertical offset of the second outline");
            public static readonly GUIContent outline2UVSpeed = new GUIContent("Outline 2 UV Speed", "Scroll speed for second outline texture");
        }

        // Face
        private MaterialProperty faceTex;
        private MaterialProperty faceUVSpeedX;
        private MaterialProperty faceUVSpeedY;
        private MaterialProperty faceColor;
        private MaterialProperty faceDilate;

        // Outline
        private MaterialProperty outlineColor;
        private MaterialProperty outlineTex;
        private MaterialProperty outlineUVSpeedX;
        private MaterialProperty outlineUVSpeedY;
        private MaterialProperty outlineWidth;
        private MaterialProperty outlineSoftness;
        private MaterialProperty outlineMode;

        // Outline2
        private MaterialProperty outline2Color;
        private MaterialProperty outline2Tex;
        private MaterialProperty outline2UVSpeedX;
        private MaterialProperty outline2UVSpeedY;
        private MaterialProperty outline2Width;
        private MaterialProperty outline2Softness;
        private MaterialProperty outline2OffsetX;
        private MaterialProperty outline2OffsetY;

        private bool m_FaceFoldout = true;
        private bool m_OutlineFoldout = true;
        private bool m_Outline2Foldout = true;

        private MaterialProperty[] m_Props;

        private MaterialProperty FindPropSafe(string name)
        {
            try { return FindProperty(name, m_Props); }
            catch { return null; }
        }

        public void FindProperties(MaterialProperty[] props)
        {
            m_Props = props;

            faceTex = FindPropSafe("_FaceTex");
            faceUVSpeedX = FindPropSafe("_FaceUVSpeedX");
            faceUVSpeedY = FindPropSafe("_FaceUVSpeedY");
            faceColor = FindPropSafe("_FaceColor");
            faceDilate = FindPropSafe("_FaceDilate");

            outlineColor = FindPropSafe("_OutlineColor");
            outlineTex = FindPropSafe("_OutlineTex");
            outlineUVSpeedX = FindPropSafe("_OutlineUVSpeedX");
            outlineUVSpeedY = FindPropSafe("_OutlineUVSpeedY");
            outlineWidth = FindPropSafe("_OutlineWidth");
            outlineSoftness = FindPropSafe("_OutlineSoftness");
            outlineMode = FindPropSafe("_OutlineMode");

            outline2Color = FindPropSafe("_Outline2Color");
            outline2Tex = FindPropSafe("_Outline2Tex");
            outline2UVSpeedX = FindPropSafe("_Outline2UVSpeedX");
            outline2UVSpeedY = FindPropSafe("_Outline2UVSpeedY");
            outline2Width = FindPropSafe("_Outline2Width");
            outline2Softness = FindPropSafe("_Outline2Softness");
            outline2OffsetX = FindPropSafe("_Outline2OffsetX");
            outline2OffsetY = FindPropSafe("_Outline2OffsetY");
        }

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            FindProperties(properties);
            Material material = materialEditor.target as Material;

            EditorGUIUtility.labelWidth = 0f;
            EditorGUI.BeginChangeCheck();

            // Face Section
            m_FaceFoldout = EditorGUILayout.Foldout(m_FaceFoldout, Styles.faceLabel, true);
            if (m_FaceFoldout)
            {
                EditorGUI.indentLevel++;
                if (faceTex != null) materialEditor.TextureProperty(faceTex, Styles.faceTex.text);
                if (faceColor != null) materialEditor.ColorProperty(faceColor, Styles.faceColor.text);
                if (faceDilate != null) materialEditor.RangeProperty(faceDilate, Styles.faceDilate.text);

                if (faceUVSpeedX != null || faceUVSpeedY != null)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel(Styles.faceUVSpeed);
                    EditorGUIUtility.labelWidth = 12f;
                    if (faceUVSpeedX != null) materialEditor.FloatProperty(faceUVSpeedX, "X");
                    if (faceUVSpeedY != null) materialEditor.FloatProperty(faceUVSpeedY, "Y");
                    EditorGUIUtility.labelWidth = 0f;
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            // Outline Section
            m_OutlineFoldout = EditorGUILayout.Foldout(m_OutlineFoldout, Styles.outlineLabel, true);
            if (m_OutlineFoldout)
            {
                EditorGUI.indentLevel++;
                if (outlineColor != null) materialEditor.ColorProperty(outlineColor, Styles.outlineColor.text);
                if (outlineTex != null) materialEditor.TextureProperty(outlineTex, Styles.outlineTex.text);
                if (outlineWidth != null) materialEditor.RangeProperty(outlineWidth, Styles.outlineWidth.text);
                if (outlineSoftness != null) materialEditor.RangeProperty(outlineSoftness, Styles.outlineSoftness.text);
                if (outlineMode != null) materialEditor.ShaderProperty(outlineMode, Styles.outlineMode.text);

                if (outlineUVSpeedX != null || outlineUVSpeedY != null)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel(Styles.outlineUVSpeed);
                    EditorGUIUtility.labelWidth = 12f;
                    if (outlineUVSpeedX != null) materialEditor.FloatProperty(outlineUVSpeedX, "X");
                    if (outlineUVSpeedY != null) materialEditor.FloatProperty(outlineUVSpeedY, "Y");
                    EditorGUIUtility.labelWidth = 0f;
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            // Outline 2 Section
            m_Outline2Foldout = EditorGUILayout.Foldout(m_Outline2Foldout, Styles.outline2Label, true);
            if (m_Outline2Foldout)
            {
                EditorGUI.indentLevel++;

                bool outline2Enabled = material.IsKeywordEnabled("OUTLINE2_ON");
                EditorGUI.BeginChangeCheck();
                outline2Enabled = EditorGUILayout.Toggle("Enable Outline 2", outline2Enabled);
                if (EditorGUI.EndChangeCheck())
                {
                    materialEditor.RegisterPropertyChangeUndo("Toggle Outline 2");
                    if (outline2Enabled) material.EnableKeyword("OUTLINE2_ON"); else material.DisableKeyword("OUTLINE2_ON");
                }

                if (outline2Enabled)
                {
                    if (outline2Color != null) materialEditor.ColorProperty(outline2Color, Styles.outline2Color.text);
                    if (outline2Tex != null) materialEditor.TextureProperty(outline2Tex, Styles.outline2Tex.text);
                    if (outline2Width != null) materialEditor.RangeProperty(outline2Width, Styles.outline2Width.text);
                    if (outline2Softness != null) materialEditor.RangeProperty(outline2Softness, Styles.outline2Softness.text);
                    if (outline2OffsetX != null) materialEditor.RangeProperty(outline2OffsetX, Styles.outline2OffsetX.text);
                    if (outline2OffsetY != null) materialEditor.RangeProperty(outline2OffsetY, Styles.outline2OffsetY.text);

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel(Styles.outline2UVSpeed);
                    EditorGUIUtility.labelWidth = 12f;
                    if (outline2UVSpeedX != null) materialEditor.FloatProperty(outline2UVSpeedX, "X");
                    if (outline2UVSpeedY != null) materialEditor.FloatProperty(outline2UVSpeedY, "Y");
                    EditorGUIUtility.labelWidth = 0f;
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.PropertiesChanged();
            }
        }
    }
}
