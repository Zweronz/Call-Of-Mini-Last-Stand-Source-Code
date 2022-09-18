Shader "Triniti/ModelTransparentForShadow" {
Properties {
 _MainTex ("MainTex", 2D) = "" {}
}
SubShader { 
 Tags { "QUEUE"="Geometry+1" }
 Pass {
  Tags { "QUEUE"="Geometry+1" }
  Cull Off
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_MainTex] { combine texture, texture alpha }
 }
}
}