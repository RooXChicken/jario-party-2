@tool
class_name Globals extends Node

enum CharacterType {
	JARIO,
	WOOIGI,
	GRAPEJUICE,
	JOSH
}

static var Characters = {
	CharacterType.JARIO:
		CharacterData.new("Jario", "res://sprites/objects/characters/playable/jario_sprites.tres", Color.WHITE),
	CharacterType.WOOIGI:
		CharacterData.new("Wooigi", "res://sprites/objects/characters/playable/wooigi_sprites.tres", Color.WHITE),
	CharacterType.GRAPEJUICE:
		CharacterData.new("Grapejuice", "res://sprites/objects/characters/playable/grapejuice_sprites.tres", Color.WHITE),
	CharacterType.JOSH:
		CharacterData.new("Josh", "res://sprites/objects/characters/playable/josh_sprites.tres", Color.WHITE),
}
