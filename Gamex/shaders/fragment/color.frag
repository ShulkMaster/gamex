#version 330 core

struct Material {
  vec3 ambient;
  vec3 diffuse;
};

in vec3 Normal;

uniform Material material;

out vec4 FragColor;

void main()
{
    FragColor = vec4(material.diffuse * normalize(abs(Normal)), 1.0f);
}