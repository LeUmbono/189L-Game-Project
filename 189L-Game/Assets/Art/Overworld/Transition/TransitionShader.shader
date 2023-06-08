Shader "Custom/TransitionShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CutoffTex("Cutoff Texture", 2D) = "grey" {}
        _Color("Color", Color) = (0.0, 0.0, 0.0, 0.0)
        _Cutoff("Cutoff", Range(0,1)) = 1
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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _CutoffTex;
            float4 _Color;
            float _Cutoff;

            // Default vertex function
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Sample the "cutoff texture"
                fixed4 transit = tex2D(_CutoffTex, i.uv);
                
                // Since "transit" is animated overtime, the more "dark" a pixel is, 
                // the longer it takes to be turned black.
                // The end effect is a screen gradually turning black according
                // to the given texture file.
                if (transit.b < _Cutoff)
                    return _Color;

                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
