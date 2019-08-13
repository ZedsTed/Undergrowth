Shader "Unlit/AlphaMask" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_AlphaTex ("Alpha mask (R)", 2D) = "white" {}
	}

		SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass {  
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

		struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord0 : TEXCOORD0;
		float2 texcoord1 : TEXCOORD1;
	};

	struct v2f {
		float4 vertex : SV_POSITION;
		half2 texcoord0 : TEXCOORD0;
		half2 texcoord1 : TEXCOORD1;
	};

	fixed4 _Color;
	sampler2D _MainTex;
	sampler2D _AlphaTex;

	float4 _MainTex_ST;
	float4 _AlphaTex_ST;

	v2f vert (appdata_t v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.texcoord0 = TRANSFORM_TEX(v.texcoord0, _MainTex);
		o.texcoord1 = TRANSFORM_TEX(v.texcoord1, _AlphaTex);

		return o;
	}

	fixed4 frag (v2f i) : SV_Target
	{
		fixed3 col = _Color.rgb;
		fixed4 alpha1 = tex2D(_MainTex, i.texcoord0);
		fixed4 alpha2 = tex2D(_AlphaTex, i.texcoord1);

	return fixed4(col.r, col.g, col.b, saturate(mul(alpha1.a,alpha2.r)));
	}
		ENDCG
	}
	}

}