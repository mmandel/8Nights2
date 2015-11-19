// Shader created with Shader Forge v1.21 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.21;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:1,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:False,igpj:True,qofs:1000,qpre:4,rntp:4,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:2865,x:32719,y:32712,varname:node_2865,prsc:2|emission-2523-OUT;n:type:ShaderForge.SFN_Color,id:6665,x:31978,y:32816,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.2196079,c2:0.2196079,c3:0.2196079,c4:1;n:type:ShaderForge.SFN_Tex2d,id:7634,x:30626,y:32622,varname:_noise_copy,prsc:2,tex:4e488caea5ebb1c4c942fb5df9e6260c,ntxv:0,isnm:False|UVIN-3938-OUT,TEX-2003-TEX;n:type:ShaderForge.SFN_TexCoord,id:8735,x:30031,y:32561,varname:node_8735,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:3938,x:30404,y:32622,varname:node_3938,prsc:2|A-4365-UVOUT,B-5164-OUT;n:type:ShaderForge.SFN_Vector2,id:5164,x:30229,y:32707,varname:node_5164,prsc:2,v1:2,v2:1;n:type:ShaderForge.SFN_Panner,id:4365,x:30229,y:32561,varname:node_4365,prsc:2,spu:0,spv:0|UVIN-8735-UVOUT;n:type:ShaderForge.SFN_Tex2dAsset,id:2003,x:30302,y:33081,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:node_2003,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:4e488caea5ebb1c4c942fb5df9e6260c,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:3372,x:31978,y:33029,varname:node_3372,prsc:2,tex:4e488caea5ebb1c4c942fb5df9e6260c,ntxv:0,isnm:False|UVIN-4303-UVOUT,TEX-2003-TEX;n:type:ShaderForge.SFN_Panner,id:4303,x:31740,y:32863,varname:node_4303,prsc:2,spu:0.01,spv:0.02|UVIN-297-OUT;n:type:ShaderForge.SFN_Multiply,id:7636,x:32208,y:32921,varname:node_7636,prsc:2|A-6665-RGB,B-3372-G;n:type:ShaderForge.SFN_Tex2d,id:9797,x:31359,y:32865,ptovrint:False,ptlb:facets,ptin:_facets,varname:node_9797,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:934e890c2c24c6a4197d9e84524dc97c,ntxv:0,isnm:False|UVIN-7483-UVOUT;n:type:ShaderForge.SFN_UVTile,id:5159,x:30830,y:32876,varname:node_5159,prsc:2|WDT-9844-OUT,HGT-7912-OUT,TILE-6972-OUT;n:type:ShaderForge.SFN_Vector1,id:9844,x:30633,y:32855,varname:node_9844,prsc:2,v1:3;n:type:ShaderForge.SFN_Vector1,id:7912,x:30635,y:32915,varname:node_7912,prsc:2,v1:5;n:type:ShaderForge.SFN_Vector1,id:6972,x:30635,y:32974,varname:node_6972,prsc:2,v1:4;n:type:ShaderForge.SFN_ValueProperty,id:2410,x:32208,y:33063,ptovrint:False,ptlb:overbloom,ptin:_overbloom,varname:node_2410,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3;n:type:ShaderForge.SFN_ComponentMask,id:297,x:31547,y:32863,varname:node_297,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-9797-RGB;n:type:ShaderForge.SFN_Panner,id:3458,x:31119,y:32865,varname:node_3458,prsc:2,spu:0,spv:0|UVIN-5159-UVOUT;n:type:ShaderForge.SFN_Rotator,id:7483,x:31119,y:32743,varname:node_7483,prsc:2|UVIN-5159-UVOUT,SPD-3997-OUT;n:type:ShaderForge.SFN_Vector1,id:3997,x:30893,y:32814,varname:node_3997,prsc:2,v1:0.01;n:type:ShaderForge.SFN_Multiply,id:2523,x:32406,y:32921,varname:node_2523,prsc:2|A-7636-OUT,B-2410-OUT;proporder:6665-2003-9797-2410;pass:END;sub:END;*/

Shader "Shader Forge/icon_torch" {
    Properties {
        _Color ("Color", Color) = (0.2196079,0.2196079,0.2196079,1)
        _Noise ("Noise", 2D) = "white" {}
        _facets ("facets", 2D) = "white" {}
        _overbloom ("overbloom", Float ) = 3
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Overlay+1000"
            "RenderType"="Background"
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
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform sampler2D _facets; uniform float4 _facets_ST;
            uniform float _overbloom;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
////// Lighting:
////// Emissive:
                float4 node_2761 = _Time + _TimeEditor;
                float node_7483_ang = node_2761.g;
                float node_7483_spd = 0.01;
                float node_7483_cos = cos(node_7483_spd*node_7483_ang);
                float node_7483_sin = sin(node_7483_spd*node_7483_ang);
                float2 node_7483_piv = float2(0.5,0.5);
                float node_9844 = 3.0;
                float node_6972 = 4.0;
                float2 node_5159_tc_rcp = float2(1.0,1.0)/float2( node_9844, 5.0 );
                float node_5159_ty = floor(node_6972 * node_5159_tc_rcp.x);
                float node_5159_tx = node_6972 - node_9844 * node_5159_ty;
                float2 node_5159 = (i.uv0 + float2(node_5159_tx, node_5159_ty)) * node_5159_tc_rcp;
                float2 node_7483 = (mul(node_5159-node_7483_piv,float2x2( node_7483_cos, -node_7483_sin, node_7483_sin, node_7483_cos))+node_7483_piv);
                float4 _facets_var = tex2D(_facets,TRANSFORM_TEX(node_7483, _facets));
                float2 node_4303 = (_facets_var.rgb.rg+node_2761.g*float2(0.01,0.02));
                float4 node_3372 = tex2D(_Noise,TRANSFORM_TEX(node_4303, _Noise));
                float3 emissive = ((_Color.rgb*node_3372.g)*_overbloom);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform sampler2D _facets; uniform float4 _facets_ST;
            uniform float _overbloom;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
/////// Vectors:
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float4 node_3948 = _Time + _TimeEditor;
                float node_7483_ang = node_3948.g;
                float node_7483_spd = 0.01;
                float node_7483_cos = cos(node_7483_spd*node_7483_ang);
                float node_7483_sin = sin(node_7483_spd*node_7483_ang);
                float2 node_7483_piv = float2(0.5,0.5);
                float node_9844 = 3.0;
                float node_6972 = 4.0;
                float2 node_5159_tc_rcp = float2(1.0,1.0)/float2( node_9844, 5.0 );
                float node_5159_ty = floor(node_6972 * node_5159_tc_rcp.x);
                float node_5159_tx = node_6972 - node_9844 * node_5159_ty;
                float2 node_5159 = (i.uv0 + float2(node_5159_tx, node_5159_ty)) * node_5159_tc_rcp;
                float2 node_7483 = (mul(node_5159-node_7483_piv,float2x2( node_7483_cos, -node_7483_sin, node_7483_sin, node_7483_cos))+node_7483_piv);
                float4 _facets_var = tex2D(_facets,TRANSFORM_TEX(node_7483, _facets));
                float2 node_4303 = (_facets_var.rgb.rg+node_3948.g*float2(0.01,0.02));
                float4 node_3372 = tex2D(_Noise,TRANSFORM_TEX(node_4303, _Noise));
                o.Emission = ((_Color.rgb*node_3372.g)*_overbloom);
                
                float3 diffColor = float3(0,0,0);
                o.Albedo = diffColor;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
