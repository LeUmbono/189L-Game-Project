Shader "SteamBarShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        steam_color("Steam Color", Color) = (1.0, 1.0, 1.0, 1.0)
        time_shift("Time Shift", Float) = 0.3
        cloudSize1("Cloud Size 1", Float) = 0.0
        cloudSize2("Cloud Size 2", Float) = 100.0
        brightness("Brightness", Float) = 0.3
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

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float3 steam_color;
            float time_shift;
            float cloudSize1;
            float cloudSize2;
            float brightness;

            float flip(float t)
            {
                return 1.0 - t;
            }

            float lerp(float a, float b, float t)
            {
                return a + (b - a) * t;
            }

            float3 Noise(float2 uv)
            {
                float noise = tex2D(_MainTex, float2(uv.x - _Time.y * time_shift, uv.y)).r;
                noise = noise * lerp(cloudSize1, cloudSize2, uv.x) * brightness; //makes texture into two colors
                return float3(noise, noise, noise);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float food = Noise(i.uv);
                if (food < 0.2)
                {
                    discard;
                }
                return float4(steam_color * float3(Noise(i.uv)), 1.0);
                //float4(Noise(i.uv), 1.0);  //tex2D(_MainTex, i.uv);//float4(Noise(i.uv), 1.0);
            }
            ENDCG
        }
    }
}