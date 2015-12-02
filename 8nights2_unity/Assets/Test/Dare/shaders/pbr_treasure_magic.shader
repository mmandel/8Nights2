// Shader created with Shader Forge v1.21 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.21;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:1,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:2865,x:32954,y:32718,varname:node_2865,prsc:2|diff-1935-OUT,spec-5650-OUT,gloss-4959-OUT,normal-5964-RGB,emission-9018-OUT,transm-1599-OUT;n:type:ShaderForge.SFN_Color,id:6665,x:32304,y:32583,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.3517241,c3:0,c4:1;n:type:ShaderForge.SFN_Tex2d,id:5964,x:33205,y:32773,ptovrint:True,ptlb:Normal Map,ptin:_BumpMap,varname:_BumpMap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Slider,id:358,x:31745,y:31973,ptovrint:False,ptlb:Metallic,ptin:_Metallic,varname:node_358,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.7154677,max:1;n:type:ShaderForge.SFN_Slider,id:1813,x:31745,y:32180,ptovrint:False,ptlb:Roughness,ptin:_Roughness,varname:_Metallic_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Tex2d,id:7634,x:30846,y:32335,varname:_noise_copy,prsc:2,tex:4e488caea5ebb1c4c942fb5df9e6260c,ntxv:0,isnm:False|UVIN-3938-OUT,TEX-2003-TEX;n:type:ShaderForge.SFN_TexCoord,id:8735,x:30118,y:32272,varname:node_8735,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:3938,x:30491,y:32333,varname:node_3938,prsc:2|A-4365-UVOUT,B-5164-OUT;n:type:ShaderForge.SFN_Vector2,id:5164,x:30316,y:32418,varname:node_5164,prsc:2,v1:2,v2:1;n:type:ShaderForge.SFN_Panner,id:4365,x:30316,y:32272,varname:node_4365,prsc:2,spu:0.08,spv:-0.16|UVIN-8735-UVOUT;n:type:ShaderForge.SFN_Fresnel,id:2947,x:31162,y:32689,varname:node_2947,prsc:2;n:type:ShaderForge.SFN_OneMinus,id:809,x:31350,y:32689,varname:node_809,prsc:2|IN-2947-OUT;n:type:ShaderForge.SFN_Blend,id:5650,x:32255,y:32042,varname:node_5650,prsc:2,blmd:10,clmp:True|SRC-358-OUT,DST-4211-OUT;n:type:ShaderForge.SFN_Blend,id:4959,x:32255,y:32203,varname:node_4959,prsc:2,blmd:10,clmp:True|SRC-1813-OUT,DST-4614-OUT;n:type:ShaderForge.SFN_TexCoord,id:7092,x:31389,y:32450,varname:node_7092,prsc:2,uv:0;n:type:ShaderForge.SFN_Tex2dAsset,id:2003,x:29797,y:33258,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:node_2003,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:4e488caea5ebb1c4c942fb5df9e6260c,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:3372,x:31720,y:33158,varname:node_3372,prsc:2,tex:4e488caea5ebb1c4c942fb5df9e6260c,ntxv:0,isnm:False|UVIN-4303-UVOUT,TEX-2003-TEX;n:type:ShaderForge.SFN_Panner,id:4303,x:31533,y:33000,varname:node_4303,prsc:2,spu:0.08,spv:0.16|UVIN-297-OUT;n:type:ShaderForge.SFN_Multiply,id:7636,x:31784,y:32612,varname:node_7636,prsc:2|A-1041-OUT,B-809-OUT,C-809-OUT;n:type:ShaderForge.SFN_OneMinus,id:1041,x:31552,y:32490,varname:node_1041,prsc:2|IN-7092-V;n:type:ShaderForge.SFN_Tex2d,id:9797,x:31120,y:33000,ptovrint:False,ptlb:facets,ptin:_facets,varname:node_9797,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:934e890c2c24c6a4197d9e84524dc97c,ntxv:0,isnm:False|UVIN-7483-UVOUT;n:type:ShaderForge.SFN_UVTile,id:5159,x:30490,y:32794,varname:node_5159,prsc:2|WDT-9844-OUT,HGT-7912-OUT,TILE-6972-OUT;n:type:ShaderForge.SFN_Vector1,id:9844,x:30293,y:32773,varname:node_9844,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:7912,x:30295,y:32833,varname:node_7912,prsc:2,v1:2;n:type:ShaderForge.SFN_Vector1,id:6972,x:30295,y:32892,varname:node_6972,prsc:2,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:2410,x:32304,y:32848,ptovrint:False,ptlb:overbloom,ptin:_overbloom,varname:node_2410,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3;n:type:ShaderForge.SFN_ValueProperty,id:1599,x:32710,y:32908,ptovrint:False,ptlb:lightTransmission,ptin:_lightTransmission,varname:node_1599,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_RemapRange,id:4211,x:32097,y:32061,varname:node_4211,prsc:2,frmn:0,frmx:1,tomn:0.45,tomx:0.65|IN-7634-G;n:type:ShaderForge.SFN_RemapRange,id:4614,x:32097,y:32234,varname:node_4614,prsc:2,frmn:0,frmx:1,tomn:0.45,tomx:0.65|IN-7634-R;n:type:ShaderForge.SFN_Multiply,id:1935,x:32519,y:32704,varname:node_1935,prsc:2|A-6665-RGB,B-3372-G;n:type:ShaderForge.SFN_ComponentMask,id:297,x:31340,y:33000,varname:node_297,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-9797-RGB;n:type:ShaderForge.SFN_Panner,id:3458,x:30686,y:32794,varname:node_3458,prsc:2,spu:-0.003,spv:-0.001|UVIN-5159-UVOUT;n:type:ShaderForge.SFN_Rotator,id:7483,x:30912,y:32880,varname:node_7483,prsc:2|UVIN-3458-UVOUT,SPD-3997-OUT;n:type:ShaderForge.SFN_Vector1,id:3997,x:30686,y:32951,varname:node_3997,prsc:2,v1:0.03;n:type:ShaderForge.SFN_Tex2d,id:9776,x:31684,y:32804,ptovrint:False,ptlb:flicker gradient,ptin:_flickergradient,varname:node_9776,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:33808ab004e674f428e41cb59df35a2b,ntxv:0,isnm:False|UVIN-2436-UVOUT;n:type:ShaderForge.SFN_Panner,id:2436,x:31471,y:32826,varname:node_2436,prsc:2,spu:0.6,spv:0.6|UVIN-6696-OUT;n:type:ShaderForge.SFN_Multiply,id:2523,x:32121,y:32703,varname:node_2523,prsc:2|A-7636-OUT,B-1328-OUT;n:type:ShaderForge.SFN_Vector2,id:6696,x:31300,y:32826,varname:node_6696,prsc:2,v1:0,v2:0;n:type:ShaderForge.SFN_RemapRange,id:1328,x:31852,y:32804,varname:node_1328,prsc:2,frmn:0,frmx:1,tomn:0,tomx:0.2|IN-9776-R;n:type:ShaderForge.SFN_Multiply,id:9018,x:32519,y:32832,varname:node_9018,prsc:2|A-2523-OUT,B-2410-OUT,C-809-OUT,D-809-OUT;proporder:5964-6665-358-1813-2003-9797-2410-1599-9776;pass:END;sub:END;*/

