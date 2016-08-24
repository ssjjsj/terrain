Shader "Unlit/customTerrain"
{
	Properties
	{
		_SandColor ("Sand Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_RockColor ("Rock Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_VegetationColor ("Vegetation Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_ErosionColor ("Erosion Color", Color) = (1.0, 1.0, 1.0, 1.0)

		_SandHeight ("Sand", Vector) = (0, 20, 50)
		_RockHeight ("Rock", Vector) = (30, 100, 150)
		_VegetationHeight ("Vegetation", Vector) = (120, 180, 200)
		_ErosionHeight ("Erosion", Vector) = (190, 220, 255)
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

			float4 _SandHeight;
			float4 _RockHeight; 
			float4 _VegetationHeight; 
			float4 _ErosionHeight;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.height = v.vertex.y;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			float4 heightColor(float height, float4 heightRange, float4 color)
			{
			    float4 col = float4(0, 0, 0, 1);
			    if (height > heightRange.x && height <= heightRange.y)
				{
				    float delta = (height-heightRange.x)/(heightRange.y-heightRange.x);
				    col.r = color.r*delta;
					col.g = color.g*delta;
					col.b = color.b*delta;
				}
				else if (height > heightRange.y && height < heightRange.z)
				{
				    float delta = 1-(height-heightRange.y)/(heightRange.z-heightRange.y);
				    col.r = color.r*delta;
					col.g = color.g*delta;
					col.b = color.b*delta;
				}
				
				return col;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
                float4 col = heightColor(i.height, _SandHeight, _SandColor) +
                            heightColor(i.height, _ErosionHeight, _ErosionColor) +
                            heightColor(i.height, _RockHeight, _RockColor) +
                            heightColor(i.height, _VegetationHeight, _VegetationColor);
                col.a = 1.0;							
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
