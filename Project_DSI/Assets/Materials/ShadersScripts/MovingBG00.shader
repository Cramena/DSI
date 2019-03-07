// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MovingBG"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_T_BigGausianNoise03("T_BigGausianNoise03", 2D) = "white" {}
		_Color0("Color 0", Color) = (0,0,0,0)
		_Color1("Color 1", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color0;
		uniform float4 _Color1;
		uniform sampler2D _TextureSample0;
		uniform sampler2D _T_BigGausianNoise03;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord5 = i.uv_texcoord * float2( 0.5,0.5 );
			float2 panner14 = ( 0.02 * _Time.y * float2( 0,-1 ) + uv_TexCoord5);
			float cos4 = cos( 0.02 * _Time.y );
			float sin4 = sin( 0.02 * _Time.y );
			float2 rotator4 = mul( uv_TexCoord5 - float2( 0,1 ) , float2x2( cos4 , -sin4 , sin4 , cos4 )) + float2( 0,1 );
			float4 lerpResult23 = lerp( _Color0 , _Color1 , ( tex2D( _TextureSample0, panner14 ).r * tex2D( _T_BigGausianNoise03, rotator4 ).r ));
			o.Albedo = lerpResult23.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
1793.667;166.6667;1792;1044;1659.321;1160.938;1.732502;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-1215.6,124.9;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.5,0.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;14;-781.328,-203.5443;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-1;False;1;FLOAT;0.02;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode;4;-779,118;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,1;False;2;FLOAT;0.02;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;25;-35.46171,-871.9676;Float;False;284;257;Comment;1;22;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;24;-61.46171,-506.9676;Float;False;284;257;Comment;1;26;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;18;-531.3348,-271.8986;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;None;45742ecc0417503409238948ceea4ce2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;17;-542.7847,-44.65271;Float;True;Property;_T_BigGausianNoise03;T_BigGausianNoise03;1;0;Create;True;0;0;False;0;None;45742ecc0417503409238948ceea4ce2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-104.3348,-118.8986;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;22;14.53829,-821.9676;Float;False;Property;_Color0;Color 0;2;0;Create;True;0;0;False;0;0,0,0,0;0.1839623,0.2695459,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;26;0.4153669,-441.9496;Float;False;Property;_Color1;Color 1;3;0;Create;True;0;0;False;0;0,0,0,0;0.1839623,0.2695459,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;10;-779,-73;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,1;False;1;FLOAT;0.05;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode;6;-767,317;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;-0.15,0.2;False;2;FLOAT;0.05;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;23;362.7866,-283.2709;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;758.2999,-149.2;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;MovingBG;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;14;0;5;0
WireConnection;4;0;5;0
WireConnection;18;1;14;0
WireConnection;17;1;4;0
WireConnection;19;0;18;1
WireConnection;19;1;17;1
WireConnection;10;0;5;0
WireConnection;6;0;5;0
WireConnection;23;0;22;0
WireConnection;23;1;26;0
WireConnection;23;2;19;0
WireConnection;0;0;23;0
ASEEND*/
//CHKSM=0C1463C736FBAD6E7C70C020DECFC07A8F01053A