Shader "Hidden/Shader_PlaceCircle"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // Enable depth writing and depth testing
        Cull Off
        ZWrite On
        ZTest LEqual

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // Enable transparency
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Alpha;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // draw circle with center
                float2 center = float2(0.5, 0.5);
                float radius = 0.5f;
                float dis = distance(i.uv, center);
                if (dis > radius)
                {
                    discard;
                }

                col.a = 0.2f;
                col.rgb = 1.0f;
                return col;
            }
            ENDCG
        }
    }
}
