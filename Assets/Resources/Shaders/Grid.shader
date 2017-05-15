Shader "Grid" {
	Properties{
		[Toggle] _Enabled("IsEnabled", float) = 0
		_Color("Glow Color", Color) = (1,1,1,0.25)
		_GlowStrength("Glow Strength", float) = 1.0
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}

	_PlayerPosition("Player Position", vector) = (0,0,0,0)
		_FadeStartDistance("Fade start distance", float) = 10.0
		_FadeEndDistance("Fade end distance", float) = 12.0

	}

		SubShader{
		Tags{ "Queue" = "Transparent" "RenderType" = "MKGlow" }
		LOD 200

		CGPROGRAM
#pragma surface surf BlinnPhong  alpha

		sampler2D _MainTex;
	fixed4 _Color;

	uniform float4 _PlayerPosition;
	uniform float _FadeStartDistance;
	uniform float _FadeEndDistance;
	uniform float _GlowStrength;
	uniform float _Enabled;

	struct Input {
		float2 uv_MainTex;
		float2 uv_MKGlowTex;
		float3 worldPos;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		
		if(_Enabled < 0.5)
		{
			o.Alpha = 0;
			return;
		}

		float4 color = tex2D(_MainTex, IN.uv_MainTex);

		// Calculate distance to player position
		float dist = distance(IN.worldPos, _PlayerPosition);

		if (dist >= _FadeStartDistance && dist <= _FadeEndDistance) {
			color.a *= 1.0 - (dist - _FadeStartDistance) / (_FadeEndDistance - _FadeStartDistance);
		}
		else if (dist > _FadeEndDistance) {
			color.a = 0.0; // Invisible
			color.rgb = 0.0;
		}

		o.Albedo = color.rgb;
		o.Alpha = color.a;
	}
	ENDCG
	}

		Fallback "Transparent/VertexLit"
}
