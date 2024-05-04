#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;

uniform mat4 rotationMatrix;

out vec4 transformedPosition;
out vec3 Normal;

void main()
{
    transformedPosition = rotationMatrix * vec4(aPosition, 1.0);
    Normal = aNormal;
    gl_Position = transformedPosition;
}