// Shader created with Shader Forge v1.21 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.21;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:3138,x:33397,y:32797,varname:node_3138,prsc:2|emission-4562-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32572,y:32730,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843138,c2:0.3921569,c3:0.7843137,c4:1;n:type:ShaderForge.SFN_Tex2d,id:9574,x:32572,y:32896,ptovrint:False,ptlb:flare tex,ptin:_flaretex,varname:node_9574,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:1a83f200746603247acf9762ff97e7d4,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:4562,x:33142,y:32900,varname:node_4562,prsc:2|A-7241-RGB,B-9574-R,C-15-OUT,D-5322-OUT;n:type:ShaderForge.SFN_TexCoord,id:7009,x:32120,y:33032,varname:node_7009,prsc:2,uv:0;n:type:ShaderForge.SFN_Distance,id:7433,x:32316,y:33109,varname:node_7433,prsc:2|A-7009-UVOUT,B-2779-OUT;n:type:ShaderForge.SFN_Vector1,id:2779,x:32120,y:33177,varname:node_2779,prsc:2,v1:0.5;n:type:ShaderForge.SFN_OneMinus,id:695,x:32541,y:33109,varname:node_695,prsc:2|IN-7433-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4156,x:32591,y:33300,ptovrint:False,ptlb:Overbloom,ptin:_Overbloom,varname:node_4156,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:6;n:type:ShaderForge.SFN_Multiply,id:15,x:32815,y:32952,varname:node_15,prsc:2|A-695-OUT,B-4156-OUT;n:type:ShaderForge.SFN_Slider,id:5322,x:32842,y:33115,ptovrint:False,ptlb:Alpha,ptin:_Alpha,varname:node_5322,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;proporder:7241-9574-4156-5322;pass:END;sub:END;*/

Shader "Shader Forge/icon_flare" {
    Properties {
        _Color ("Color", Color) = (0.07843138,0.3921569,0.7843137,1)
        _flaretex ("flare tex", 2D) = "white" {}
        _Overbloom ("Overbloom", Float ) = 6
        _Alpha ("Alpha", Range(0, 1)) = 1
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _Color;
            uniform sampler2D _flaretex; uniform float4 _flaretex_ST;
            uniform float _Overbloom;
            uniform float _Alpha;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
////// Lighting:
////// Emissive:
                float4 _flaretex_var = tex2D(_flaretex,TRANSFORM_TEX(i.uv0, _flaretex));
                float3 emissive = (_Color.rgb*_flaretex_var.r*((1.0 - distance(i.uv0,0.5))*_Overbloom)*_Alpha);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
