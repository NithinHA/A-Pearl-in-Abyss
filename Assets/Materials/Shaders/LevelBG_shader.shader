Shader "Custom/LevelBG_shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Colour", Color) = (1,1,1,1)
        _Scale ("Scale", float) = 1
        _Speed ("Speed", float) = 1
        _Frequency ("Frequency", float) = 1
    }
    SubShader
    {
        Pass
        {
            Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		    Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            LOD 100

                CGPROGRAM
                //#pragma surface surf Lambert alpha:fade
                #include "UnityCG.cginc"
                
                #pragma vertex vert
                #pragma fragment frag

                sampler2D _MainTex;
                float4 _MainTex_ST;
                half4 _Color;
                float _Scale, _Speed, _Frequency;

                //float _WaveAmplitude_1;
                //float _WaveAmplitude_2;
                //float _WaveAmplitude_3;
                //float _WaveAmplitude_4;
                //float _WaveAmplitude_5;
                //float _WaveAmplitude_6;
                //float _WaveAmplitude_7;
                //float _WaveAmplitude_8;

                //float _OffsetX_1;
                //float _OffsetX_2;
                //float _OffsetX_3;
                //float _OffsetX_4;
                //float _OffsetX_5;
                //float _OffsetX_6;
                //float _OffsetX_7;
                //float _OffsetX_8;

                //float _OffsetY_1;
                //float _OffsetY_2;
                //float _OffsetY_3;
                //float _OffsetY_4;
                //float _OffsetY_5;
                //float _OffsetY_6;
                //float _OffsetY_7;
                //float _OffsetY_8;

                struct Input
                {
                    float2 uv_MainTex;
                };

                //void surf(Input IN, inout SurfaceOutput o){
                //    fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
                //    o.Albedo = c.rgb * _Color.rgb;
                //    o.Alpha = c.a;
                //}
                struct appdata{
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };
                struct v2f{
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                    //float4 color : COLOR;
                };

                v2f vert (appdata v)
                {
                    v2f o;
                    half offsetVert = v.vertex.x * v.vertex.x + v.vertex.y * v.vertex.y;
                
                    //half value1 = _Scale * sin(_Time.w * _Speed + offsetVert * _Frequency + (v.vertex.x * _OffsetX_1) + (v.vertex.y * _OffsetY_1));
                    //half value2 = _Scale * sin(_Time.w * _Speed + offsetVert * _Frequency + (v.vertex.x * _OffsetX_2) + (v.vertex.y * _OffsetY_2));
                    //half value3 = _Scale * sin(_Time.w * _Speed + offsetVert * _Frequency + (v.vertex.x * _OffsetX_3) + (v.vertex.y * _OffsetY_3));
                    //half value4 = _Scale * sin(_Time.w * _Speed + offsetVert * _Frequency + (v.vertex.x * _OffsetX_4) + (v.vertex.y * _OffsetY_4));
                    //half value5 = _Scale * sin(_Time.w * _Speed + offsetVert * _Frequency + (v.vertex.x * _OffsetX_5) + (v.vertex.y * _OffsetY_5));
                    //half value6 = _Scale * sin(_Time.w * _Speed + offsetVert * _Frequency + (v.vertex.x * _OffsetX_6) + (v.vertex.y * _OffsetY_6));
                    //half value7 = _Scale * sin(_Time.w * _Speed + offsetVert * _Frequency + (v.vertex.x * _OffsetX_7) + (v.vertex.y * _OffsetY_7));
                    //half value8 = _Scale * sin(_Time.w * _Speed + offsetVert * _Frequency + (v.vertex.x * _OffsetX_8) + (v.vertex.y * _OffsetY_8));
                    
                    half value = _Scale * sin(_Time.w * _Speed + offsetVert * _Frequency + (v.vertex.x) + (v.vertex.y));
                    //half value1 = _Scale * sin(_Time.w * _Speed + _offsetVert * _Frequency + (v.vertex.x * _OffsetX_4) + (v.vertex.y * _OffsetY_4));

                    //v.vertex.z -= value1 * _WaveAmplitude_1;
                    //v.vertex.z -= value2 * _WaveAmplitude_2;
                    //v.vertex.z -= value3 * _WaveAmplitude_3;
                    //v.vertex.z -= value4 * _WaveAmplitude_4;
                    //v.vertex.z -= value5 * _WaveAmplitude_5;
                    //v.vertex.z -= value6 * _WaveAmplitude_6;
                    //v.vertex.z -= value7 * _WaveAmplitude_7;
                    //v.vertex.z -= value8 * _WaveAmplitude_8;
                    v.vertex.z += value * .4;
                    //v.vertex.z += value1 * _WaveAmplitude_4;
                    
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    //o.vertex.z += value  * _WaveAmplitude_4;
                    //o.color.bg = (v.vertex.y  +.2) *2;
                    //o.color.b = (v.vertex.x  +.2) *2;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                    return col;
                }

                ENDCG
        }
    }
        FallBack "Diffuse"
}
