Shader "UI/MethilUiWavyBlob"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FillColor ("Fill Color", Color) = (1,1,1,1)
        _BorderColor ("Border Color", Color) = (0,1,0,1)

        [Header(Border Controls)]
        _BorderThickness ("Border Thickness", Range(0, 0.2)) = 0.05
        _BorderOffset ("Border Offset", Range(0, 0.2)) = 0.0
        _BorderOffsetX ("Border Offset X", Range(-0.3, 0.3)) = 0.0
        _BorderOffsetY ("Border Offset Y", Range(-0.3, 0.3)) = 0.0
        _CornerRadius ("Corner Radius", Range(0, 0.5)) = 0.2

        [Header(Noise Controls)]
        _NoiseScale ("Noise Scale", Range(1, 40)) = 6.0
        _NoiseAmplitude ("Noise Amplitude", Range(0, 0.2)) = 0.05
        _NoiseSpeed ("Noise Speed", Range(0, 4)) = 0.5

        [Toggle] _EnableBorder ("Enable Border", Float) = 1
        
        _AspectRatio ("Aspect Ratio (W/H)", Float) = 1.0
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        Cull Off ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes { float4 positionOS : POSITION; float2 uv : TEXCOORD0; };
            struct Varyings { float4 positionHCS : SV_POSITION; float2 uv : TEXCOORD0; };

            CBUFFER_START(UnityPerMaterial)
                float4 _FillColor;
                float4 _BorderColor;
                float _BorderThickness;
                float _BorderOffset;
                float _BorderOffsetX;
                float _BorderOffsetY;
                float _CornerRadius;
                float _NoiseScale;
                float _NoiseAmplitude;
                float _NoiseSpeed;
                float _EnableBorder;
                float _AspectRatio;
            CBUFFER_END

            // --- Hash + Smooth noise ---
            float hash(float2 p) { return frac(sin(dot(p, float2(127.1,311.7))) * 43758.5453); }

            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                float a = hash(i);
                float b = hash(i + float2(1, 0));
                float c = hash(i + float2(0, 1));
                float d = hash(i + float2(1, 1));
                float2 u = f * f * (3.0 - 2.0 * f);
                return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
            }

            // --- fBm noise (smoother & more detailed) ---
            float fbm(float2 p)
            {
                float v = 0.0;
                float a = 0.5;
                for (int i = 0; i < 4; i++)
                {
                    v += a * noise(p);
                    p *= 2.02;
                    a *= 0.5;
                }
                return v;
            }

            float sdRoundedRect(float2 p, float2 b, float r)
            {
                float2 q = abs(p) - b + r;
                return length(max(q, 0.0)) - r;
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;
                float time = _Time.y * _NoiseSpeed;
            
                float2 p = uv - 0.5;
                
                float aspectRatio = max(_AspectRatio, 0.01);
                p.x *= aspectRatio;
            
                float padding = _BorderThickness + _NoiseAmplitude * 1.5;
                float2 halfSize = float2((0.5 - padding) * aspectRatio, 0.5 - padding);
            
                float2 shadowOffset = float2(_BorderOffsetX * aspectRatio, _BorderOffsetY);
                float2 pShadow = p - shadowOffset;
                
                // Shadow shape distance (outline is a full copy of the shape, not just a ring)
                float distShadowOuter = sdRoundedRect(pShadow, halfSize, _CornerRadius);
                float distShadowInner = sdRoundedRect(pShadow, halfSize - _BorderThickness, _CornerRadius);
                
                // Apply noise to shadow shape
                float2 nUVShadow = pShadow * _NoiseScale + float2(time * 0.25, time * 0.15);
                float shadowNoise = (fbm(nUVShadow) - 0.5) * 2.0 * _NoiseAmplitude;
                float distShadowOuterNoisy = distShadowOuter + shadowNoise;
                float distShadowInnerNoisy = distShadowInner + shadowNoise;
                
                float distMain = sdRoundedRect(p, halfSize, _CornerRadius);
                
                // Apply noise to main shape
                float2 nUVMain = p * _NoiseScale + float2(time * 0.25, time * 0.15);
                float mainNoise = (fbm(nUVMain) - 0.5) * 2.0 * _NoiseAmplitude;
                float distMainNoisy = distMain + mainNoise;
            
                float antiAlias = fwidth(distMain) * 1.2;
                
                float shadowOuter = smoothstep(antiAlias, 0.0, distShadowOuterNoisy);
                float shadowInner = smoothstep(antiAlias, 0.0, distShadowInnerNoisy);
                float shadowRing = (_EnableBorder > 0.5) ? (shadowOuter - shadowInner) : 0.0;
                
                float mainShape = smoothstep(antiAlias, 0.0, distMainNoisy);
            
                float4 col = _BorderColor * shadowRing;
                col = lerp(col, _FillColor, mainShape);
                col.a = saturate(shadowRing * _BorderColor.a + mainShape * _FillColor.a);
            
                col.rgb = pow(col.rgb, 1.0 / 2.2);
                return col;
            }
            ENDHLSL
        }
    }
}
