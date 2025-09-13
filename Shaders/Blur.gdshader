shader_type canvas_item;

uniform float radius : hint_range(1.0, 15.0, 1.0) = 6.0;
uniform float sigma : hint_range(0.1, 10.0, 0.1) = 4.0;

float gaussian(float x, float s) {
    return exp(-(x * x) / (2.0 * s * s)) / (sqrt(6.28318) * s);
}

void fragment() {
    vec2 texel = SCREEN_PIXEL_SIZE; // pixel size
    vec4 sum = vec4(0.0);
    float weight_sum = 0.0;

    // Sample a square kernel [-radius, radius] in both directions
    for (int x = -20; x <= 20; x++) {
        if (abs(x) > int(radius)) continue;
        for (int y = -20; y <= 20; y++) {
            if (abs(y) > int(radius)) continue;

            float w = gaussian(float(x), sigma) * gaussian(float(y), sigma);
            vec2 offset = vec2(float(x) * texel.x, float(y) * texel.y);
            sum += texture(TEXTURE, UV + offset) * w;
            weight_sum += w;
        }
    }

    COLOR = sum / weight_sum;
}
