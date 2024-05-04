#version 330 core

struct Material {
  vec3 ambient;
  vec3 diffuse;
};

struct Light {
  vec3 loc;
  vec3 color;
};

in vec3 Normal;
in vec3 fragPos;

uniform Material material;
uniform Light light;

out vec4 FragColor;

void main()
{
    vec3 ambient = material.ambient * light.color;
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.loc - fragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = material.diffuse * light.color * diff;
    vec3 result = ambient + diffuse;
    FragColor = vec4(result, 1.0f);
}