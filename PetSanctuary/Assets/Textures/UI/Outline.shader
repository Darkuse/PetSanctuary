Shader "Custom/Outline"
{
    Properties
    {
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth("Outline Width", Float) = 1.0
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        // First Pass: Render the object as usual
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 normal : NORMAL;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = v.normal;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Basic color for the object
                return fixed4(1, 1, 1, 1);
            }
            ENDCG
        }
        // Second Pass: Render the outline
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            float _OutlineWidth;
            fixed4 _OutlineColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 screenPos : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float3 norm = UnityObjectToWorldNormal(v.normal);
                float4 posWorld = mul(unity_ObjectToWorld, v.vertex);

                // Expanding the vertices along their normals
                posWorld.xyz += norm * _OutlineWidth;

                o.pos = UnityObjectToClipPos(posWorld);
                o.screenPos = ComputeScreenPos(o.pos);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Returning the outline color
                return _OutlineColor;
            }
            ENDCG
        }
    }
}
