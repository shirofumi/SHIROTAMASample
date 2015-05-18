Shader "Custom/Ball"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_EdgeThickness ("Edge Thickness", Range(0, 0.1)) = 0.025
		_EdgeColor ("Edge Color", Color) = (0, 0, 0, 1)
		_ToonShadeColor ("ToonShade Color", Color) = (0.5, 0.5, 0.5, 1)
		_ToonShadeThreshold ("ToonShade Threshold", Range(0, 1)) = 0.25
		_ToonShadeTransition ("ToonShade Transition", Range(0, 1)) = 0.25
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Geometry"
			"IgnoreProjector"="True" 
		}

		Cull Back
		ZWrite On
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf BallToon noambient keepalpha
		
		half4 _Color;
		half4 _ToonShadeColor;
		half _ToonShadeThreshold;
		half _ToonShadeTransition;
		
		inline half4 LightingBallToon (SurfaceOutput s, half3 lightDir, half atten)
		{
			#ifndef USING_DIRECTIONAL_LIGHT
			lightDir = normalize(lightDir);
			#endif
			
			half d = dot(s.Normal, lightDir) * 0.5 + 0.5;
			half t = saturate((d - _ToonShadeThreshold) / _ToonShadeTransition);

			half4 c;
			c.rgb = (s.Albedo * t + _ToonShadeColor.rgb * _Color.rgb * (1.0 - t)) * 0.5f;
			c.a = s.Alpha;
			return c;
		}
		
		sampler2D _MainTex;
		
		struct Input {
			half2 uv_MainTex : TEXCOORD0;
		};
		
		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}

		ENDCG

		Pass
		{
			Cull Front
			ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			float _EdgeThickness;
			half4 _EdgeColor;
			half4 _Color;

			struct Input{
				float4 pos : POSITION;
				float3 normal : NORMAL;
			};

			struct Output{
				float4 pos : POSITION;
			};
			
			Output vert(Input i){
				Output o;
				o.pos = mul(UNITY_MATRIX_MVP, i.pos);
				float3 n = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, i.normal));
				o.pos.xy += TransformViewToProjection(n.xy) * _EdgeThickness;
				return o;
			}

			half4 frag() : COLOR{
				return _EdgeColor * _Color;
			}
			ENDCG
		}
	}
}