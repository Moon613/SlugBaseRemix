# Getting started with SlugBase!
## Prerequisite
This guide assumes that you already know how to make Rain World mods. If that isn't the case, see the [SlugTemplate Walkthrough](template.md) to build a mod from the ground up.

## Custom Slugcats
To create a simple SlugBase slugcat, start by making a file called `slugbase\scholar.json`. The name of this file isn't crucial and won't break if other mods name theirs the same. The contents must be a JSON object with at least the "id", "name", and "description" properties:
```json
{
  "id": "Scholar",
  "name": "The Scholar",
  "description": "This is the description of The Scholar, and will appear on the main menu.<LINE>This will appear on line 2."
}
```
- **"id"**: A unique ID that isn't displayed to the user. Your mod will fail to load if another character has this ID!
- **"name"**: The display name of this character.
- **"description"**: The description of this character on the character select menu.
- **"features"**: An optional list of settings, abilities, or other features of this character or its save slot.

This will create a copy of Survivor called "The Scholar". That's not very interesting on its own; that's where features come in!
```json
{
  "id": "Scholar",
  ...
  "features": {
    "color": "9530A1",
    "custom_colors": [
      { "name": "Body", "story": "9530A1" },
      { "name": "Eyes", "story": "FFFFFF" }
    ],
    "start_room": "SI_S04",
    "select_menu_scene": "Slugcat_Scholar"
  }
}
```
Now the Scholar is purple with white eyes, starts in Sky Islands, and will use the "Slugcat_Scholar" scene in the character select menu. See [Built-In Features](features.md) for a list of features you can use here. Most characters will need some new mechanics to set them apart from other campaigns. To implement those, see [Custom Features](custom-features.md).

Some features, such as color or stats, might take some iteration to get right. With SlugBase 2, you don't have to compile your mod and restart the game to make these changes; *just edit the JSON file and they will apply immediately!* This won't work with features that are only checked occasionally, such as "karma" or "start_room".

The Scholar will work as it is, but its character select scene will be blank. We need some sort of system for...

## Custom Scenes
Custom scenes are made in a similar way to custom characters. To get started, you make a file called `slugbase\scenes\slugcat_scholar.json`. Again, the file name does not need to be unique. The JSON object needs at least the "id", "images", and "idle_depths" properties:
```json
{
  "id": "Slugcat_Scholar",
  "scene_folder": "slugbase/scenes/slugcat_scholar",
  "images": [
    {"name": "flat",       "pos": [  0,   0], "depth": -1.0, "flatmode": true},
    {"name": "background", "pos": [442, 134], "depth":  3.5},
    {"name": "scholar",    "pos": [516, 185], "depth":  2.8, "shader": "Basic"},
    {"name": "foreground", "pos": [301,  18], "depth":  1.5}
  ],
  "idle_depths": [2.3, 2.9],
  "glow_pos": [730, 335],
  "mark_pos": [740, 575],
  "select_menu_pos": [-10, 100],
  "slugcat_depth": 2.8
}
```

- **"id"**: A unique ID. Your scene will fail to load if another scene has this ID!
- **"scene_folder"**: An optional path to the folder containing scene images. This is relative to `StreamingAssets` or your mod's folder, and defaults to `illustrations`.
- **"images"**: A list of image objects that make up this scene.
  - **"name"**: A file name to append to "scene_folder" when loading this image.
  - **"pos"**: The position of this image's bottom left corner relative to the bottom left of the screen. For an image of size `1366x768` this should be `[0,0]` to display in the center of the screen.
  - **"depth"**: The depth of this object in the scene. This only determines how it will be blurred and how parallax will be applied. Layering follows the order specified, with earlier images being layered below later ones.
  - **"shader"**: The shader to use when drawing this image. This may be "Normal", "Lighten", "LightEdges", "Rain", "Overlay", "Basic", "SoftLight", or "Multiply".
- **"idle_depths"**: A list of depths for the camera to focus on. The actual focus depth may be up to 0.5 greater than this. The focus depth will also disregard these values while the mouse is in motion. If you need an image to not be blurry, change "shader" to "Basic" and remove its depth map.

