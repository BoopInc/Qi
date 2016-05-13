Shader "Fixed Unlit" {
	Properties{
		_Color("Main Color", Color) = (1.0,1.0,1.0,1.0)
	}

	SubShader
	{
		Pass
		{

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		struct appdata {
			float4 vertex : POSITION;
		};

		struct v2f {
			float4 pos : SV_POSITION;
		};

		float4 _Color;

		v2f vert(appdata IN) {
			v2f OUT;
			OUT.pos = mul(UNITY_MATRIX_MVP, IN.vertex);
			return OUT;
		}

		float4 frag(v2f IN) : COLOR {
			return _Color;
		}

		ENDCG

		}
	}
}