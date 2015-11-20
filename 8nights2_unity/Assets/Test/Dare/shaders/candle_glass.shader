// Shader created with Shader Forge v1.21 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.21;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:1,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:2865,x:33301,y:32720,varname:node_2865,prsc:2|diff-1935-OUT,spec-5650-OUT,gloss-4959-OUT,normal-5964-RGB,emission-278-OUT,transm-1599-OUT,clip-1651-OUT;n:type:ShaderForge.SFN_Color,id:6665,x:31889,y:32511,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.3517241,c3:0,c4:1;n:type:ShaderForge.SFN_Tex2d,id:5964,x:33552,y:32775,ptovrint:True,ptlb:Normal Map,ptin:_BumpMap,varname:_BumpMap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Slider,id:358,x:32114,y:32073,ptovrint:False,ptlb:Metallic,ptin:_Metallic,varname:node_358,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.7154677,max:1;n:type:ShaderForge.SFN_Slider,id:1813,x:32114,y:32238,ptovrint:False,ptlb:Roughness,ptin:_Roughness,varname:_Metallic_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Tex2d,id:7634,x:31863,y:32354,varname:_noise_copy,prsc:2,tex:4e488caea5ebb1c4c942fb5df9e6260c,ntxv:0,isnm:False|UVIN-3938-OUT,TEX-2003-TEX;n:type:ShaderForge.SFN_TexCoord,id:8735,x:30924,y:32296,varname:node_8735,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:3938,x:31297,y:32357,varname:node_3938,prsc:2|A-4365-UVOUT,B-5164-OUT;n:type:ShaderForge.SFN_Vector2,id:5164,x:31122,y:32442,varname:node_5164,prsc:2,v1:2,v2:1;n:type:ShaderForge.SFN_Panner,id:4365,x:31122,y:32296,varname:node_4365,prsc:2,spu:0,spv:0|UVIN-8735-UVOUT;n:type:ShaderForge.SFN_Fresnel,id:2947,x:31601,y:32864,varname:node_2947,prsc:2;n:type:ShaderForge.SFN_OneMinus,id:809,x:31789,y:32864,varname:node_809,prsc:2|IN-2947-OUT;n:type:ShaderForge.SFN_Blend,id:5650,x:32890,y:32302,varname:node_5650,prsc:2,blmd:10,clmp:True|SRC-358-OUT,DST-4211-OUT;n:type:ShaderForge.SFN_Blend,id:4959,x:32890,y:32463,varname:node_4959,prsc:2,blmd:10,clmp:True|SRC-1813-OUT,DST-4614-OUT;n:type:ShaderForge.SFN_TexCoord,id:7092,x:31769,y:33035,varname:node_7092,prsc:2,uv:0;n:type:ShaderForge.SFN_Tex2dAsset,id:2003,x:31122,y:32584,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:node_2003,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:4e488caea5ebb1c4c942fb5df9e6260c,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:3372,x:31496,y:32593,varname:node_3372,prsc:2,tex:4e488caea5ebb1c4c942fb5df9e6260c,ntxv:0,isnm:False|UVIN-718-OUT,TEX-2003-TEX;n:type:ShaderForge.SFN_Panner,id:4303,x:31122,y:32749,varname:node_4303,prsc:2,spu:0.01,spv:0.02|UVIN-297-OUT;n:type:ShaderForge.SFN_Multiply,id:7636,x:32098,y:32857,varname:node_7636,prsc:2|A-809-OUT,B-1041-OUT;n:type:ShaderForge.SFN_OneMinus,id:1041,x:31957,y:32917,varname:node_1041,prsc:2|IN-7092-V;n:type:ShaderForge.SFN_Tex2d,id:9797,x:30759,y:32688,ptovrint:False,ptlb:facets,ptin:_facets,varname:node_9797,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:934e890c2c24c6a4197d9e84524dc97c,ntxv:0,isnm:False|UVIN-6999-OUT;n:type:ShaderForge.SFN_UVTile,id:5159,x:30010,y:32927,varname:node_5159,prsc:2|WDT-9844-OUT,HGT-7912-OUT,TILE-6972-OUT;n:type:ShaderForge.SFN_Vector1,id:9844,x:29813,y:32906,varname:node_9844,prsc:2,v1:3;n:type:ShaderForge.SFN_Vector1,id:7912,x:29815,y:32966,varname:node_7912,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:6972,x:29815,y:33025,varname:node_6972,prsc:2,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:2410,x:32392,y:32977,ptovrint:False,ptlb:overbloom,ptin:_overbloom,varname:node_2410,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3;n:type:ShaderForge.SFN_ValueProperty,id:1599,x:33083,y:32885,ptovrint:False,ptlb:lightTransmission,ptin:_lightTransmission,varname:node_1599,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_RemapRange,id:4211,x:32732,y:32321,varname:node_4211,prsc:2,frmn:0,frmx:1,tomn:0.45,tomx:0.65|IN-7634-R;n:type:ShaderForge.SFN_RemapRange,id:4614,x:32732,y:32494,varname:node_4614,prsc:2,frmn:0,frmx:1,tomn:0.45,tomx:0.65|IN-7634-G;n:type:ShaderForge.SFN_Multiply,id:1935,x:32089,y:32613,varname:node_1935,prsc:2|A-6665-RGB,B-9201-OUT;n:type:ShaderForge.SFN_ComponentMask,id:297,x:30917,y:32688,varname:node_297,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-9797-RGB;n:type:ShaderForge.SFN_Rotator,id:7483,x:30231,y:32927,varname:node_7483,prsc:2|UVIN-5159-UVOUT,SPD-3997-OUT;n:type:ShaderForge.SFN_Vector1,id:3997,x:30231,y:33057,varname:node_3997,prsc:2,v1:0.01;n:type:ShaderForge.SFN_Tex2d,id:9776,x:32474,y:33106,ptovrint:False,ptlb:flicker gradient,ptin:_flickergradient,varname:node_9776,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:33808ab004e674f428e41cb59df35a2b,ntxv:0,isnm:False|UVIN-2436-UVOUT;n:type:ShaderForge.SFN_Panner,id:2436,x:32274,y:33106,varname:node_2436,prsc:2,spu:0.4,spv:0.4|UVIN-6696-OUT;n:type:ShaderForge.SFN_Multiply,id:2523,x:32392,y:32821,varname:node_2523,prsc:2|A-1935-OUT,B-7636-OUT,C-7092-V,D-4900-OUT;n:type:ShaderForge.SFN_Vector2,id:6696,x:32098,y:33106,varname:node_6696,prsc:2,v1:0,v2:0;n:type:ShaderForge.SFN_RemapRange,id:1328,x:32677,y:33124,varname:node_1328,prsc:2,frmn:0,frmx:1,tomn:0,tomx:0.01|IN-9776-R;n:type:ShaderForge.SFN_Add,id:278,x:32920,y:32824,varname:node_278,prsc:2|A-9391-OUT,B-1328-OUT;n:type:ShaderForge.SFN_RemapRange,id:9201,x:31675,y:32631,varname:node_9201,prsc:2,frmn:0,frmx:1,tomn:0.6,tomx:1|IN-3372-G;n:type:ShaderForge.SFN_TexCoord,id:3177,x:30376,y:32485,varname:node_3177,prsc:2,uv:0;n:type:ShaderForge.SFN_ObjectPosition,id:1838,x:30276,y:33178,varname:node_1838,prsc:2;n:type:ShaderForge.SFN_Add,id:4057,x:30467,y:33198,varname:node_4057,prsc:2|A-1838-X,B-1838-Y,C-1838-Z;n:type:ShaderForge.SFN_Add,id:6999,x:30890,y:32955,varname:node_6999,prsc:2|A-7483-UVOUT,B-7640-OUT;n:type:ShaderForge.SFN_Multiply,id:7640,x:30731,y:33206,varname:node_7640,prsc:2|A-4057-OUT,B-5269-OUT;n:type:ShaderForge.SFN_Vector1,id:5269,x:30541,y:33354,varname:node_5269,prsc:2,v1:7.43;n:type:ShaderForge.SFN_Add,id:718,x:31327,y:32809,varname:node_718,prsc:2|A-4303-UVOUT,B-7640-OUT;n:type:ShaderForge.SFN_Multiply,id:9391,x:32647,y:32821,varname:node_9391,prsc:2|A-2523-OUT,B-2410-OUT;n:type:ShaderForge.SFN_Vector1,id:4900,x:32229,y:32937,varname:node_4900,prsc:2,v1:3;n:type:ShaderForge.SFN_Tex2d,id:5813,x:32083,y:33420,varname:node_5813,prsc:2,tex:4e488caea5ebb1c4c942fb5df9e6260c,ntxv:0,isnm:False|UVIN-2073-OUT,TEX-2003-TEX;n:type:ShaderForge.SFN_TexCoord,id:2085,x:32302,y:33596,varname:node_2085,prsc:2,uv:0;n:type:ShaderForge.SFN_Slider,id:2850,x:31883,y:33803,ptovrint:False,ptlb:Appear,ptin:_Appear,varname:node_2850,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Multiply,id:2073,x:31784,y:33417,varname:node_2073,prsc:2|A-776-UVOUT,B-3976-OUT;n:type:ShaderForge.SFN_TexCoord,id:776,x:31542,y:33345,varname:node_776,prsc:2,uv:0;n:type:ShaderForge.SFN_OneMinus,id:2769,x:32281,y:33420,varname:node_2769,prsc:2|IN-5813-R;n:type:ShaderForge.SFN_Vector2,id:3976,x:31555,y:33530,varname:node_3976,prsc:2,v1:4,v2:3;n:type:ShaderForge.SFN_Multiply,id:6467,x:32652,y:33411,varname:node_6467,prsc:2|A-2769-OUT,B-8241-OUT;n:type:ShaderForge.SFN_Add,id:8241,x:32511,y:33637,varname:node_8241,prsc:2|A-2085-V,B-1511-OUT;n:type:ShaderForge.SFN_Step,id:2863,x:32982,y:33645,varname:node_2863,prsc:2|A-8241-OUT,B-4308-OUT;n:type:ShaderForge.SFN_Vector1,id:4308,x:32747,y:33747,varname:node_4308,prsc:2,v1:0.5;n:type:ShaderForge.SFN_OneMinus,id:7193,x:32838,y:33392,varname:node_7193,prsc:2|IN-6467-OUT;n:type:ShaderForge.SFN_Multiply,id:1651,x:33234,y:33392,varname:node_1651,prsc:2|A-7193-OUT,B-2863-OUT,C-7193-OUT;n:type:ShaderForge.SFN_RemapRange,id:1511,x:32285,y:33796,varname:node_1511,prsc:2,frmn:0,frmx:1,tomn:0.5,tomx:-0.7|IN-2850-OUT;proporder:5964-6665-358-1813-2003-9797-2410-1599-9776-2850;pass:END;sub:END;*/

