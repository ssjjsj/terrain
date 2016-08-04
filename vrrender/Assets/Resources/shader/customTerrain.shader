Shader "Unlit/customTerrain"
{
	Properties
	{
		_SandColor ("Sand Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_RockColor ("Rock Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_VegetationColor ("Vegetation Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_ErosionColor ("Erosion Color", Color) = (1.0, 1.0, 1.0, 1.0)

		_SandHeight ("Sand", Float) = 1.0
		_RockHeight ("Rock", Float) = 1.0
		_VegetationHeight ("Vegetation", Float) = 1.0
		_ErosionHeight ("Erosion", Float) = 1.0
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }
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
				float height : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			float4	_SandColor;
			float4 _RockColor; 
			float4 _VegetationColor; 
			float4 _ErosionColor;

			float _SandHeight;
			float _RockHeight; 
			float _VegetationHeight; 
			float _ErosionHeight;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.height = v.vertex.y;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = _ErosionColor;
				if (i.height<_RockHeight)
				{
				    float delta = (i.height-_SandHeight)/(_RockHeight-_SandHeight);
				    col.r = lerp(_SandColor.r, _RockColor.r, delta);
					col.g = lerp(_SandColor.g, _RockColor.g, delta);
					col.b = lerp(_SandColor.b, _RockColor.b, delta);
				}
				else if (i.height <_VegetationHeight)
				{
					float delta = (i.height-_RockColor)/(_VegetationHeight-_RockHeight);
				    col.r = lerp(_RockColor.r, _VegetationColor.r, delta);
					col.g = lerp(_RockColor.g, _VegetationColor.g, delta);
					col.b = lerp(_RockColor.b, _VegetationColor.b, delta);
				}
				else if (i.height <_ErosionHeight)
				{
				    float delta = (i.height-_VegetationHeight)/(_ErosionHeight-_VegetationHeight);
				    col.r = lerp(_VegetationColor.r, _ErosionColor.r, delta);
					col.g = lerp(_VegetationColor.g, _ErosionColor.g, delta);
					col.b = lerp(_VegetationColor.b, _ErosionColor.b, delta);
				}
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
