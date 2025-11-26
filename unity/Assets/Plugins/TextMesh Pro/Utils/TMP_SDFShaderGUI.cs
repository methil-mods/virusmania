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
            public static readonly GUIContent underlayLabel = new GUIContent("Underlay (Drop Shadow)");
            public static readonly GUIContent glowLabel = new GUIContent("Glow");
            public static readonly GUIContent debugLabel = new GUIContent("Debug Settings");
            
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
            
            public static readonly GUIContent underlayColor = new GUIContent("Color", "Color of the drop shadow");
            public static readonly GUIContent underlayOffsetX = new GUIContent("Offset X", "Horizontal offset of the drop shadow");
            public static readonly GUIContent underlayOffsetY = new GUIContent("Offset Y", "Vertical offset of the drop shadow");
            public static readonly GUIContent underlayDilate = new GUIContent("Dilate", "Expand or contract the drop shadow");
            public static readonly GUIContent underlaySoftness = new GUIContent("Softness", "Softness of the drop shadow edge");
            
            public static readonly GUIContent glowColor = new GUIContent("Color", "Color of the glow");
            public static readonly GUIContent glowOffset = new GUIContent("Offset", "Offset of the glow effect");
            public static readonly GUIContent glowInner = new GUIContent("Inner", "Inner glow radius");
            public static readonly GUIContent glowOuter = new GUIContent("Outer", "Outer glow radius");
            public static readonly GUIContent glowPower = new GUIContent("Falloff", "Falloff of the glow");
        }

        private MaterialProperty faceTex;
        private MaterialProperty faceUVSpeedX;
        private MaterialProperty faceUVSpeedY;
        private MaterialProperty faceColor;
        private MaterialProperty faceDilate;
        
        private MaterialProperty outlineColor;
        private MaterialProperty outlineTex;
        private MaterialProperty outlineUVSpeedX;
        private MaterialProperty outlineUVSpeedY;
        private MaterialProperty outlineWidth;
        private MaterialProperty outlineSoftness;
        private MaterialProperty outlineMode;
        
        private MaterialProperty underlayColor;
        private MaterialProperty underlayOffsetX;
        private MaterialProperty underlayOffsetY;
        private MaterialProperty underlayDilate;
        private MaterialProperty underlaySoftness;
        
        private MaterialProperty glowColor;
        private MaterialProperty glowOffset;
        private MaterialProperty glowInner;
        private MaterialProperty glowOuter;
        private MaterialProperty glowPower;
        
        private MaterialProperty shaderFlags;

        private bool m_FaceFoldout = true;
        private bool m_OutlineFoldout = true;
        private bool m_UnderlayFoldout = false;
        private bool m_GlowFoldout = false;

        public void FindProperties(MaterialProperty[] props)
        {
            faceTex = FindProperty("_FaceTex", props);
            faceUVSpeedX = FindProperty("_FaceUVSpeedX", props);
            faceUVSpeedY = FindProperty("_FaceUVSpeedY", props);
            faceColor = FindProperty("_FaceColor", props);
            faceDilate = FindProperty("_FaceDilate", props);
            
            outlineColor = FindProperty("_OutlineColor", props);
            outlineTex = FindProperty("_OutlineTex", props);
            outlineUVSpeedX = FindProperty("_OutlineUVSpeedX", props);
            outlineUVSpeedY = FindProperty("_OutlineUVSpeedY", props);
            outlineWidth = FindProperty("_OutlineWidth", props);
            outlineSoftness = FindProperty("_OutlineSoftness", props);
            outlineMode = FindProperty("_OutlineMode", props);
            
            underlayColor = FindProperty("_UnderlayColor", props);
            underlayOffsetX = FindProperty("_UnderlayOffsetX", props);
            underlayOffsetY = FindProperty("_UnderlayOffsetY", props);
            underlayDilate = FindProperty("_UnderlayDilate", props);
            underlaySoftness = FindProperty("_UnderlaySoftness", props);
            
            glowColor = FindProperty("_GlowColor", props);
            glowOffset = FindProperty("_GlowOffset", props);
            glowInner = FindProperty("_GlowInner", props);
            glowOuter = FindProperty("_GlowOuter", props);
            glowPower = FindProperty("_GlowPower", props);
            
            shaderFlags = FindProperty("_ShaderFlags", props);
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
                materialEditor.ColorProperty(faceColor, Styles.faceColor.text);
                materialEditor.TextureProperty(faceTex, Styles.faceTex.text);
                materialEditor.RangeProperty(faceDilate, Styles.faceDilate.text);
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(Styles.faceUVSpeed);
                EditorGUIUtility.labelWidth = 12f;
                materialEditor.RangeProperty(faceUVSpeedX, "X");
                materialEditor.RangeProperty(faceUVSpeedY, "Y");
                EditorGUIUtility.labelWidth = 0f;
                EditorGUILayout.EndHorizontal();
                
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            // Outline Section
            m_OutlineFoldout = EditorGUILayout.Foldout(m_OutlineFoldout, Styles.outlineLabel, true);
            if (m_OutlineFoldout)
            {
                EditorGUI.indentLevel++;
                materialEditor.ColorProperty(outlineColor, Styles.outlineColor.text);
                materialEditor.TextureProperty(outlineTex, Styles.outlineTex.text);
                materialEditor.RangeProperty(outlineWidth, Styles.outlineWidth.text);
                materialEditor.RangeProperty(outlineSoftness, Styles.outlineSoftness.text);
                
                // Outline Mode with keyword management
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = outlineMode.hasMixedValue;
                int outlineModeValue = (int)outlineMode.floatValue;
                outlineModeValue = EditorGUILayout.Popup(Styles.outlineMode, outlineModeValue, 
                    new string[] { "Both", "Outer", "Inner" });
                EditorGUI.showMixedValue = false;
                
                if (EditorGUI.EndChangeCheck())
                {
                    materialEditor.RegisterPropertyChangeUndo("Outline Mode");
                    outlineMode.floatValue = outlineModeValue;
                    
                    // Set shader keywords
                    foreach (Material mat in outlineMode.targets)
                    {
                        SetOutlineModeKeyword(mat, outlineModeValue);
                    }
                }
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(Styles.outlineUVSpeed);
                EditorGUIUtility.labelWidth = 12f;
                materialEditor.RangeProperty(outlineUVSpeedX, "X");
                materialEditor.RangeProperty(outlineUVSpeedY, "Y");
                EditorGUIUtility.labelWidth = 0f;
                EditorGUILayout.EndHorizontal();
                
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            // Underlay Section
            m_UnderlayFoldout = EditorGUILayout.Foldout(m_UnderlayFoldout, Styles.underlayLabel, true);
            if (m_UnderlayFoldout)
            {
                EditorGUI.indentLevel++;
                
                bool underlayEnabled = material.IsKeywordEnabled("UNDERLAY_ON") || 
                                      material.IsKeywordEnabled("UNDERLAY_INNER");
                
                EditorGUI.BeginChangeCheck();
                underlayEnabled = EditorGUILayout.Toggle("Enable Underlay", underlayEnabled);
                if (EditorGUI.EndChangeCheck())
                {
                    materialEditor.RegisterPropertyChangeUndo("Toggle Underlay");
                    if (underlayEnabled)
                    {
                        material.EnableKeyword("UNDERLAY_ON");
                        material.DisableKeyword("UNDERLAY_INNER");
                    }
                    else
                    {
                        material.DisableKeyword("UNDERLAY_ON");
                        material.DisableKeyword("UNDERLAY_INNER");
                    }
                }
                
                if (underlayEnabled)
                {
                    materialEditor.ColorProperty(underlayColor, Styles.underlayColor.text);
                    materialEditor.RangeProperty(underlayOffsetX, Styles.underlayOffsetX.text);
                    materialEditor.RangeProperty(underlayOffsetY, Styles.underlayOffsetY.text);
                    materialEditor.RangeProperty(underlayDilate, Styles.underlayDilate.text);
                    materialEditor.RangeProperty(underlaySoftness, Styles.underlaySoftness.text);
                }
                
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            // Glow Section
            m_GlowFoldout = EditorGUILayout.Foldout(m_GlowFoldout, Styles.glowLabel, true);
            if (m_GlowFoldout)
            {
                EditorGUI.indentLevel++;
                
                bool glowEnabled = material.IsKeywordEnabled("GLOW_ON");
                
                EditorGUI.BeginChangeCheck();
                glowEnabled = EditorGUILayout.Toggle("Enable Glow", glowEnabled);
                if (EditorGUI.EndChangeCheck())
                {
                    materialEditor.RegisterPropertyChangeUndo("Toggle Glow");
                    if (glowEnabled)
                        material.EnableKeyword("GLOW_ON");
                    else
                        material.DisableKeyword("GLOW_ON");
                }
                
                if (glowEnabled)
                {
                    materialEditor.ColorProperty(glowColor, Styles.glowColor.text);
                    materialEditor.RangeProperty(glowOffset, Styles.glowOffset.text);
                    materialEditor.RangeProperty(glowInner, Styles.glowInner.text);
                    materialEditor.RangeProperty(glowOuter, Styles.glowOuter.text);
                    materialEditor.RangeProperty(glowPower, Styles.glowPower.text);
                }
                
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.PropertiesChanged();
            }
        }

        private void SetOutlineModeKeyword(Material material, int mode)
        {
            // Clear all outline mode keywords first
            material.DisableKeyword("_OUTLINEMODE_BOTH");
            material.DisableKeyword("_OUTLINEMODE_OUTER");
            material.DisableKeyword("_OUTLINEMODE_INNER");
            
            // Set the appropriate keyword
            switch (mode)
            {
                case 0: // Both
                    material.EnableKeyword("_OUTLINEMODE_BOTH");
                    break;
                case 1: // Outer
                    material.EnableKeyword("_OUTLINEMODE_OUTER");
                    break;
                case 2: // Inner
                    material.EnableKeyword("_OUTLINEMODE_INNER");
                    break;
            }
        }
    }
}