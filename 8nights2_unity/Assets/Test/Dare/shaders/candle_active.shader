// Shader created with Shader Forge v1.21 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.21;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:3138,x:32627,y:32492,varname:node_3138,prsc:2|emission-8641-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:31427,y:32609,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843138,c2:0.3921569,c3:0.7843137,c4:1;n:type:ShaderForge.SFN_TexCoord,id:2195,x:31588,y:32464,varname:node_2195,prsc:2,uv:0;n:type:ShaderForge.SFN_OneMinus,id:6878,x:31743,y:32491,varname:node_6878,prsc:2|IN-2195-V;n:type:ShaderForge.SFN_Multiply,id:8028,x:32199,y:32591,varname:node_8028,prsc:2|A-6878-OUT,B-7241-RGB,C-3372-OUT,D-2413-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3372,x:31663,y:32675,ptovrint:False,ptlb:brightness,ptin:_brightness,varname:node_3372,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3;n:type:ShaderForge.SFN_Tex2d,id:1320,x:31729,y:32830,varname:node_1320,prsc:2,tex:4e488caea5ebb1c4c942fb5df9e6260c,ntxv:0,isnm:False|UVIN-2657-UVOUT,TEX-7507-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:7507,x:30921,y:33316,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:node_7507,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:4e488caea5ebb1c4c942fb5df9e6260c,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:4538,x:31010,y:33109,varname:node_4538,prsc:2,tex:4e488caea5ebb1c4c942fb5df9e6260c,ntxv:0,isnm:False|UVIN-339-OUT,TEX-7507-TEX;n:type:ShaderForge.SFN_Multiply,id:339,x:30796,y:33109,varname:node_339,prsc:2|A-1601-UVOUT,B-5622-OUT;n:type:ShaderForge.SFN_Vector2,id:5622,x:30547,y:33165,varname:node_5622,prsc:2,v1:1,v2:3;n:type:ShaderForge.SFN_Panner,id:1601,x:30547,y:33026,varname:node_1601,prsc:2,spu:0.1,spv:-0.05;n:type:ShaderForge.SFN_Multiply,id:2413,x:31967,y:32935,varname:node_2413,prsc:2|A-1320-G,B-4538-R;n:type:ShaderForge.SFN_Append,id:1447,x:31202,y:32800,varname:node_1447,prsc:2|A-4538-R,B-4538-R;n:type:ShaderForge.SFN_Panner,id:2657,x:31389,y:32800,varname:node_2657,prsc:2,spu:0.2,spv:0.2|UVIN-1447-OUT;n:type:ShaderForge.SFN_Multiply,id:8641,x:32406,y:32695,varname:node_8641,prsc:2|A-8028-OUT,B-9028-OUT;n:type:ShaderForge.SFN_Slider,id:9028,x:32015,y:32777,ptovrint:False,ptlb:alpha,ptin:_alpha,varname:node_9028,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;proporder:7241-3372-7507-9028;pass:END;sub:END;*/

Shader "Shader Forge/candle_active" {
    Properties {
        _Color ("Color", Color) = (0.07843138,0.3921569,0.7843137,1)
        _brightness ("brightness", Float ) = 3
        _Texture ("Texture", 2D) = "white" {}
        _alpha ("alpha", Range(0, 1)) = 1
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
            uniform float _brightness;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float _alpha;
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
                float4 node_3911 = _Time + _TimeEditor;
                float2 node_339 = ((i.uv0+node_3911.g*float2(0.1,-0.05))*float2(1,3));
                float4 node_4538 = tex2D(_Texture,TRANSFORM_TEX(node_339, _Texture));
                float2 node_2657 = (float2(node_4538.r,node_4538.r)+node_3911.g*float2(0.2,0.2));
                float4 node_1320 = tex2D(_Texture,TRANSFORM_TEX(node_2657, _Texture));
                float3 emissive = (((1.0 - i.uv0.g)*_Color.rgb*_brightness*(node_1320.g*node_4538.r))*_alpha);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
