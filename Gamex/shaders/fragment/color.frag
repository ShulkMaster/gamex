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
    float ambientStrength = 0.1;
    vec3 ambient = ambientStrength * material.ambient;

    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.loc - fragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * light.color;

    vec3 result = (ambient + diffuse) * material.diffuse;
    
    FragColor = vec4(result, 1.0f);
}