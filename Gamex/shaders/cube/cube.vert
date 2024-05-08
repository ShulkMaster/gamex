#version 330 core
layout (location = 0) in vec3 aPosition;

uniform mat4 projectionM;

void main() {
    vec4 location = projectionM * vec4(aPosition, 1.0);
    gl_Position = location;
}
