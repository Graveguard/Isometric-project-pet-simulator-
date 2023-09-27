// MatCap Shader, (c) 2015-2019 Jean Moreno
Shader "Custom/Matcap Emission"
{
    Properties
    {
        _Color("Color", Color) = (1.000000,1.000000,1.000000,1.000000)
        _MainTex("Base (RGB)", 2D) = "white" {}
        _MatCap("MatCap (RGB)", 2D) = "white" {}
        _EmissionTex ("Emission (RGB)", 2D)= "white"{}
        [Toggle]_EToggle("Emission",float) = 0
        _EmissionSlider("Emission Slider", Range(0,1)) = 0.5

    }

        Subshader
    {
        Tags { "RenderType" = "Opaque" }

        Pass
        {
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                #pragma multi_compile_fog
                #include "UnityCG.cginc"

                struct v2f
                {
                    float4 pos  : SV_POSITION;
                    float2 uv   : TEXCOORD0;
                    float2 cap  : TEXCOORD1;
                    UNITY_FOG_COORDS(2)
                };

                uniform float4 _MainTex_ST;
                uniform float4 _EmissionTex_ST;

                v2f vert(appdata_base v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                    half2 capCoord;

                    float3 worldNorm = normalize(unity_WorldToObject[0].xyz * v.normal.x + unity_WorldToObject[1].xyz * v.normal.y + unity_WorldToObject[2].xyz * v.normal.z);
                    worldNorm = mul((float3x3)UNITY_MATRIX_V, worldNorm);
                    o.cap.xy = worldNorm.xy * 0.5 + 0.5;

                    UNITY_TRANSFER_FOG(o, o.pos);
                    return o;
                }
                
                uniform sampler2D _MainTex;
                uniform sampler2D _MatCap;
                uniform sampler2D _EmissionTex;
                bool _EToggle;
                half _EmissionSlider;
                float4  _Color;

                fixed4 frag(v2f i) : COLOR
                {
                    fixed4 emis = tex2D(_EmissionTex,i.uv);
                    fixed4 tex = tex2D(_MainTex, i.uv);
                    fixed4 mc = tex2D(_MatCap, i.cap) * tex * unity_ColorSpaceDouble;
                    mc = mc * _Color;

                    if (_EToggle)
                    {
                        mc = mc + (emis*_EmissionSlider);

                        UNITY_APPLY_FOG(i.fogCoord, mc);
                        return mc;
                    }

                    else
                    {

                        UNITY_APPLY_FOG(i.fogCoord, mc);
                        return mc;
                    }
                  
                }
            ENDCG
        }
    }

        Fallback "VertexLit"
}
