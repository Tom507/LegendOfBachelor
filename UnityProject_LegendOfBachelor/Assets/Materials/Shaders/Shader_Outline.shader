Shader "Custom/Shader_Outline" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_ColorOutline ("Color Outline", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Push ("Push", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Geometry"}
		LOD 200

		Cull Back
		Stencil {
			Ref 1 
			Comp always
			Pass replace
		}

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG

		Cull Front
		Stencil {
			Ref 1 
			Comp notequal
			Pass replace
		}

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows vertex:vert
		#pragma target 3.0

		sampler2D _MainTex;
		half _Push;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _ColorOutline;

		void vert (inout appdata_full v)
		{
			v.vertex.xyz += v.normal * _Push;
			v.normal *= -1;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			o.Albedo = _ColorOutline.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
