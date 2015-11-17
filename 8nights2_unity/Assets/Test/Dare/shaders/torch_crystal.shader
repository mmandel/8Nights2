// Shader created with Shader Forge v1.21 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.21;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:1,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:4013,x:32904,y:32628,varname:node_4013,prsc:2|emission-4789-OUT;n:type:ShaderForge.SFN_Color,id:1304,x:32283,y:32341,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_1304,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5367647,c2:0.5564988,c3:0.5564988,c4:1;n:type:ShaderForge.SFN_Tex2d,id:6109,x:32090,y:32767,ptovrint:False,ptlb:noise,ptin:_noise,varname:node_6109,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:4e488caea5ebb1c4c942fb5df9e6260c,ntxv:0,isnm:False|UVIN-2043-OUT;n:type:ShaderForge.SFN_TexCoord,id:9355,x:31496,y:32679,varname:node_9355,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:2043,x:31909,y:32767,varname:node_2043,prsc:2|A-4308-UVOUT,B-8933-OUT;n:type:ShaderForge.SFN_Vector2,id:8933,x:31694,y:32825,varname:node_8933,prsc:2,v1:2,v2:1;n:type:ShaderForge.SFN_Panner,id:4308,x:31694,y:32679,varname:node_4308,prsc:2,spu:0,spv:0|UVIN-9355-UVOUT;n:type:ShaderForge.SFN_Fresnel,id:1984,x:32285,y:32707,varname:node_1984,prsc:2|EXP-6109-G;n:type:ShaderForge.SFN_OneMinus,id:4789,x:32448,y:32707,varname:node_4789,prsc:2|IN-1984-OUT;n:type:ShaderForge.SFN_TexCoord,id:8567,x:32299,y:32940,varname:node_8567,prsc:2,uv:0;proporder:1304-6109;pass:END;sub:END;*/

Shader "" {
    Properties {
        _Color ("Color", Color) = (0.5367647,0.5564988,0.5564988,1)
        _noise ("noise", 2D) = "white" {}
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
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _noise; uniform float4 _noise_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                UNITY_FOG_COORDS(3)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float4 node_5881 = _Time + _TimeEditor;
                float2 node_2043 = ((i.uv0+node_5881.g*float2(0,0))*float2(2,1));
                float4 _noise_var = tex2D(_noise,TRANSFORM_TEX(node_2043, _noise));
                float node_4789 = (1.0 - pow(1.0-max(0,dot(normalDirection, viewDirection)),_noise_var.g));
                float3 emissive = float3(node_4789,node_4789,node_4789);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
