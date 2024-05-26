#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;

uniform mat4 projectionM;
uniform mat4 view;
uniform mat4 model;

void main() {
    gl_Position = projectionM * view * model * vec4(aPosition, 1.0);
}
