// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Matthew/RimShader" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_RimPow("RimPower", Range(0.01, 10)) = 3.0
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader{
			Pass{
			Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
			Blend One One
			Cull Off
			ZWrite Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f {
				float4 pos : SV_POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
				float3 viewDir : TEXCOORD1;
			};

			float4 _Color;
			float _RimPow;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(appdata_full v)
			{
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.normal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal.xyz));
				o.viewDir = normalize(_WorldSpaceCameraPos - mul((float3x3)unity_ObjectToWorld, v.vertex.xyz));
				o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				float t = tex2D(_MainTex, i.uv);
				float val = 1 - abs(dot(i.viewDir, i.normal)) * _RimPow;
				return _Color * _Color.a * val * val * t;
			}

			ENDCG
	}
}
	FallBack "Diffuse"
}
