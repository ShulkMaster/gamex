#version 330 core

struct Material {
  vec3 ambient;
  vec3 diffuse;
};

uniform Material material;
out vec4 FragColor;

void main()
{
    FragColor = vec4(material.diffuse, 1.0f);
}