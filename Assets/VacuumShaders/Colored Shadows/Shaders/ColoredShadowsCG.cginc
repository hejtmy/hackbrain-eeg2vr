// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

#ifndef V_COLORED_SHADOWS_CG
#define V_COLORED_SHADOWS_CG



uniform float4 _LightColor0;

fixed4 _Color;
sampler2D _MainTex;
half4 _MainTex_ST;
fixed _V_CSH_ShadowIntencity;
fixed _V_CSH_ShadowEmissive;

#ifdef DECAL_ON
	sampler2D _DecalTex;
	half4 _DecalTex_ST;
#endif

#ifdef SPECULAR_ON
	half _Shininess;
	fixed4 _SpecColor;
#endif
#ifdef BUMP_ON
	sampler2D _BumpMap;
	half4 _BumpMap_ST;
#endif
#ifdef REFLECTION_ON
	fixed4 _ReflectColor;
	samplerCUBE _Cube;
#endif

#ifdef PARALLAX_ON
	half _Parallax;
	sampler2D _ParallaxMap;
	half4 _ParallaxMap_ST;
#endif

#ifdef CUTOUT_ON
	half _Cutoff;
#endif

#ifdef V_CSH_COLOR_CUSTOMCOLOR
	fixed4 _V_CSH_ShadowColor;
#endif

#if defined(V_CSH_TEXTURE_RGB) || defined(V_CSH_TEXTURE_RGBA)
	sampler2D _V_CSH_ShadowTex;
	half4 _V_CSH_ShadowTex_ST;
#endif

#ifdef V_CSH_COLOR_GRADIENT
	sampler2D _V_CSH_GradientTex;
#endif


#define WorldReflectionVector(data,normal) reflect (half3(data.TtoW0.w, data.TtoW1.w, data.TtoW2.w), half3(dot(data.TtoW0,normal), dot(data.TtoW1,normal), dot(data.TtoW2,normal)))

#if defined(V_CSH_TEXTURE_RGBA) && defined(V_CSH_SOLID_SHADOW_ON)
#undef V_CSH_SOLID_SHADOW_ON
#endif

struct appdata 
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float2 texcoord0 : TEXCOORD0;

	#ifdef V_NEED_TANGENT
		float4 tangent : TANGENT;
	#endif
};


v2f vert (appdata v) 
{
    v2f o = (v2f)0;

    o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
    o.uv.xy = v.texcoord0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
	#if defined(V_CSH_TEXTURE_RGB) || defined(V_CSH_TEXTURE_RGBA)
		o.uv.zw = v.texcoord0.xy * _V_CSH_ShadowTex_ST.xy + _V_CSH_ShadowTex_ST.zw;
	#endif
	
	#ifdef DECAL_ON
		o.uvDecal = v.texcoord0.xy * _DecalTex_ST.xy + _DecalTex_ST.zw;
	#endif

	#ifdef BUMP_ON
		o.uv2.xy = v.texcoord0.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
	#endif
	#ifdef PARALLAX_ON
		o.uv2.zw = v.texcoord0.xy * _ParallaxMap_ST.xy + _ParallaxMap_ST.zw;
	#endif 


	#ifdef NEED_CALC_TANGET_ROTATION
		half3 binormal = cross( v.normal, v.tangent.xyz ) * v.tangent.w; 
		half3x3 rotation = half3x3( v.tangent.xyz, binormal, v.normal );
	#endif

	#ifdef NEED_CALC_NORMAL_WS
		half3 normal_WS = (mul((half3x3)unity_ObjectToWorld, v.normal));		
	#endif
    
	#ifdef NEED_CALC_LIGHTDIR_WS
		#ifdef UNITY_PASS_FORWARDBASE
			half3 ligthDir_WS = _WorldSpaceLightPos0.xyz;
		#endif
		#ifdef UNITY_PASS_FORWARDADD
			half3 ligthDir_WS = _WorldSpaceLightPos0.xyz - mul(unity_ObjectToWorld, v.vertex).xyz;
		#endif
	#endif
	#ifdef NEED_CALC_LIGHTDIR_TS
		half3 lightDir_TS = mul (rotation, ObjSpaceLightDir(v.vertex));
	#endif

	#ifdef NEED_CALC_VIEWDIR_WS
		half3 viewDir_WS = WorldSpaceViewDir( v.vertex );
	#endif
	#ifdef NEED_CALC_VIEWDIR_TS
		half3 viewDir_TS = mul (rotation, ObjSpaceViewDir(v.vertex));
	#endif

	#ifdef NEED_CALC_REFL_WS
		float3 viewDir = -ObjSpaceViewDir(v.vertex);
	    float3 viewRefl = reflect (viewDir, v.normal);
	    half3 refl_WS = mul ((float3x3)unity_ObjectToWorld, viewRefl);
	#endif

	#ifdef NEED_CALC_TANGENT_NORAML_REFL
		float3 viewDir = -ObjSpaceViewDir(v.vertex);
		float3 worldRefl = mul ((float3x3)unity_ObjectToWorld, viewDir);

		float4 TtoW0 = float4(mul(rotation, unity_ObjectToWorld[0].xyz), worldRefl.x)*1.0;
	    float4 TtoW1 = float4(mul(rotation, unity_ObjectToWorld[1].xyz), worldRefl.y)*1.0;
		float4 TtoW2 = float4(mul(rotation, unity_ObjectToWorld[2].xyz), worldRefl.z)*1.0;
	#endif



	#ifdef NEED_P_NORMAL_WS
		o.normal = normal_WS;
	#endif
	#ifdef NEED_P_LIGHTDIR_WS
		o.ligthDir = ligthDir_WS;
	#endif
	#ifdef NEED_P_LIGHTDIR_TS
		o.ligthDir = lightDir_TS;
	#endif
	#ifdef NEED_P_VIEWDIR_WS
		o.viewDir = viewDir_WS;
	#endif
	#ifdef NEED_P_VIEWDIR_TS
		o.viewDir = viewDir_TS;
	#endif
	#ifdef NEED_P_REFL_WS
		o.refl = refl_WS;
	#endif
	#ifdef NEED_P_TANGENT_NORMAL_REFL
		o.TtoW0 = TtoW0;
		o.TtoW1 = TtoW1;
		o.TtoW2 = TtoW2;
	#endif

	                
    TRANSFER_VERTEX_TO_FRAGMENT(o)
    return o;
}

