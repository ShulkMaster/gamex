#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;

uniform mat4 projection;
uniform mat4 view;
uniform mat4 model;
uniform mat3 invertModel;

out vec3 fragPos;
out vec3 Normal;

void main()
{
    Normal = invertModel * aNormal;
    vec4 temp = view * vec4(aPosition, 1.0);
    fragPos = temp.xyz;
    gl_Position = projection * view * vec4(aPosition, 1.0);
}