Shader "Custom/FlowMapShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _FlowMap ("FlowMap", 2D) = "white" {}
        _FlowSpeed ("FlowSpeed", Range(0,10)) = 10
        _FlowIntensity ("FlowIntensity", Range(0,1)) = 0.8
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        
        _Alpha ("Alpha", Range(0,1)) = 1
        _AlphaNoise ("FlowMap", 2D) = "white" {}
        _AlphaNoiseIntensity ("Alpha Noise Intensity", Range(0,2)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _FlowMap;        
        sampler2D _AlphaNoise;        
        float _FlowSpeed; 
        float _FlowIntensity; 
        float _Alpha; 
        float _AlphaNoiseIntensity; 

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
//         UNITY_INSTANCING_BUFFER_START(Props)
//             // put more per-instance properties here
//         UNITY_INSTANCING_BUFFER_END(Props)
        
        float4 mix(float4 first, float4 second, float delta) 
        {
            return first + second * delta;
        }
        
        float fract (float x)
        {
            return x - floor(x);
        }
        
        float brightness (float3 color) {
             return color.r * 0.3 + color.g * 0.59 + color.b * 0.11;
        }
        
        float min(float a, float b)
        {
            return a < b ? a : b;
        }
        
        float clampZeroToOne(float a)
        {
            if (a < 0) return 0;      
            return a < 1 ? a : 1;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 flow = tex2D(_FlowMap, IN.uv_MainTex).xy;
            float timePhase1 = sin(_Time * _FlowSpeed); // -0.5 * 2; // -1..1            
            float2 phase1Uv = IN.uv_MainTex + flow * timePhase1 * _FlowIntensity;
            float4 color = tex2D(_MainTex, phase1Uv);
            float2 noiseUv = (0, _SinTime);       
            float3 alphaColor = tex2D(_AlphaNoise, noiseUv).rgb;
                        
            o.Albedo = color.rgb;
            o.Alpha = color.a * clampZeroToOne(_Alpha + (brightness(alphaColor)-0.5) * 2 * _AlphaNoiseIntensity);
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
        }
        
        ENDCG
    }
    FallBack "Diffuse"
}
