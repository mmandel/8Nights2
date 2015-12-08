// Shader created with Shader Forge v1.21 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.21;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:3138,x:32657,y:32800,varname:node_3138,prsc:2|emission-7394-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:30750,y:32413,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843138,c2:0.3921569,c3:0.7843137,c4:1;n:type:ShaderForge.SFN_VertexColor,id:389,x:30750,y:32553,varname:node_389,prsc:2;n:type:ShaderForge.SFN_Multiply,id:8994,x:32240,y:32801,varname:node_8994,prsc:2|A-7241-RGB,B-389-RGB,C-389-A,D-7909-OUT;n:type:ShaderForge.SFN_Tex2d,id:7520,x:31236,y:32827,ptovrint:False,ptlb:particle shape,ptin:_particleshape,varname:node_7520,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:89eaf17cf1b75a3478de20e698d57f15,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:1734,x:31038,y:33149,ptovrint:False,ptlb:p_filler,ptin:_p_filler,varname:node_1734,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:f2177c5cac1fc9f4b8d6cb423a4e00d1,ntxv:0,isnm:False|UVIN-7715-UVOUT;n:type:ShaderForge.SFN_Rotator,id:7715,x:30861,y:33149,varname:node_7715,prsc:2|SPD-6361-OUT;n:type:ShaderForge.SFN_Vector1,id:6361,x:30690,y:33209,varname:node_6361,prsc:2,v1:4;n:type:ShaderForge.SFN_Add,id:7909,x:31894,y:32991,varname:node_7909,prsc:2|A-4007-OUT,B-2608-OUT;n:type:ShaderForge.SFN_Multiply,id:3994,x:31894,y:33182,varname:node_3994,prsc:2|A-1734-G,B-6652-OUT;n:type:ShaderForge.SFN_Multiply,id:4007,x:31557,y:32890,varname:node_4007,prsc:2|A-6306-OUT,B-7520-A;n:type:ShaderForge.SFN_Vector1,id:6306,x:31389,y:32858,varname:node_6306,prsc:2,v1:0.4;n:type:ShaderForge.SFN_Add,id:7394,x:32432,y:32897,varname:node_7394,prsc:2|A-8994-OUT,B-3994-OUT;n:type:ShaderForge.SFN_Tex2d,id:4920,x:30932,y:33375,ptovrint:False,ptlb:flicker_gradient,ptin:_flicker_gradient,varname:node_4920,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:33808ab004e674f428e41cb59df35a2b,ntxv:0,isnm:False|UVIN-7764-UVOUT;n:type:ShaderForge.SFN_Panner,id:7764,x:30762,y:33375,varname:node_7764,prsc:2,spu:0.2,spv:0.1|UVIN-1572-OUT;n:type:ShaderForge.SFN_Vector2,id:1115,x:30427,y:33473,varname:node_1115,prsc:2,v1:0,v2:0;n:type:ShaderForge.SFN_RemapRange,id:6652,x:31148,y:33473,varname:node_6652,prsc:2,frmn:0,frmx:1,tomn:0.3,tomx:1|IN-4920-R;n:type:ShaderForge.SFN_Add,id:1572,x:30601,y:33375,varname:node_1572,prsc:2|A-8488-OUT,B-1115-OUT;n:type:ShaderForge.SFN_RemapRange,id:7052,x:31148,y:33301,varname:node_7052,prsc:2,frmn:0,frmx:1,tomn:0.8,tomx:1.2|IN-4920-R;n:type:ShaderForge.SFN_Multiply,id:2608,x:31557,y:33015,varname:node_2608,prsc:2|A-1734-R,B-7052-OUT;n:type:ShaderForge.SFN_Multiply,id:8488,x:30315,y:33220,varname:node_8488,prsc:2|A-389-R,B-30-OUT;n:type:ShaderForge.SFN_Vector1,id:30,x:30154,y:33278,varname:node_30,prsc:2,v1:0.6;proporder:7241-7520-1734-4920;pass:END;sub:END;*/

Shader "Shader Forge/p_rainbow_rain" {
    Properties {
        _Color ("Color", Color) = (0.07843138,0.3921569,0.7843137,1)
        _particleshape ("particle shape", 2D) = "white" {}
        _p_filler ("p_filler", 2D) = "white" {}
        _flicker_gradient ("flicker_gradient", 2D) = "white" {}
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
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform sampler2D _particleshape; uniform float4 _particleshape_ST;
            uniform sampler2D _p_filler; uniform float4 _p_filler_ST;
            uniform sampler2D _flicker_gradient; uniform float4 _flicker_gradient_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
////// Lighting:
////// Emissive:
                float4 _particleshape_var = tex2D(_particleshape,TRANSFORM_TEX(i.uv0, _particleshape));
                float4 node_1022 = _Time + _TimeEditor;
                float node_7715_ang = node_1022.g;
                float node_7715_spd = 4.0;
                float node_7715_cos = cos(node_7715_spd*node_7715_ang);
                float node_7715_sin = sin(node_7715_spd*node_7715_ang);
                float2 node_7715_piv = float2(0.5,0.5);
                float2 node_7715 = (mul(i.uv0-node_7715_piv,float2x2( node_7715_cos, -node_7715_sin, node_7715_sin, node_7715_cos))+node_7715_piv);
                float4 _p_filler_var = tex2D(_p_filler,TRANSFORM_TEX(node_7715, _p_filler));
                float2 node_7764 = (((i.vertexColor.r*0.6)+float2(0,0))+node_1022.g*float2(0.2,0.1));
                float4 _flicker_gradient_var = tex2D(_flicker_gradient,TRANSFORM_TEX(node_7764, _flicker_gradient));
                float3 node_8994 = (_Color.rgb*i.vertexColor.rgb*i.vertexColor.a*((0.4*_particleshape_var.a)+(_p_filler_var.r*(_flicker_gradient_var.r*0.4+0.8))));
                float3 emissive = (node_8994+(_p_filler_var.g*(_flicker_gradient_var.r*0.7+0.3)));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
