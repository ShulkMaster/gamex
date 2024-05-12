#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;

uniform mat4 projection;

out vec3 fragPos;
out vec3 Normal;

void main()
{
    vec4 location = projection * vec4(aPosition, 1.0);
    Normal = aNormal;
    fragPos = aPosition;
    gl_Position = location;
}