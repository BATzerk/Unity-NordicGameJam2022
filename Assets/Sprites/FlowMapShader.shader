Shader "Custom/FlowMapShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _FlowMap ("FlowMap", 2D) = "white" {}
        _FlowSpeed ("FlowSpeed", Range(0,1)) = 0.1
        _FlowIntensity ("FlowIntensity", Range(0,1)) = 0.1
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _FlowMap;        
        float _FlowSpeed; 
        float _FlowIntensity; 

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
        
        float4 mix(float4 first, float4 second, float delta) 
        {
            return first + second * delta;
        }
        
        float fract (float x)
        {
            return x - floor(x);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 flow = tex2D(_FlowMap, IN.uv_MainTex).xy;
            //flow = (flow - 0.5) * 2.0; // remap -1 to 1
            float timePhase1 = fract(_Time * _FlowSpeed); // 0..1
            float timePhase2 = fract(timePhase1 + 0.5); // 0..1 offset
//             float timePhase1 = _Time * _FlowSpeed; // 0..1 
//             float timePhase2 = timePhase1 + 0.5; // 0..1 offset
            float flowMix = abs(timePhase1 - 0.5) * 2; // oscilating between 0..1
                        
            float2 phase1Uv = IN.uv_MainTex + flow * timePhase1 * _FlowIntensity;            
            float2 phase2Uv = IN.uv_MainTex + flow * timePhase2 * _FlowIntensity;

            float4 phase1Color = tex2D(_MainTex, phase1Uv);
            float4 phase2Color = tex2D(_MainTex, phase2Uv);
            
            
            float4 color = mix(phase1Color, phase2Color, flowMix);
//             float4 color = tex2D(_MainTex, phase1Uv);
        
            o.Albedo = color;
            o.Alpha = color.a;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
