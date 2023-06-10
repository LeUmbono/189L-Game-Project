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
                // Texture is pebbles with "nice circular bodies of white".
                // Sample the texture, but scroll rightwards.   
                float noise = tex2D(_MainTex, float2(uv.x - _Time.y * time_shift, uv.y)).r;
                // As the texture is more left, make the cloud size bigger.
                // As the texture is more right, make the cloud size smaller.
                // Multiply it according to its brightness.
                noise = noise * lerp(cloudSize1, cloudSize2, uv.x) * brightness; 
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
                // If the pixel is "black enough", discard it.
                float albedo = Noise(i.uv);
                if (albedo < 0.2)
                {
                    discard;
                }
                return float4(steam_color * float3(Noise(i.uv)), 1.0);
            }
            ENDCG
        }
    }
}