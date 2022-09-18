Shader "iPhone/CutAlpha" {
Properties {
 _MainTex ("Particle Texture", 2D) = "white" {}
 _Alpha ("Alpha", Range(0,1)) = 0.9
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="True" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="True" }
  ZWrite Off
  Cull Off
  Blend SrcAlpha Zero
  AlphaTest Greater [_Alpha]
  ColorMask RGB
  SetTexture [_MainTex] { combine texture }
 }
}
}