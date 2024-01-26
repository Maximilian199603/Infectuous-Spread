extends Node2D

#GetReference to Cellancestor
var par;
# Called when the node enters the scene tree for the first time.
func _ready():
	par = get_parent();
	pass # Replace with function body.

func _draw() -> void:
	DrawLineTo(Color.black, 8.0);
	pass


func DrawLineTo(col: Color, thickness: float):
	draw_line(position, Vector2.UP, col, thickness);
	pass
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
