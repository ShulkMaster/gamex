#version 330 core

uniform vec3 color;

in vec3 Normal;

out vec4 FragColor;

void main() {
    vec3 n = abs(normalize(Normal));
    FragColor = vec4(n, 1);
}