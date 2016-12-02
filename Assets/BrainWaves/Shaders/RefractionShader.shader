// Upgrade NOTE: replaced 'PositionFog()' with multiply of UNITY_MATRIX_MVP by position
// Upgrade NOTE: replaced 'V2F_POS_FOG' with 'float4 pos : SV_POSITION'
// Upgrade NOTE: replaced 'glstate.matrix.mvp' with 'UNITY_MATRIX_MVP'
// Upgrade NOTE: replaced 'glstate.matrix.texture[0]' with 'UNITY_MATRIX_TEXTURE0'
// Upgrade NOTE: replaced 'samplerRECT' with 'sampler2D'
// Upgrade NOTE: replaced 'texRECTproj' with 'tex2Dproj'

// Uses geometry normals to distort the image behind, and
// an additional texture to tint the color.

Shader "FX/Refraction Distort" {
	Properties{
		_BumpAmt("Distortion", range(0,128)) = 10.0
		_MainTex("Tint Color (RGB)", 2D) = "white" {}
	}

		Category{

		// We must be transparent, so other objects are drawn before this one.
		Tags{ "Queue" = "Transparent" }

		// ------------------------------------------------------------------
		//  ARB fragment program

		SubShader{

		// This pass grabs the screen behind the object into a texture.
		// We can access the result in the next pass as _GrabTexture
		GrabPass{
		Name "BASE"
		Tags{ "LightMode" = "Always" }
	}

		// Main pass: Take the texture grabbed above and use the normals to perturb it
		// on to the screen
		Pass{
		Name "BASE"
		Tags{ "LightMode" = "Always" }

		CGPROGRAM
		// profiles arbfp1
		// vertex vert
		// fragment frag
		// fragmentoption ARB_precision_hint_fastest 
		// fragmentoption ARB_fog_exp2

#include "UnityCG.cginc"

		sampler2D _MainTex : register(s1);
	sampler2D _GrabTexture : register(s0);

	struct v2f {
		float4 pos : SV_POSITION;
		float4 uvrefr    : TEXCOORD0;
		float2 uv         : TEXCOORD1;
		float3 normal    : TEXCOORD2;
	};

	uniform float _BumpAmt;

	v2f vert(appdata_base v)
	{
		v2f o;
		o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
		o.uv = TRANSFORM_UV(1);
		o.uvrefr = mul(UNITY_MATRIX_TEXTURE0, v.vertex);
		o.normal = mul((float3x3)UNITY_MATRIX_MVP, v.normal);

		return o;
	}

	half4 frag(v2f i) : COLOR
	{
		i.normal = normalize(i.normal);

	// Calculate refracted vector based on the surface normal.
	// This is only an approximation because we don't know the
	// thickness of the object. So just use anything that looks
	// "good enough"

	half3 refracted = i.normal * abs(i.normal);
	//half3 refracted = refract( i.normal, half3(0,0,1), 1.333 );

	// perturb coordinates of the grabbed image
	i.uvrefr.xy = refracted.xy * (i.uvrefr.w * _BumpAmt) + i.uvrefr.xy;

	half4 refr = tex2Dproj(_GrabTexture, i.uvrefr);
	half4 col = tex2D(_MainTex, i.uv.xy);
	return col * refr;
	}

		ENDCG
		// Set up the textures for this pass
		SetTexture[_GrabTexture]{}    // Texture we grabbed in the pass above
		SetTexture[_MainTex]{}        // Color tint
	}
	}

		// ------------------------------------------------------------------
		// Fallback for older cards and Unity non-Pro

		SubShader{
		Blend DstColor Zero
		Pass{
		Name "BASE"
		SetTexture[_MainTex]{ combine texture }
	}
	}
	}
}