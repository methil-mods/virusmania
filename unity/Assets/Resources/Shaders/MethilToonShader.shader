Shader "Custom/MethilToon_ForwardPlus_Fixed"
{
    Properties
    {
        _MainTex ("Base Texture (optional)", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _Steps ("Lighting Steps", Float) = 3
        _Brightness ("Brightness", Range(0,2)) = 1
        _MinLight ("Min Light Level", Range(0,1)) = 0.12
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _CLUSTER_LIGHT_LOOP
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHTS
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RealtimeLights.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionWS  : TEXCOORD0;
                float3 normalWS    : TEXCOORD1;
                float2 uv          : TEXCOORD2;
                float4 positionCS  : TEXCOORD3;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float4 _MainTex_ST;
            float4 _BaseColor;
            float _Steps;
            float _Brightness;
            float _MinLight;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionWS = vertexInput.positionWS;
                OUT.positionHCS = vertexInput.positionCS;
                OUT.positionCS = vertexInput.positionCS;
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            inline float3 SampleBaseColor(float2 uv)
            {
                float4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                const float WHITE_EPS = 0.02;
                float isWhite = step(WHITE_EPS, length(tex.rgb - 1.0));
                return lerp(_BaseColor.rgb, tex.rgb, isWhite);
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float3 baseColor = SampleBaseColor(IN.uv);
                float3 N = normalize(IN.normalWS);
                float3 accumLight = 0;

                InputData inputData = (InputData)0;
                inputData.positionWS = IN.positionWS;
                inputData.normalWS = N;
                inputData.viewDirectionWS = GetWorldSpaceNormalizeViewDir(IN.positionWS);
                inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(IN.positionCS);

                // ===== MAIN LIGHT =====
                Light mainLight = GetMainLight();
                float3 mainDir = normalize(mainLight.direction);
                float NdotMain = saturate(dot(N, -mainDir));

                // Quantize into toon steps
                float steps = max(1.0, _Steps);
                float qMain = floor(NdotMain * steps) / (steps - 1.0);
                qMain = saturate(qMain);
                qMain *= _Brightness;

                accumLight += qMain * mainLight.color.rgb;

                // ===== ADDITIONAL LIGHTS (Forward+ Clustered) =====
                #if defined(_ADDITIONAL_LIGHTS)
                uint pixelLightCount = GetAdditionalLightsCount();
                LIGHT_LOOP_BEGIN(pixelLightCount)
                    Light l = GetAdditionalLight(lightIndex, inputData.positionWS, half4(1,1,1,1));
                    float3 Ldir = normalize(l.direction);
                    float NdotA = saturate(dot(N, Ldir));

                    // Toon quantization
                    float qA = floor(NdotA * steps) / (steps - 1.0);
                    qA = saturate(qA);
                    qA *= _Brightness;

                    // Respect point light attenuation
                    float atten = l.distanceAttenuation;

                    accumLight += qA * l.color.rgb * atten;
                LIGHT_LOOP_END
                #endif

                // ===== FINAL OUTPUT =====
                // Apply minimal ambient clamp
                accumLight = max(accumLight, float3(_MinLight, _MinLight, _MinLight));

                float3 finalCol = baseColor * accumLight;
                return float4(finalCol, _BaseColor.a);
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/Unlit"
}
