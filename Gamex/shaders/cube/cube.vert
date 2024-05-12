#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;

uniform mat4 projectionM;

void main() {
    vec4 location = projectionM * vec4(aPosition, 1.0);
    gl_Position = location;
}