Shader "Shader Forge/pbr_treasure_magic" {
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
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
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
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
                float3 normalLocal = _BumpMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float4 node_1555 = _Time + _TimeEditor;
                float2 node_3938 = ((i.uv0+node_1555.g*float2(0.08,-0.16))*float2(2,1));
                float4 _noise_copy = tex2D(_Noise,TRANSFORM_TEX(node_3938, _Noise));
                float gloss = 1.0 - saturate(( (_noise_copy.r*0.2+0.45) > 0.5 ? (1.0-(1.0-2.0*((_noise_copy.r*0.2+0.45)-0.5))*(1.0-_Roughness)) : (2.0*(_noise_copy.r*0.2+0.45)*_Roughness) )); // Convert roughness to gloss
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
                float node_7483_ang = node_1555.g;
                float node_7483_spd = 0.03;
                float node_7483_cos = cos(node_7483_spd*node_7483_ang);
                float node_7483_sin = sin(node_7483_spd*node_7483_ang);
                float2 node_7483_piv = float2(0.5,0.5);
                float node_9844 = 1.0;
                float node_6972 = 1.0;
                float2 node_5159_tc_rcp = float2(1.0,1.0)/float2( node_9844, 2.0 );
                float node_5159_ty = floor(node_6972 * node_5159_tc_rcp.x);
                float node_5159_tx = node_6972 - node_9844 * node_5159_ty;
                float2 node_5159 = (i.uv0 + float2(node_5159_tx, node_5159_ty)) * node_5159_tc_rcp;
                float2 node_7483 = (mul((node_5159+node_1555.g*float2(-0.003,-0.001))-node_7483_piv,float2x2( node_7483_cos, -node_7483_sin, node_7483_sin, node_7483_cos))+node_7483_piv);
                float4 _facets_var = tex2D(_facets,TRANSFORM_TEX(node_7483, _facets));
                float2 node_4303 = (_facets_var.rgb.rg+node_1555.g*float2(0.08,0.16));
                float4 node_3372 = tex2D(_Noise,TRANSFORM_TEX(node_4303, _Noise));
                float3 diffuseColor = (_Color.rgb*node_3372.g); // Need this for specular when using metallic
                float specularMonochrome;
                float3 specularColor;
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, saturate(( (_noise_copy.g*0.2+0.45) > 0.5 ? (1.0-(1.0-2.0*((_noise_copy.g*0.2+0.45)-0.5))*(1.0-_Metallic)) : (2.0*(_noise_copy.g*0.2+0.45)*_Metallic) )), specularColor, specularMonochrome );
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
                float node_809 = (1.0 - (1.0-max(0,dot(normalDirection, viewDirection))));
                float2 node_2436 = (float2(0,0)+node_1555.g*float2(0.6,0.6));
                float4 _flickergradient_var = tex2D(_flickergradient,TRANSFORM_TEX(node_2436, _flickergradient));
                float node_9018 = ((((1.0 - i.uv0.g)*node_809*node_809)*(_flickergradient_var.r*0.2+0.0))*_overbloom*node_809*node_809);
                float3 emissive = float3(node_9018,node_9018,node_9018);
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
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
                float3 normalLocal = _BumpMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float4 node_8180 = _Time + _TimeEditor;
                float2 node_3938 = ((i.uv0+node_8180.g*float2(0.08,-0.16))*float2(2,1));
                float4 _noise_copy = tex2D(_Noise,TRANSFORM_TEX(node_3938, _Noise));
                float gloss = 1.0 - saturate(( (_noise_copy.r*0.2+0.45) > 0.5 ? (1.0-(1.0-2.0*((_noise_copy.r*0.2+0.45)-0.5))*(1.0-_Roughness)) : (2.0*(_noise_copy.r*0.2+0.45)*_Roughness) )); // Convert roughness to gloss
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float node_7483_ang = node_8180.g;
                float node_7483_spd = 0.03;
                float node_7483_cos = cos(node_7483_spd*node_7483_ang);
                float node_7483_sin = sin(node_7483_spd*node_7483_ang);
                float2 node_7483_piv = float2(0.5,0.5);
                float node_9844 = 1.0;
                float node_6972 = 1.0;
                float2 node_5159_tc_rcp = float2(1.0,1.0)/float2( node_9844, 2.0 );
                float node_5159_ty = floor(node_6972 * node_5159_tc_rcp.x);
                float node_5159_tx = node_6972 - node_9844 * node_5159_ty;
                float2 node_5159 = (i.uv0 + float2(node_5159_tx, node_5159_ty)) * node_5159_tc_rcp;
                float2 node_7483 = (mul((node_5159+node_8180.g*float2(-0.003,-0.001))-node_7483_piv,float2x2( node_7483_cos, -node_7483_sin, node_7483_sin, node_7483_cos))+node_7483_piv);
                float4 _facets_var = tex2D(_facets,TRANSFORM_TEX(node_7483, _facets));
                float2 node_4303 = (_facets_var.rgb.rg+node_8180.g*float2(0.08,0.16));
                float4 node_3372 = tex2D(_Noise,TRANSFORM_TEX(node_4303, _Noise));
                float3 diffuseColor = (_Color.rgb*node_3372.g); // Need this for specular when using metallic
                float specularMonochrome;
                float3 specularColor;
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, saturate(( (_noise_copy.g*0.2+0.45) > 0.5 ? (1.0-(1.0-2.0*((_noise_copy.g*0.2+0.45)-0.5))*(1.0-_Metallic)) : (2.0*(_noise_copy.g*0.2+0.45)*_Metallic) )), specularColor, specularMonochrome );
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
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float node_809 = (1.0 - (1.0-max(0,dot(normalDirection, viewDirection))));
                float4 node_2725 = _Time + _TimeEditor;
                float2 node_2436 = (float2(0,0)+node_2725.g*float2(0.6,0.6));
                float4 _flickergradient_var = tex2D(_flickergradient,TRANSFORM_TEX(node_2436, _flickergradient));
                float node_9018 = ((((1.0 - i.uv0.g)*node_809*node_809)*(_flickergradient_var.r*0.2+0.0))*_overbloom*node_809*node_809);
                o.Emission = float3(node_9018,node_9018,node_9018);
                
                float node_7483_ang = node_2725.g;
                float node_7483_spd = 0.03;
                float node_7483_cos = cos(node_7483_spd*node_7483_ang);
                float node_7483_sin = sin(node_7483_spd*node_7483_ang);
                float2 node_7483_piv = float2(0.5,0.5);
                float node_9844 = 1.0;
                float node_6972 = 1.0;
                float2 node_5159_tc_rcp = float2(1.0,1.0)/float2( node_9844, 2.0 );
                float node_5159_ty = floor(node_6972 * node_5159_tc_rcp.x);
                float node_5159_tx = node_6972 - node_9844 * node_5159_ty;
                float2 node_5159 = (i.uv0 + float2(node_5159_tx, node_5159_ty)) * node_5159_tc_rcp;
                float2 node_7483 = (mul((node_5159+node_2725.g*float2(-0.003,-0.001))-node_7483_piv,float2x2( node_7483_cos, -node_7483_sin, node_7483_sin, node_7483_cos))+node_7483_piv);
                float4 _facets_var = tex2D(_facets,TRANSFORM_TEX(node_7483, _facets));
                float2 node_4303 = (_facets_var.rgb.rg+node_2725.g*float2(0.08,0.16));
                float4 node_3372 = tex2D(_Noise,TRANSFORM_TEX(node_4303, _Noise));
                float3 diffColor = (_Color.rgb*node_3372.g);
                float specularMonochrome;
                float3 specColor;
                float2 node_3938 = ((i.uv0+node_2725.g*float2(0.08,-0.16))*float2(2,1));
                float4 _noise_copy = tex2D(_Noise,TRANSFORM_TEX(node_3938, _Noise));
                diffColor = DiffuseAndSpecularFromMetallic( diffColor, saturate(( (_noise_copy.g*0.2+0.45) > 0.5 ? (1.0-(1.0-2.0*((_noise_copy.g*0.2+0.45)-0.5))*(1.0-_Metallic)) : (2.0*(_noise_copy.g*0.2+0.45)*_Metallic) )), specColor, specularMonochrome );
                float roughness = saturate(( (_noise_copy.r*0.2+0.45) > 0.5 ? (1.0-(1.0-2.0*((_noise_copy.r*0.2+0.45)-0.5))*(1.0-_Roughness)) : (2.0*(_noise_copy.r*0.2+0.45)*_Roughness) ));
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
