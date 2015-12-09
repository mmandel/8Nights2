// Shader created with Shader Forge v1.21 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.21;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:3138,x:32657,y:32701,varname:node_3138,prsc:2|emission-3417-OUT,alpha-8994-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32003,y:32625,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843138,c2:0.3921569,c3:0.7843137,c4:1;n:type:ShaderForge.SFN_VertexColor,id:389,x:32003,y:32785,varname:node_389,prsc:2;n:type:ShaderForge.SFN_Multiply,id:8994,x:32320,y:33013,varname:node_8994,prsc:2|A-389-A,B-4435-OUT;n:type:ShaderForge.SFN_Tex2d,id:7520,x:31734,y:33117,ptovrint:False,ptlb:particle shape,ptin:_particleshape,varname:node_7520,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:b8f33de70f20e98449d54ed37528898c,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:6193,x:31551,y:32877,ptovrint:False,ptlb:flicker gradient,ptin:_flickergradient,varname:node_6193,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:33808ab004e674f428e41cb59df35a2b,ntxv:0,isnm:False|UVIN-9491-OUT;n:type:ShaderForge.SFN_Panner,id:4028,x:31172,y:32790,varname:node_4028,prsc:2,spu:0.1,spv:0|UVIN-2114-OUT;n:type:ShaderForge.SFN_Multiply,id:4435,x:31965,y:33037,varname:node_4435,prsc:2|A-2127-OUT,B-7520-A;n:type:ShaderForge.SFN_Vector2,id:2114,x:31004,y:32790,varname:node_2114,prsc:2,v1:0,v2:0;n:type:ShaderForge.SFN_FragmentPosition,id:3169,x:30801,y:32992,varname:node_3169,prsc:2;n:type:ShaderForge.SFN_Add,id:9491,x:31383,y:32877,varname:node_9491,prsc:2|A-4028-UVOUT,B-8443-OUT;n:type:ShaderForge.SFN_Multiply,id:8443,x:31172,y:32966,varname:node_8443,prsc:2|A-8567-OUT,B-6694-OUT;n:type:ShaderForge.SFN_Add,id:6694,x:30988,y:33014,varname:node_6694,prsc:2|A-3169-X,B-3169-Y,C-3169-Z;n:type:ShaderForge.SFN_Multiply,id:2127,x:31734,y:32959,varname:node_2127,prsc:2|A-6193-R,B-2379-OUT;n:type:ShaderForge.SFN_Vector1,id:2379,x:31551,y:33047,varname:node_2379,prsc:2,v1:6;n:type:ShaderForge.SFN_ValueProperty,id:8567,x:30988,y:32951,ptovrint:False,ptlb:flicker_scale,ptin:_flicker_scale,varname:node_8567,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3.2;n:type:ShaderForge.SFN_Multiply,id:3417,x:32324,y:32636,varname:node_3417,prsc:2|A-7241-RGB,B-389-RGB,C-5796-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5796,x:32160,y:32829,ptovrint:False,ptlb:overbloom,ptin:_overbloom,varname:node_5796,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1.2;proporder:7241-7520-6193-8567-5796;pass:END;sub:END;*/

Shader "Shader Forge/p_torch_newtrail" {
    Properties {
        _Color ("Color", Color) = (0.07843138,0.3921569,0.7843137,1)
        _particleshape ("particle shape", 2D) = "white" {}
        _flickergradient ("flicker gradient", 2D) = "white" {}
        _flicker_scale ("flicker_scale", Float ) = 3.2
        _overbloom ("overbloom", Float ) = 1.2
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
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
            Blend SrcAlpha OneMinusSrcAlpha
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
            uniform sampler2D _flickergradient; uniform float4 _flickergradient_ST;
            uniform float _flicker_scale;
            uniform float _overbloom;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
////// Lighting:
////// Emissive:
                float3 emissive = (_Color.rgb*i.vertexColor.rgb*_overbloom);
                float3 finalColor = emissive;
                float4 node_3805 = _Time + _TimeEditor;
                float2 node_9491 = ((float2(0,0)+node_3805.g*float2(0.1,0))+(_flicker_scale*(i.posWorld.r+i.posWorld.g+i.posWorld.b)));
                float4 _flickergradient_var = tex2D(_flickergradient,TRANSFORM_TEX(node_9491, _flickergradient));
                float4 _particleshape_var = tex2D(_particleshape,TRANSFORM_TEX(i.uv0, _particleshape));
                return fixed4(finalColor,(i.vertexColor.a*((_flickergradient_var.r*6.0)*_particleshape_var.a)));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