Shader "Shader Forge/candle_glass" {
    Properties {
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _Color ("Color", Color) = (1,0.3517241,0,1)
        _Metallic ("Metallic", Range(0, 1)) = 0.7154677
        _Roughness ("Roughness", Range(0, 1)) = 1
        _Noise ("Noise", 2D) = "white" {}
        _facets ("facets", 2D) = "white" {}
        _overbloom ("overbloom", Float ) = 3
        _lightTransmission ("lightTransmission", Float ) = 1
        _flickergradient ("flicker gradient", 2D) = "white" {}
        _Appear ("Appear", Range(0, 1)) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
            uniform float _Metallic;
            uniform float _Roughness;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform sampler2D _facets; uniform float4 _facets_ST;
            uniform float _overbloom;
            uniform float _lightTransmission;
            uniform sampler2D _flickergradient; uniform float4 _flickergradient_ST;
            uniform float _Appear;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD10;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #elif UNITY_SHOULD_SAMPLE_SH
                #endif
                #ifdef DYNAMICLIGHTMAP_ON
                    o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 objPos = mul ( _Object2World, float4(0,0,0,1) );
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 objPos = mul ( _Object2World, float4(0,0,0,1) );
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
                float3 normalLocal = _BumpMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float2 node_2073 = (i.uv0*float2(4,3));
                float4 node_5813 = tex2D(_Noise,TRANSFORM_TEX(node_2073, _Noise));
                float node_8241 = (i.uv0.g+(_Appear*-1.2+0.5));
                float node_6467 = ((1.0 - node_5813.r)*node_8241);
                float node_7193 = (1.0 - node_6467);
                clip((node_7193*step(node_8241,0.5)*node_7193) - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float4 node_7295 = _Time + _TimeEditor;
                float2 node_3938 = ((i.uv0+node_7295.g*float2(0,0))*float2(2,1));
                float4 _noise_copy = tex2D(_Noise,TRANSFORM_TEX(node_3938, _Noise));
                float gloss = 1.0 - saturate(( (_noise_copy.g*0.2+0.45) > 0.5 ? (1.0-(1.0-2.0*((_noise_copy.g*0.2+0.45)-0.5))*(1.0-_Roughness)) : (2.0*(_noise_copy.g*0.2+0.45)*_Roughness) )); // Convert roughness to gloss
                float specPow = exp2( gloss * 10.0+1.0);
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    d.ambient = 0;
                    d.lightmapUV = i.ambientOrLightmapUV;
                #else
                    d.ambient = i.ambientOrLightmapUV;
                #endif
                d.boxMax[0] = unity_SpecCube0_BoxMax;
                d.boxMin[0] = unity_SpecCube0_BoxMin;
                d.probePosition[0] = unity_SpecCube0_ProbePosition;
                d.probeHDR[0] = unity_SpecCube0_HDR;
                d.boxMax[1] = unity_SpecCube1_BoxMax;
                d.boxMin[1] = unity_SpecCube1_BoxMin;
                d.probePosition[1] = unity_SpecCube1_ProbePosition;
                d.probeHDR[1] = unity_SpecCube1_HDR;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float node_7483_ang = node_7295.g;
                float node_7483_spd = 0.01;
                float node_7483_cos = cos(node_7483_spd*node_7483_ang);
                float node_7483_sin = sin(node_7483_spd*node_7483_ang);
                float2 node_7483_piv = float2(0.5,0.5);
                float node_9844 = 3.0;
                float node_6972 = 1.0;
                float2 node_5159_tc_rcp = float2(1.0,1.0)/float2( node_9844, 1.0 );
                float node_5159_ty = floor(node_6972 * node_5159_tc_rcp.x);
                float node_5159_tx = node_6972 - node_9844 * node_5159_ty;
                float2 node_5159 = (i.uv0 + float2(node_5159_tx, node_5159_ty)) * node_5159_tc_rcp;
                float2 node_7483 = (mul(node_5159-node_7483_piv,float2x2( node_7483_cos, -node_7483_sin, node_7483_sin, node_7483_cos))+node_7483_piv);
                float node_7640 = ((objPos.r+objPos.g+objPos.b)*7.43);
                float2 node_6999 = (node_7483+node_7640);
                float4 _facets_var = tex2D(_facets,TRANSFORM_TEX(node_6999, _facets));
                float2 node_718 = ((_facets_var.rgb.rg+node_7295.g*float2(0.01,0.02))+node_7640);
                float4 node_3372 = tex2D(_Noise,TRANSFORM_TEX(node_718, _Noise));
                float3 node_1935 = (_Color.rgb*(node_3372.g*0.4+0.6));
                float3 diffuseColor = node_1935; // Need this for specular when using metallic
                float specularMonochrome;
                float3 specularColor;
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, saturate(( (_noise_copy.r*0.2+0.45) > 0.5 ? (1.0-(1.0-2.0*((_noise_copy.r*0.2+0.45)-0.5))*(1.0-_Metallic)) : (2.0*(_noise_copy.r*0.2+0.45)*_Metallic) )), specularColor, specularMonochrome );
                specularMonochrome = 1-specularMonochrome;
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float NdotH = max(0.0,dot( normalDirection, halfDirection ));
                float VdotH = max(0.0,dot( viewDirection, halfDirection ));
                float visTerm = SmithBeckmannVisibilityTerm( NdotL, NdotV, 1.0-gloss );
                float normTerm = max(0.0, NDFBlinnPhongNormalizedTerm(NdotH, RoughnessToSpecPower(1.0-gloss)));
                float specularPBL = max(0, (NdotL*visTerm*normTerm) * unity_LightGammaCorrectionConsts_PIDiv4 );
                float3 directSpecular = 1 * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularPBL*lightColor*FresnelTerm(specularColor, LdotH);
                half grazingTerm = saturate( gloss + specularMonochrome );
                float3 indirectSpecular = (gi.indirect.specular);
                indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
                float3 specular = (directSpecular + indirectSpecular);
/////// Diffuse:
                NdotL = dot( normalDirection, lightDirection );
                float3 forwardLight = max(0.0, NdotL );
                float3 backLight = max(0.0, -NdotL ) * float3(_lightTransmission,_lightTransmission,_lightTransmission);
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float3 NdotLWrap = max(0,NdotL);
                NdotLWrap = max(float3(0,0,0), NdotLWrap);
                float3 directDiffuse = ((forwardLight+backLight) + ((1 +(fd90 - 1)*pow((1.00001-NdotLWrap), 5)) * (1 + (fd90 - 1)*pow((1.00001-NdotV), 5)) * NdotL)) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float2 node_2436 = (float2(0,0)+node_7295.g*float2(0.4,0.4));
                float4 _flickergradient_var = tex2D(_flickergradient,TRANSFORM_TEX(node_2436, _flickergradient));
                float3 emissive = (((node_1935*((1.0 - (1.0-max(0,dot(normalDirection, viewDirection))))*(1.0 - i.uv0.g))*i.uv0.g*3.0)*_overbloom)+(_flickergradient_var.r*0.01+0.0));
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
            uniform float _Metallic;
            uniform float _Roughness;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform sampler2D _facets; uniform float4 _facets_ST;
            uniform float _overbloom;
            uniform float _lightTransmission;
            uniform sampler2D _flickergradient; uniform float4 _flickergradient_ST;
            uniform float _Appear;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 objPos = mul ( _Object2World, float4(0,0,0,1) );
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 objPos = mul ( _Object2World, float4(0,0,0,1) );
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
                float3 normalLocal = _BumpMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float2 node_2073 = (i.uv0*float2(4,3));
                float4 node_5813 = tex2D(_Noise,TRANSFORM_TEX(node_2073, _Noise));
                float node_8241 = (i.uv0.g+(_Appear*-1.2+0.5));
                float node_6467 = ((1.0 - node_5813.r)*node_8241);
                float node_7193 = (1.0 - node_6467);
                clip((node_7193*step(node_8241,0.5)*node_7193) - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float4 node_6966 = _Time + _TimeEditor;
                float2 node_3938 = ((i.uv0+node_6966.g*float2(0,0))*float2(2,1));
                float4 _noise_copy = tex2D(_Noise,TRANSFORM_TEX(node_3938, _Noise));
                float gloss = 1.0 - saturate(( (_noise_copy.g*0.2+0.45) > 0.5 ? (1.0-(1.0-2.0*((_noise_copy.g*0.2+0.45)-0.5))*(1.0-_Roughness)) : (2.0*(_noise_copy.g*0.2+0.45)*_Roughness) )); // Convert roughness to gloss
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float node_7483_ang = node_6966.g;
                float node_7483_spd = 0.01;
                float node_7483_cos = cos(node_7483_spd*node_7483_ang);
                float node_7483_sin = sin(node_7483_spd*node_7483_ang);
                float2 node_7483_piv = float2(0.5,0.5);
                float node_9844 = 3.0;
                float node_6972 = 1.0;
                float2 node_5159_tc_rcp = float2(1.0,1.0)/float2( node_9844, 1.0 );
                float node_5159_ty = floor(node_6972 * node_5159_tc_rcp.x);
                float node_5159_tx = node_6972 - node_9844 * node_5159_ty;
                float2 node_5159 = (i.uv0 + float2(node_5159_tx, node_5159_ty)) * node_5159_tc_rcp;
                float2 node_7483 = (mul(node_5159-node_7483_piv,float2x2( node_7483_cos, -node_7483_sin, node_7483_sin, node_7483_cos))+node_7483_piv);
                float node_7640 = ((objPos.r+objPos.g+objPos.b)*7.43);
                float2 node_6999 = (node_7483+node_7640);
                float4 _facets_var = tex2D(_facets,TRANSFORM_TEX(node_6999, _facets));
                float2 node_718 = ((_facets_var.rgb.rg+node_6966.g*float2(0.01,0.02))+node_7640);
                float4 node_3372 = tex2D(_Noise,TRANSFORM_TEX(node_718, _Noise));
                float3 node_1935 = (_Color.rgb*(node_3372.g*0.4+0.6));
                float3 diffuseColor = node_1935; // Need this for specular when using metallic
                float specularMonochrome;
                float3 specularColor;
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, saturate(( (_noise_copy.r*0.2+0.45) > 0.5 ? (1.0-(1.0-2.0*((_noise_copy.r*0.2+0.45)-0.5))*(1.0-_Metallic)) : (2.0*(_noise_copy.r*0.2+0.45)*_Metallic) )), specularColor, specularMonochrome );
                specularMonochrome = 1-specularMonochrome;
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float NdotH = max(0.0,dot( normalDirection, halfDirection ));
                float VdotH = max(0.0,dot( viewDirection, halfDirection ));
                float visTerm = SmithBeckmannVisibilityTerm( NdotL, NdotV, 1.0-gloss );
                float normTerm = max(0.0, NDFBlinnPhongNormalizedTerm(NdotH, RoughnessToSpecPower(1.0-gloss)));
                float specularPBL = max(0, (NdotL*visTerm*normTerm) * unity_LightGammaCorrectionConsts_PIDiv4 );
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularPBL*lightColor*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = dot( normalDirection, lightDirection );
                float3 forwardLight = max(0.0, NdotL );
                float3 backLight = max(0.0, -NdotL ) * float3(_lightTransmission,_lightTransmission,_lightTransmission);
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float3 NdotLWrap = max(0,NdotL);
                NdotLWrap = max(float3(0,0,0), NdotLWrap);
                float3 directDiffuse = ((forwardLight+backLight) + ((1 +(fd90 - 1)*pow((1.00001-NdotLWrap), 5)) * (1 + (fd90 - 1)*pow((1.00001-NdotV), 5)) * NdotL)) * attenColor;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _Appear;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float2 uv1 : TEXCOORD2;
                float2 uv2 : TEXCOORD3;
                float4 posWorld : TEXCOORD4;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float2 node_2073 = (i.uv0*float2(4,3));
                float4 node_5813 = tex2D(_Noise,TRANSFORM_TEX(node_2073, _Noise));
                float node_8241 = (i.uv0.g+(_Appear*-1.2+0.5));
                float node_6467 = ((1.0 - node_5813.r)*node_8241);
                float node_7193 = (1.0 - node_6467);
                clip((node_7193*step(node_8241,0.5)*node_7193) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
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
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform float _Metallic;
            uniform float _Roughness;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform sampler2D _facets; uniform float4 _facets_ST;
            uniform float _overbloom;
            uniform sampler2D _flickergradient; uniform float4 _flickergradient_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 objPos = mul ( _Object2World, float4(0,0,0,1) );
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                float4 objPos = mul ( _Object2World, float4(0,0,0,1) );
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float4 node_1203 = _Time + _TimeEditor;
                float node_7483_ang = node_1203.g;
                float node_7483_spd = 0.01;
                float node_7483_cos = cos(node_7483_spd*node_7483_ang);
                float node_7483_sin = sin(node_7483_spd*node_7483_ang);
                float2 node_7483_piv = float2(0.5,0.5);
                float node_9844 = 3.0;
                float node_6972 = 1.0;
                float2 node_5159_tc_rcp = float2(1.0,1.0)/float2( node_9844, 1.0 );
                float node_5159_ty = floor(node_6972 * node_5159_tc_rcp.x);
                float node_5159_tx = node_6972 - node_9844 * node_5159_ty;
                float2 node_5159 = (i.uv0 + float2(node_5159_tx, node_5159_ty)) * node_5159_tc_rcp;
                float2 node_7483 = (mul(node_5159-node_7483_piv,float2x2( node_7483_cos, -node_7483_sin, node_7483_sin, node_7483_cos))+node_7483_piv);
                float node_7640 = ((objPos.r+objPos.g+objPos.b)*7.43);
                float2 node_6999 = (node_7483+node_7640);
                float4 _facets_var = tex2D(_facets,TRANSFORM_TEX(node_6999, _facets));
                float2 node_718 = ((_facets_var.rgb.rg+node_1203.g*float2(0.01,0.02))+node_7640);
                float4 node_3372 = tex2D(_Noise,TRANSFORM_TEX(node_718, _Noise));
                float3 node_1935 = (_Color.rgb*(node_3372.g*0.4+0.6));
                float2 node_2436 = (float2(0,0)+node_1203.g*float2(0.4,0.4));
                float4 _flickergradient_var = tex2D(_flickergradient,TRANSFORM_TEX(node_2436, _flickergradient));
                o.Emission = (((node_1935*((1.0 - (1.0-max(0,dot(normalDirection, viewDirection))))*(1.0 - i.uv0.g))*i.uv0.g*3.0)*_overbloom)+(_flickergradient_var.r*0.01+0.0));
                
                float3 diffColor = node_1935;
                float specularMonochrome;
                float3 specColor;
                float2 node_3938 = ((i.uv0+node_1203.g*float2(0,0))*float2(2,1));
                float4 _noise_copy = tex2D(_Noise,TRANSFORM_TEX(node_3938, _Noise));
                diffColor = DiffuseAndSpecularFromMetallic( diffColor, saturate(( (_noise_copy.r*0.2+0.45) > 0.5 ? (1.0-(1.0-2.0*((_noise_copy.r*0.2+0.45)-0.5))*(1.0-_Metallic)) : (2.0*(_noise_copy.r*0.2+0.45)*_Metallic) )), specColor, specularMonochrome );
                float roughness = saturate(( (_noise_copy.g*0.2+0.45) > 0.5 ? (1.0-(1.0-2.0*((_noise_copy.g*0.2+0.45)-0.5))*(1.0-_Roughness)) : (2.0*(_noise_copy.g*0.2+0.45)*_Roughness) ));
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
