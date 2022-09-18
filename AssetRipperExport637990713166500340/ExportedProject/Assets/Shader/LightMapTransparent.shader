Shader "Triniti/LightMapTransparent" {
Properties {
 _texBase ("MainTex", 2D) = "" {}
 _texLightmap ("LightMap", 2D) = "" {}
}
SubShader { 
 Tags { "QUEUE"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "texcoord", TexCoord0
   Bind "texcoord1", TexCoord1
  }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_texBase] { combine texture }
  SetTexture [_texLightmap] { combine previous * texture }
 }
}
}