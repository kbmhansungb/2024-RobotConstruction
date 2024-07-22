Shader "Hidden/Shader_PlaceCircleHide"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

                // sin(시간)에 따른 투명도 조절
                float time = _Time.y;
                _Alpha = 0.5f + 0.5f * sin(time * 2 * 3.141592);
                
                col.a = _Alpha;
                col.rgb = 0.05f;

                // depth

                return col;
            }
            ENDCG
        }
    }
}
