Shader "UI/MethilUiWavyBlobOutline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FillColor ("Fill Color", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (0,1,0,1)

        [Header(Outline Controls)]
        _OutlineThickness ("Outline Thickness", Range(0, 0.5)) = 0.05
        _OutlineSizeMultiplier ("Outline Size Multiplier", Range(0, 3)) = 1.0
        _OutlineOffset ("Outline Offset", Range(0, 0.2)) = 0.0
        _OutlineOffsetX ("Outline Offset X", Range(-0.3, 0.3)) = 0.0
        _OutlineOffsetY ("Outline Offset Y", Range(-0.3, 0.3)) = 0.0
        _CornerRadius ("Corner Radius", Range(0, 0.5)) = 0.2

        [Header(Noise Controls)]
        _NoiseScale ("Noise Scale", Range(1, 40)) = 6.0
        _NoiseAmplitude ("Noise Amplitude", Range(0, 0.2)) = 0.05
        _NoiseSpeed ("Noise Speed", Range(0, 4)) = 0.5

        [Toggle] _EnableOutline ("Enable Outline", Float) = 1
        
        _AspectRatio ("Aspect Ratio (W/H)", Float) = 1.0

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        Cull Off ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask]

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes 
            { 
                float4 positionOS : POSITION; 
                float2 uv : TEXCOORD0; 
                float4 color : COLOR;
            };
            
            struct Varyings 
            { 
                float4 positionHCS : SV_POSITION; 
                float2 uv : TEXCOORD0; 
                float4 color : COLOR;
                float4 worldPosition : TEXCOORD1;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _FillColor;
                float4 _OutlineColor;
                float _OutlineThickness;
                float _OutlineSizeMultiplier;
                float _OutlineOffset;
                float _OutlineOffsetX;
                float _OutlineOffsetY;
                float _CornerRadius;
                float _NoiseScale;
                float _NoiseAmplitude;
                float _NoiseSpeed;
                float _EnableOutline;
                float _AspectRatio;
                float4 _ClipRect;
            CBUFFER_END

            float UnityGet2DClipping(float2 position, float4 clipRect)
            {
                float2 inside = step(clipRect.xy, position.xy) * step(position.xy, clipRect.zw);
                return inside.x * inside.y;
            }

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
                float3 pos = IN.positionOS.xyz;
                OUT.positionHCS = TransformObjectToHClip(pos);
                OUT.uv = IN.uv;
                OUT.color = IN.color;
                OUT.worldPosition = IN.positionOS;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;
                float time = _Time.y * _NoiseSpeed;

                float2 p = uv - 0.5;

                // Correction aspect ratio
                float2 screenSize = float2(_ScreenParams.x, _ScreenParams.y);
                float aspect = max(_AspectRatio, 0.01); // configurable
                p.x *= aspect;

                float padding = _NoiseAmplitude * 1.5;
                float2 halfSize = float2((0.5 - padding) * aspect, 0.5 - padding);

                // Main shape distance field
                float distMain = sdRoundedRect(p, halfSize, _CornerRadius);

                // Optional noise
                float2 pixelPos = p * screenSize;
                float2 nUV = pixelPos * (_NoiseScale / max(screenSize.x, screenSize.y)) + float2(time*0.25, time*0.15);
                float mainNoise = (_NoiseAmplitude > 0.0) ? (fbm(nUV) - 0.5) * 2.0 * _NoiseAmplitude : 0.0;
                distMain += mainNoise;

                float antiAlias = fwidth(distMain) * 1.2;

                // Outline computation
                float outlineWidth = _OutlineThickness * _OutlineSizeMultiplier;
                float outlineMask = (_EnableOutline > 0.5) ? smoothstep(outlineWidth + antiAlias, outlineWidth - antiAlias, abs(distMain)) : 0.0;

                // Fill computation
                float4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                float4 fillColor = texColor * _FillColor * IN.color;
                float fillMask = smoothstep(antiAlias, 0.0, distMain);

                // Combine outline and fill
                float4 outlineColor = _OutlineColor * outlineMask;
                float4 col = fillColor * fillMask + outlineColor * (1.0 - fillMask);
                col.a = saturate(fillMask + outlineMask);

                col.rgb = pow(col.rgb, 1.0 / 2.2);
                col.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

                return col;
            }

            ENDHLSL
        }
    }
}
