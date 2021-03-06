Shader "Unlit/UnlitOutline"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _Outline ("Outline", Color) = (0.1,0.1,0.1,1)
		_OutlineWidth ("OutlineWidth", float) = 0.02
        _Color ("Color", Color) = (1,1,1,1)
        _Lightness ("Lightness", float) = 0.0
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}

        Blend One OneMinusSrcAlpha
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
            fixed4 _Color;
		    fixed4 _Outline;
		    float _OutlineWidth;
            float _Lightness;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
                // calculate UV distance
			    float distY = abs(i.uv.y - .5) * 2;

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) * _Color;

                if (distY > (1 - _OutlineWidth)){
				    col = _Outline;
			    }

                fixed4 diff = fixed4(1, 1, 1, 0) - col;

                col = col + (diff * _Lightness);

                //col.w = 1 - _Lightness;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
