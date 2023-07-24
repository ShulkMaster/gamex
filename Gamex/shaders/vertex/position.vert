#version 330 core
uniform mat4 rotationMatrix;
layout (location = 0) in vec3 aPosition;
out vec4 transformedPosition;

void main()
{
    transformedPosition = rotationMatrix * vec4(aPosition, 1.0);
    gl_Position = transformedPosition;
}