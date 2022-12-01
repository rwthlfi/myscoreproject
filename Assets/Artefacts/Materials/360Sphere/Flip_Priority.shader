// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Based on Unlit shader, but culls the front faces instead of the back

Shader "FlipPriority" {
    Properties{
        _MainTex("Base (RGB)", 2D) = "white" { }
    }

        SubShader{
            Tags { "Queue" = "Overlay+1" }
            ZTest Always
            Cull front    // Flipping Normals VR360TV
        LOD 100

        Pass {
                CGPROGRAM
                #pragma vertex vert
    #pragma fragment frag
    # include "UnityCG.cginc"

    struct appdata
    {
        float4 vertex : POSITION;
                    float2 texcoord : TEXCOORD0;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct v2f
    {
        float4 vertex : SV_POSITION;
                    half2 texcoord : TEXCOORD0;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                    UNITY_VERTEX_OUTPUT_STEREO
                };

    sampler2D _MainTex;
    float4 _MainTex_ST;

    v2f vert(appdata v)
    {
        v2f o;

        UNITY_SETUP_INSTANCE_ID(v);
        UNITY_INITIALIZE_OUTPUT(v2f, o);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

        o.vertex = UnityObjectToClipPos(v.vertex);
        v.texcoord.x = 1 - v.texcoord.x;
        o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
        return o;
    }

    fixed4 frag(v2f i) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
        fixed4 col = tex2D(_MainTex, i.texcoord);
        return col;
    }
    ENDCG
        }
    }

}