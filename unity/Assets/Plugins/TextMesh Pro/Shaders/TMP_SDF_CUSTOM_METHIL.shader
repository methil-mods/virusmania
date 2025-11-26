Shader "TextMeshPro/Distance Field Custom Methil Simplified" {

Properties {
	_FaceTex			("Face Texture", 2D) = "white" {}
	_FaceUVSpeedX		("Face UV Speed X", Range(-5, 5)) = 0.0
	_FaceUVSpeedY		("Face UV Speed Y", Range(-5, 5)) = 0.0
	_FaceColor		    ("Face Color", Color) = (1,1,1,1)
	_FaceDilate			("Face Dilate", Range(-1,1)) = 0

	_OutlineColor	    ("Outline Color", Color) = (0,0,0,1)
	_OutlineTex			("Outline Texture", 2D) = "white" {}
	_OutlineUVSpeedX	("Outline UV Speed X", Range(-5, 5)) = 0.0
	_OutlineUVSpeedY	("Outline UV Speed Y", Range(-5, 5)) = 0.0
	_OutlineWidth		("Outline Thickness", Range(0, 1)) = 0
	_OutlineSoftness	("Outline Softness", Range(0,1)) = 0
	[KeywordEnum(Both, Outer, Inner)] _OutlineMode("Outline Mode", Float) = 0

	_Outline2Color	    ("Outline 2 Color", Color) = (1,0,0,1)
	_Outline2Tex		("Outline 2 Texture", 2D) = "white" {}
	_Outline2UVSpeedX	("Outline 2 UV Speed X", Range(-5, 5)) = 0.0
	_Outline2UVSpeedY	("Outline 2 UV Speed Y", Range(-5, 5)) = 0.0
	_Outline2Width		("Outline 2 Thickness", Range(0, 1)) = 0
	_Outline2Softness	("Outline 2 Softness", Range(0,1)) = 0
	_Outline2OffsetX	("Outline 2 Offset X", Range(-50,50)) = 0
	_Outline2OffsetY	("Outline 2 Offset Y", Range(-50,50)) = 0

	_WeightNormal		("Weight Normal", float) = 0
	_WeightBold			("Weight Bold", float) = 0.5

	_MainTex			("Font Atlas", 2D) = "white" {}
	_TextureWidth		("Texture Width", float) = 512
	_TextureHeight		("Texture Height", float) = 512
	_GradientScale		("Gradient Scale", float) = 5.0
	_ScaleX				("Scale X", float) = 1.0
	_ScaleY				("Scale Y", float) = 1.0
	_PerspectiveFilter	("Perspective Correction", Range(0, 1)) = 0.875
	_Sharpness			("Sharpness", Range(-1,1)) = 0

	_VertexOffsetX		("Vertex OffsetX", float) = 0
	_VertexOffsetY		("Vertex OffsetY", float) = 0

	_MaskCoord			("Mask Coordinates", vector) = (0, 0, 32767, 32767)
	_ClipRect			("Clip Rect", vector) = (-32767, -32767, 32767, 32767)
	_MaskSoftnessX		("Mask SoftnessX", float) = 0
	_MaskSoftnessY		("Mask SoftnessY", float) = 0

	_StencilComp		("Stencil Comparison", Float) = 8
	_Stencil			("Stencil ID", Float) = 0
	_StencilOp			("Stencil Operation", Float) = 0
	_StencilWriteMask	("Stencil Write Mask", Float) = 255
	_StencilReadMask	("Stencil Read Mask", Float) = 255

	_CullMode			("Cull Mode", Float) = 0
	_ColorMask			("Color Mask", Float) = 15
}

SubShader {

	Tags
	{
		"Queue"="Transparent"
		"IgnoreProjector"="True"
		"RenderType"="Transparent"
	}

	Stencil
	{
		Ref [_Stencil]
		Comp [_StencilComp]
		Pass [_StencilOp]
		ReadMask [_StencilReadMask]
		WriteMask [_StencilWriteMask]
	}

	Cull [_CullMode]
	ZWrite Off
	Lighting Off
	Fog { Mode Off }
	ZTest [unity_GUIZTestMode]
	Blend One OneMinusSrcAlpha
	ColorMask [_ColorMask]

	Pass {
		CGPROGRAM
		#pragma target 3.0
		#pragma vertex VertShader
		#pragma fragment PixShader
		#pragma shader_feature __ OUTLINE2_ON
		#pragma shader_feature _OUTLINEMODE_BOTH _OUTLINEMODE_OUTER _OUTLINEMODE_INNER
		#pragma multi_compile __ UNITY_UI_CLIP_RECT
		#pragma multi_compile __ UNITY_UI_ALPHACLIP

		#include "UnityCG.cginc"
		#include "UnityUI.cginc"
		#include "TMPro_Properties.cginc"
		#include "TMPro.cginc"

		struct vertex_t
		{
			UNITY_VERTEX_INPUT_INSTANCE_ID
			float4	position		: POSITION;
			float3	normal			: NORMAL;
			fixed4	color			: COLOR;
			float4	texcoord0		: TEXCOORD0;
			float2	texcoord1		: TEXCOORD1;
		};

		struct pixel_t
		{
			UNITY_VERTEX_INPUT_INSTANCE_ID
			UNITY_VERTEX_OUTPUT_STEREO
			float4	position		: SV_POSITION;
			fixed4	color			: COLOR;
			float2	atlas			: TEXCOORD0;
			float4	param			: TEXCOORD1;
			float4	mask			: TEXCOORD2;
			float3	viewDir			: TEXCOORD3;

		    float4 textures			: TEXCOORD5;
		    
		    #if OUTLINE2_ON
		    float4	texcoord3		: TEXCOORD6;
		    float2  outline2UV		: TEXCOORD7;
		    #endif
		};

		float4 _FaceTex_ST;
		float4 _OutlineTex_ST;
		float4 _Outline2Tex_ST;
		float _UIMaskSoftnessX;
        float _UIMaskSoftnessY;
        int _UIVertexColorAlwaysGammaSpace;

		pixel_t VertShader(vertex_t input)
		{
			pixel_t output;

			UNITY_INITIALIZE_OUTPUT(pixel_t, output);
			UNITY_SETUP_INSTANCE_ID(input);
			UNITY_TRANSFER_INSTANCE_ID(input,output);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

			float bold = step(input.texcoord0.w, 0);

			float4 vert = input.position;
			vert.x += _VertexOffsetX;
			vert.y += _VertexOffsetY;

			float4 vPosition = UnityObjectToClipPos(vert);

			float2 pixelSize = vPosition.w;
			pixelSize /= float2(_ScaleX, _ScaleY) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));
			float scale = rsqrt(dot(pixelSize, pixelSize));
			scale *= abs(input.texcoord0.w) * _GradientScale * (_Sharpness + 1);
			if (UNITY_MATRIX_P[3][3] == 0) scale = lerp(abs(scale) * (1 - _PerspectiveFilter), scale, abs(dot(UnityObjectToWorldNormal(input.normal.xyz), normalize(WorldSpaceViewDir(vert)))));

			float weight = lerp(_WeightNormal, _WeightBold, bold) / 4.0;
			weight = (weight + _FaceDilate) * 0.5;

			float bias =(.5 - weight) + (.5 / scale);

			float alphaClip = (1.0 - _OutlineWidth * 0.5);

			alphaClip = alphaClip / 2.0 - ( .5 / scale) - weight;

			float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
			float2 maskUV = (vert.xy - clampedRect.xy) / (clampedRect.zw - clampedRect.xy);

			float2 textureUV = input.texcoord1;
			float2 faceUV = TRANSFORM_TEX(textureUV, _FaceTex);
			float2 outlineUV = TRANSFORM_TEX(textureUV, _OutlineTex);
			float2 outline2UV = TRANSFORM_TEX(textureUV, _Outline2Tex);

            if (_UIVertexColorAlwaysGammaSpace && !IsGammaSpace())
            {
                input.color.rgb = UIGammaToLinear(input.color.rgb);
            }
			output.position = vPosition;
			output.color = input.color;
			output.atlas =	input.texcoord0;
			output.param =	float4(alphaClip, scale, bias, weight);
			const half2 maskSoftness = half2(max(_UIMaskSoftnessX, _MaskSoftnessX), max(_UIMaskSoftnessY, _MaskSoftnessY));
			output.mask = half4(vert.xy * 2 - clampedRect.xy - clampedRect.zw, 0.25 / (0.25 * maskSoftness + pixelSize.xy));
			output.viewDir =	float3(0,0,0);
			output.textures = float4(faceUV, outlineUV);

			#if OUTLINE2_ON
			float x2 = -(_Outline2OffsetX * 0.5) * _GradientScale / _TextureWidth;
			float y2 = -(_Outline2OffsetY * 0.5) * _GradientScale / _TextureHeight;
			float2 offset2 = float2(x2, y2);
			output.texcoord3 = float4(input.texcoord0 + offset2, scale, bias);
			output.outline2UV = outline2UV;
			#endif

			return output;
		}

		float GetSmoothAlpha(float distance, float scale, float bias)
		{
			float smoothing = 0.5 / scale;
			smoothing = max(smoothing, 0.05);
			return smoothstep(bias - smoothing, bias + smoothing, distance);
		}

		fixed4 PixShader(pixel_t input) : SV_Target
		{
			UNITY_SETUP_INSTANCE_ID(input);

			float c = tex2D(_MainTex, input.atlas).a;

			clip(c - input.param.x);

			float	scale	= input.param.y;
			float	bias	= input.param.z;
			float	weight	= input.param.w;
			float	sd = (bias - c) * scale;

			float outline = 0.5 * scale;
			float softness = 0.5 * scale;

			half4 faceColor = _FaceColor;
			half4 outlineColor = _OutlineColor;

			faceColor.rgb *= input.color.rgb;

			faceColor *= tex2D(_FaceTex, input.textures.xy + float2(_FaceUVSpeedX, _FaceUVSpeedY) * _Time.y);
			outlineColor *= tex2D(_OutlineTex, input.textures.zw + float2(_OutlineUVSpeedX, _OutlineUVSpeedY) * _Time.y);

			#if _OUTLINEMODE_OUTER
			float outerSD = sd + outline * 0.5;
			faceColor = GetColor(outerSD, faceColor, outlineColor, outline, softness);
			#elif _OUTLINEMODE_INNER
			float innerSD = sd - outline * 0.5;
			faceColor = GetColor(innerSD, faceColor, outlineColor, outline, softness);
			#else
			faceColor = GetColor(sd, faceColor, outlineColor, outline, softness);
			#endif

		    #if OUTLINE2_ON
			float2 atlasUV3 = input.texcoord3.xy;
			bool atlasUV3Inside = (atlasUV3.x >= 0.0 && atlasUV3.x <= 1.0 && atlasUV3.y >= 0.0 && atlasUV3.y <= 1.0);
			float outline2C = atlasUV3Inside ? tex2D(_MainTex, atlasUV3).a : 0.0;
			float outline2SD = (input.texcoord3.w - outline2C) * input.texcoord3.z;

			float outline2Thickness = 0.5 * scale;
			float outline2Softness = 0.5 * scale;
			outline2Softness = max(outline2Softness, 0.5 / scale);

			half4 outline2FaceColor = _Outline2Color;
			half4 outline2OutlineColor = _Outline2Color;

			float2 outline2TexUV = input.outline2UV + float2(_Outline2UVSpeedX, _Outline2UVSpeedY) * _Time.y;

			bool outline2TexUVInside = (outline2TexUV.x >= 0.0 && outline2TexUV.x <= 1.0 && outline2TexUV.y >= 0.0 && outline2TexUV.y <= 1.0);
			if (outline2TexUVInside)
			{
				outline2FaceColor *= tex2D(_Outline2Tex, outline2TexUV);
				outline2OutlineColor = outline2FaceColor;
			}
			else
			{
				outline2FaceColor = half4(0,0,0,0);
				outline2OutlineColor = half4(0,0,0,0);
			}

			fixed4 outline2Color;
			#if _OUTLINEMODE_OUTER
			float outline2OuterSD = outline2SD + outline2Thickness * 0.5;
			outline2Color = GetColor(outline2OuterSD, outline2FaceColor, outline2OutlineColor, outline2Thickness, outline2Softness);
			#elif _OUTLINEMODE_INNER
			float outline2InnerSD = outline2SD - outline2Thickness * 0.5;
			outline2Color = GetColor(outline2InnerSD, outline2FaceColor, outline2OutlineColor, outline2Thickness, outline2Softness);
			#else
			outline2Color = GetColor(outline2SD, outline2FaceColor, outline2OutlineColor, outline2Thickness, outline2Softness);
			#endif

			faceColor.rgb = faceColor.rgb + outline2Color.rgb * (1.0 - faceColor.a);
			faceColor.a = saturate(faceColor.a + outline2Color.a * (1.0 - faceColor.a));
		    #endif

		    #if UNITY_UI_CLIP_RECT
			half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(input.mask.xy)) * input.mask.zw);
			faceColor *= m.x * m.y;
		    #endif

		    #if UNITY_UI_ALPHACLIP
			clip(faceColor.a - 0.001);
		    #endif

  		    return faceColor * input.color.a;
		}
		ENDCG
	}
}

Fallback "TextMeshPro/Mobile/Distance Field"
CustomEditor "Plugins.TextMesh_Pro.Utils.TMP_SDFShaderGUI"
}
