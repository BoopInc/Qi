Shader "unityCookie/Introduction/1 - Flat Color" {
	Properties{
		_Color("Main Color", Color) = (1.0,1.0,1.0,1.0)
	}
		SubShader{
			Pass {
				CGPROGRAM

				//Pragmas
				#pragma vertex vert
				#pragma fragment frag

				//user defined variables
				uniform float4 _Color;

					//base input structs
				struct appdata_t {
					float4 vertex : POSITION;
				};

				struct v2f {
					float4 vertex : SV_POSITION;
				};
				//vertex function
				v2f vert(appdata_t v) {
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP,v.vertex);
					return o;
				}
			//frament function
				float4 frag(v2f i) : COLOR
				{
					float4 col = _Color;
					return col;
				}
			ENDCG
		}
	}
	//fallback commented out during development
	//Fallback "Diffuse"
}