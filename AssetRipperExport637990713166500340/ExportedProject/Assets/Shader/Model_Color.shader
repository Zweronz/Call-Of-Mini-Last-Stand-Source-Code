Shader "Triniti/Model_Color" {
Properties {
 _Color ("Color", Color) = (0.5,0.5,0.5,0.5)
 _MainTex ("MainTex", 2D) = "" {}
}
SubShader { 
 Tags { "QUEUE"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" }
  Cull Off
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_MainTex] { ConstantColor [_Color] combine texture * constant double, texture alpha * constant alpha }
 }
}
}