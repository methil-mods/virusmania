Shader "UI/CircleMask"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        _MaskSize ("Mask Size", Range(0, 1)) = 1
        _MaskOffset ("Mask Offset", Vector) = (0.5, 0.5, 0, 0)
        _BorderWidth ("Border Width", Range(0, 0.1)) = 0.01
        _BorderColor ("Border Color", Color) = (1,1,1,1)
        
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        
        _ColorMask ("Color Mask", Float) = 15
        
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }
    
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
            "RenderPipeline"="UniversalPipeline"
        }
        
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]
        
        Pass
        {
            Name "Default"
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityInput.hlsl"
            
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
            
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            struct v2f
            {
                float4 vertex   : SV_POSITION;
                half4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                float2 aspectRatio : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _MainTex_TexelSize;
                half4 _Color;
                half4 _TextureSampleAdd;
                float4 _ClipRect;
                float _MaskSize;
                float2 _MaskOffset;
                float _BorderWidth;
                half4 _BorderColor;
            CBUFFER_END
            
            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                
                OUT.worldPosition = v.vertex;
                OUT.vertex = TransformObjectToHClip(v.vertex.xyz);
                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                OUT.color = v.color * _Color;
                
                // Calculate aspect ratio from texture dimensions
                float4 texelSize = _MainTex_ST;
                OUT.aspectRatio = float2(1.0, _ScreenParams.x / _ScreenParams.y);
                
                return OUT;
            }
            
            half4 frag(v2f IN) : SV_Target
            {
                half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.texcoord) + _TextureSampleAdd;
                color *= IN.color;
                
                // Get UV coordinates
                float2 uv = IN.texcoord;
                
                // Calculate aspect ratio correction
                float aspectRatio = _MainTex_TexelSize.z / _MainTex_TexelSize.w;
                
                // Adjust UV to maintain circular shape
                float2 centeredUV = uv - _MaskOffset;
                if (aspectRatio > 1.0)
                {
                    // Width > Height
                    centeredUV.x *= aspectRatio;
                }
                else
                {
                    // Height > Width
                    centeredUV.y /= aspectRatio;
                }
                
                // Calculate distance from mask center with corrected aspect ratio
                float dist = length(centeredUV);
                
                // Calculate mask radius based on size
                float radius = _MaskSize * 2.0;
                
                // Apply circular mask with hard edge (pixel perfect)
                float mask = step(dist, radius);
                
                // Calculate inner border
                float innerRadius = radius - _BorderWidth;
                float borderMask = step(dist, radius) * (1.0 - step(dist, innerRadius));
                
                // Apply border color
                color.rgb = lerp(color.rgb, _BorderColor.rgb, borderMask * _BorderColor.a);
                
                // Apply mask
                color.a *= mask;
                
                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif
                
                #ifdef UNITY_UI_ALPHACLIP
                clip(color.a - 0.001);
                #endif
                
                return color;
            }
            ENDHLSL
        }
    }
}