fixed4 frag(v2f IN) : COLOR 
{                
	#ifdef PARALLAX_ON
		half h = tex2D (_ParallaxMap, IN.uv2.zw).w;
		float2 offset = ParallaxOffset (h, _Parallax, IN.viewDir);
		IN.uv.xy += offset;
		IN.uv.zw += offset;
		IN.uv2.xy += offset;
	#endif

	fixed4 baseTex = tex2D (_MainTex, IN.uv.xy);
    fixed4 c = baseTex;

	#ifdef DECAL_ON
		half4 decal = tex2D(_DecalTex, IN.uvDecal);
		c.rgb = lerp (c.rgb, decal.rgb, decal.a);
	#endif

	c *= _Color;

	#ifdef CUTOUT_ON
		clip (c.a - _Cutoff);
	#endif

	_V_CSH_ShadowIntencity = clamp(_V_CSH_ShadowIntencity, 0, 1);

    fixed atten = LIGHT_ATTENUATION(IN);
	#ifdef UNITY_PASS_FORWARDADD
		fixed shadowAtten = SHADOW_ATTENUATION(IN);		
	#endif

	

	#ifdef BUMP_ON
		half3 normal = UnpackNormal(tex2D(_BumpMap, IN.uv2.xy));
	#else
		half3 normal = IN.normal;
	#endif
	
	
	half3 lightDir = IN.ligthDir;
	#ifdef UNITY_PASS_FORWARDADD
		lightDir = normalize(lightDir); 
	#endif

	fixed3 shadowColor = 0;
	fixed lightIntencity = length(_LightColor0.rgb);
	fixed3 lightColor = _LightColor0.rgb / lightIntencity;

	#ifdef V_CSH_COLOR_CUSTOMCOLOR
		shadowColor = _V_CSH_ShadowColor.rgb;
	#endif
	#ifdef V_CSH_COLOR_LIGHTCOLOR
		shadowColor = lightColor * 2;		
	#endif
	#ifdef V_CSH_COLOR_INVERTLIGHTCOLOR
		shadowColor = 1 - lightColor * 2;
	#endif


			
	
	#ifdef UNITY_PASS_FORWARDBASE
		fixed nDotL = max (0, dot (normal, lightDir));
		fixed3 diff = nDotL * lightIntencity;


				
		#ifdef V_CSH_DISABLE_LIGHT_COLORES_ON
			diff *= 1.15;
		#else			
			diff *= lightColor * 2;
		#endif
		
		#ifdef V_CSH_COLOR_GRADIENT
			shadowColor = tex2D(_V_CSH_GradientTex, half2(nDotL, 0.5)).rgb;
		#endif
		#if defined(V_CSH_TEXTURE_RGB) || defined(V_CSH_TEXTURE_RGBA)
			half4 shadowTex = tex2D(_V_CSH_ShadowTex, IN.uv.zw);
			shadowColor.rgb *= shadowTex.rgb;		
			shadowTex.a *= _V_CSH_ShadowIntencity;
		#endif	


		fixed3 albedo = c.rgb * diff;	

		fixed3 shadow = shadowColor.rgb *  _V_CSH_ShadowIntencity * lightIntencity;	
		#ifdef V_CSH_EMISSION_TYPE_MULTIPLY
			shadow *= (1 + _V_CSH_ShadowEmissive) * nDotL;
		#endif
		#ifdef V_CSH_EMISSION_TYPE_EXPONENTAL
			shadow *= (1 + exp(_V_CSH_ShadowEmissive * 10)) * nDotL;
		#endif

		#ifdef V_CSH_TEXTURE_RGBA
			shadow = lerp(albedo, shadow, shadowTex.a);			  		
		#endif		
				
		c.rgb = lerp(shadow, albedo, clamp((atten + 1 -_V_CSH_ShadowIntencity), 0, 1)) + c.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * 2;
	#endif
		
		
	#ifdef UNITY_PASS_FORWARDADD
		fixed nDotL = max (0, dot (normal, lightDir));
		fixed3 diff = nDotL * lightIntencity;

		#ifndef V_CSH_DISABLE_LIGHT_COLORES_ON
			diff *= lightColor * 2;
		#endif

		#ifdef V_CSH_COLOR_GRADIENT
			shadowColor = tex2D(_V_CSH_GradientTex, half2(nDotL, 0.5)).rgb;
		#endif			
		#if defined(V_CSH_TEXTURE_RGB) || defined(V_CSH_TEXTURE_RGBA)
			half4 shadowTex = tex2D(_V_CSH_ShadowTex, IN.uv.zw);
			shadowColor.rgb *= shadowTex.rgb;		
			shadowTex.a *= _V_CSH_ShadowIntencity;
		#endif						
				 
		fixed3 albedo = c.rgb * diff * atten;
		
						
		fixed3 shadow = shadowColor.rgb * (1 - shadowAtten) * (atten - 0.001) * _V_CSH_ShadowIntencity * lightIntencity;	
		#ifdef V_CSH_EMISSION_TYPE_MULTIPLY
			shadow *= (1 + _V_CSH_ShadowEmissive) * nDotL;
		#endif
		#ifdef V_CSH_EMISSION_TYPE_EXPONENTAL
			shadow *= (1 + exp(_V_CSH_ShadowEmissive * 10)) * nDotL;
			shadow = max(shadow, 0);
		#endif

		#ifdef V_CSH_TEXTURE_RGBA
			shadow = lerp(albedo, shadow, shadowTex.a);			  		
		#endif	

		c.rgb = lerp(shadow, albedo, clamp(shadowAtten + 1 - _V_CSH_ShadowIntencity, 0, 1));
	#endif

				
	#ifdef SPECULAR_ON
		half nh = max (0, dot (normal, normalize (lightDir + normalize(IN.viewDir))));
		half spec = pow (nh, _Shininess * 128.0) * baseTex.a;
		half3 specCol = lightColor *  _SpecColor.rgb * spec;

		
		#ifdef UNITY_PASS_FORWARDBASE
			#ifdef V_CSH_TEXTURE_RGBA
				specCol *= lerp((1 - shadowTex.a * _V_CSH_ShadowIntencity), 1, atten);
			#else
				specCol *= lerp(1 - _V_CSH_ShadowIntencity, 1, atten);
			#endif

			c.rgb += specCol * lightIntencity * 2;
		#endif


		#ifdef UNITY_PASS_FORWARDADD
			#ifdef V_CSH_TEXTURE_RGBA
				specCol *= lerp((1 - shadowTex.a), 1, shadowAtten);
			#else
				specCol *= lerp(1 -_V_CSH_ShadowIntencity, 1, shadowAtten);
			#endif

			c.rgb += specCol * atten * lightIntencity * 2;
		#endif				
	#endif

	#if defined(REFLECTION_ON) && !defined(UNITY_PASS_FORWARDADD)
		#ifdef BUMP_ON
			c.rgb += texCUBE(_Cube, WorldReflectionVector(IN, normal)).rgb * _ReflectColor.rgb * baseTex.a;
		#else
			c.rgb += texCUBE(_Cube, IN.refl).rgb * _ReflectColor.rgb * baseTex.a;
		#endif
	#endif

	
	return c;
}


#endif	//file