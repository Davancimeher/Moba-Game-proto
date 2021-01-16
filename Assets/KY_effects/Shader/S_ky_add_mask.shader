// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.27 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.27;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True;n:type:ShaderForge.SFN_Final,id:4795,x:34933,y:32704,varname:node_4795,prsc:2|emission-1883-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32614,y:32600,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c3921be87f5cb4fc59d61ceac5711076,ntxv:0,isnm:False;n:type:ShaderForge.SFN_VertexColor,id:2053,x:33568,y:32899,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:880,x:33568,y:32833,ptovrint:False,ptlb:emissiveR,ptin:_emissiveR,varname:node_880,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:3898,x:33946,y:32786,varname:node_3898,prsc:2|A-6074-R,B-880-OUT,C-2053-RGB;n:type:ShaderForge.SFN_Multiply,id:1260,x:33569,y:32303,varname:node_1260,prsc:2|A-6074-G,B-2144-RGB,C-2594-OUT;n:type:ShaderForge.SFN_Color,id:2144,x:33250,y:32203,ptovrint:False,ptlb:colorG,ptin:_colorG,varname:node_2144,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:2594,x:33290,y:32427,ptovrint:False,ptlb:emissiveG,ptin:_emissiveG,varname:node_2594,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:7105,x:33682,y:32612,varname:node_7105,prsc:2|A-6074-B,B-678-RGB,C-4-OUT;n:type:ShaderForge.SFN_Color,id:678,x:33412,y:32506,ptovrint:False,ptlb:colorB,ptin:_colorB,varname:node_678,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:4,x:33439,y:32663,ptovrint:False,ptlb:emissiveB,ptin:_emissiveB,varname:node_4,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Add,id:9079,x:33904,y:32472,varname:node_9079,prsc:2|A-1260-OUT,B-7105-OUT;n:type:ShaderForge.SFN_Add,id:8275,x:34177,y:32664,varname:node_8275,prsc:2|A-9079-OUT,B-3898-OUT;n:type:ShaderForge.SFN_Multiply,id:1883,x:34626,y:32906,varname:node_1883,prsc:2|A-8275-OUT,B-2053-A,C-6351-OUT;n:type:ShaderForge.SFN_Power,id:6351,x:34157,y:33161,varname:node_6351,prsc:2|VAL-6074-A,EXP-3641-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3641,x:33896,y:33236,ptovrint:False,ptlb:alphaDensity,ptin:_alphaDensity,varname:node_3641,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;proporder:6074-880-2144-2594-678-4-3641;pass:END;sub:END;*/

Shader "KY/add_mask" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _emissiveR ("emissiveR", Float ) = 1
        _colorG ("colorG", Color) = (1,1,1,1)
        _emissiveG ("emissiveG", Float ) = 1
        _colorB ("colorB", Color) = (1,1,1,1)
        _emissiveB ("emissiveB", Float ) = 1
        _alphaDensity ("alphaDensity", Float ) = 2
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
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _emissiveR;
            uniform float4 _colorG;
            uniform float _emissiveG;
            uniform float4 _colorB;
            uniform float _emissiveB;
            uniform float _alphaDensity;
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
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 emissive = ((((_MainTex_var.g*_colorG.rgb*_emissiveG)+(_MainTex_var.b*_colorB.rgb*_emissiveB))+(_MainTex_var.r*_emissiveR*i.vertexColor.rgb))*i.vertexColor.a*pow(_MainTex_var.a,_alphaDensity));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
