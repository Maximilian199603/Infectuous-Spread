shader_type canvas_item;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec2 distortedUv = UV +  (texture(TEXTURE,UV).t * texture(TEXTURE,UV).t * 2.0 - texture(TEXTURE,UV).t * 0.5);
	vec2 distroted = UV + distortedUv - 0.5;
	vec4 pixelColor = texture(TEXTURE, distroted);
	COLOR = pixelColor;
	// Called for every pixel the material is visible on.
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