A few optional properties are specific to the character select menu:
- **"glow_pos"**: The position of the glow's center. This is typically centered on the character.
- **"mark_pos"**: The position of the mark's center. This is typically above the character's head.
- **"select_menu_pos"**: The offset of this scene when displayed in the select menu.
- **"slugcat_depth"**: The depth of the slugcat image in this scene. This is used to apply parallax to the mark and glow.
- **"dream_override"**: If this scene is used as a dream, should it override Moon and Pebble dreams being set.

Hmmm, but wouldn't it be cool if we could also have an intro slideshow like Survivor or Gourmand...

## Custom Slideshows
Custom Slideshows are similar to Custom Scenes in implementation and share some commonalities. Much the same way you created your Character Select Menu, create a file called `slugbase\slideshows\slugecat_scholar.json`. The file name also does not need to be unique. The JSON object needs at least the "id", "next_process", "scenes" properties. Each entry in the "scenes" property also requires the properties "name", and "images":
```json
{
  "id": "Scholar_Intro",
  "slideshow_folder": "slugbase/slideshows/scholar_intro",
	"next_process": "Game",
  "music": {
    "name": "RW_Intro_Theme",
    "fade_in": 40
  },
  "scenes": [
    {
      "name": "scene1",
      "images": [
        {
          "name": "background1",
          "pos": [0,0],
          "depth": 0.2,
          "shader": "Basic"
        },
        {
          "name": "scholar1",
          "pos": [0,0],
          "depth": 0.3
        }
      ],
      "movements": [
        [10, 15],
        [8.7, 5]
      ]
    },
    {
      "name": "scene2",
      "fade_in": 8.1,
      "fade_in_finish": 11.25,
      "fade_out_start": 17,
      "images": [
        {
          "name": "background2",
          "pos": [0,0],
          "depth": 0.1,
          "shader": "Basic"
        },
        {
          "name": "background3",
          "pos": [100,65],
          "depth": 0.15,
          "shader": "Basic"
        },
        {
          "name": "scholar2",
          "pos": [10,10],
          "depth": 0.21,
          "shader": "Basic"
        }
      ],
      "movements": [
        [-10,-25],
        [-2.5,4],
        [-3,7]
      ]
    }
  ]
}
```

- **"id"**: A unique ID. Your slideshow may fail to load if another slideshow has this ID!
- **"slideshow_folder"**: An optional path to the folder containing slideshow images. This is relative to `StreamingAssets` or your mod's folder, and defaults to `illustrations`.
- **"next_process"**: The process to go to when this slideshow is done playing. Common options will be `Game` for intro slideshows, and `Credits` or `Statistics` for outros.
- **"music"**: Play some music during your slideshow! Nothing will play if this goes unspecified.
  - **"name"**: The name of the music to play, should exactly match the name of one of the mp3 files in `StreamingAssets\music\songs`. A custom song can be added as easily as adding a mp3 file in `mymod\music\songs\mymusic.mp3`.
  - **"fade_in"**: The music will gradually fade in, starting from 0 and lasting until the time in seconds specified here.
- **"scenes"**: Groups of images to display, and when to display them. Each scene object here is similar to a `Custom Scene` but not related.
  - **"name"**: This only needs to be unique in relation to other scenes in this slideshow's JSON.
  - **"fade_in"**: The time in seconds for the scene's images to start fading in. If unspecified this defaults to `0`.
  - **"fade_in_finish"**: The time in seconds when the scene's images finish their fade in animation. If unspecified this defaults to `3`.
  - **"fade_out_start"**: The time in seconds when the scene's images begin to fade out. Id unspecified this defaults to `8`.
  - **"images"**: The images to use in this scene, all displayed at once, with those in the list first being placed behind those later in the list.
    - **"name"**: The name of the image to use, must match exactly.
    - **"pos"**: The position of this image's bottom left corner relative to the bottom left of the screen. For an image of size `1366x768` this should be `[0,0]` to display in the center of the screen.
    - **"depth"**: The depth of the image, should be in range from `0.0-1.0`. Values closer to 0 make the images less effected by movement.
    - **"shader"**: The shader to use when drawing this image. This may be "Normal", "Lighten", "LightEdges", "Rain", "Overlay", "Basic", "SoftLight", or "Multiply".
  - **"movements"**: A list of what dynamic movements the images in the scene will make. Values are relative to the initial position of images. If unspecified there will be no movement.

Slideshows can be used for Intros when starting the game, and Outros when finishing the game. You can call a slideshow manually at any point by using `SlugBase.Assets.CustomSlideshow.NewOutro(ProcessManager, string, float)`