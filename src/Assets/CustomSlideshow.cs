﻿using UnityEngine;
using SlideShowID = Menu.SlideShow.SlideShowID;
using System;
using System.Linq;
using System.IO;
using static SlugBase.JsonUtils;
using System.Collections.Generic;
using Menu;

namespace SlugBase.Assets
{
    /// <summary>
    /// An intro cutscene added by SlugBase.
    /// </summary>
    public class CustomSlideshow
    {
        /// <summary>
        /// Must match the ID to the id in a slideshow's json file, and provide the ProcessManager, in order to play an outro slideshow
        /// </summary>
        /// <param name="ID">The ID of the slideshow to play, should be declared as a new Menu.SlideShow.SlideShowID(string, false) with the string matching the id of a slugbase slideshow .json file.</param>
        /// <param name="manager">The ProcessManager, needed to change the active process.</param>
        public static void NewOutro(string ID, ProcessManager manager)
        {
            manager.nextSlideshow = new Menu.SlideShow.SlideShowID(ID, false);
            manager.RequestMainProcessSwitch(ProcessManager.ProcessID.SlideShow);
        }

        private static void GetScene(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stores all registered <see cref="CustomSlideshow"/>s.
        /// </summary>
        public static JsonRegistry<SlideShowID, CustomSlideshow> Registry { get; } = new((key, json) => new(key, json));

        /// <summary>
        /// This scene's unique ID.
        /// </summary>
        public SlideShowID ID { get; }

        /// <summary>
        /// A path relative to StreamingAssets to load images from.
        /// </summary>
        public string SlideshowFolder { get; }

        /// <summary>
        /// The music to play during a custom intro or outro
        /// </summary>
        public SlideshowMusic Music { get; }

        /// <summary>
        /// An array of images and other data in this scene.
        /// </summary>
        public CustomSlideshowScene[] Scenes { get; }

        /// <summary>
        /// The process to go to after playing the slideshow
        /// </summary>
        public ProcessManager.ProcessID Process { get; }

        private CustomSlideshow(SlideShowID id, JsonObject json)
        {
            ID = id;

            Scenes = json.GetList("scenes")
                .Select(img => new CustomSlideshowScene(img.AsObject()))
                .ToArray();

            SlideshowFolder = json.TryGet("slideshow_folder")?.AsString().Replace('/', Path.DirectorySeparatorChar);
            // Don't know if I should force it to defalut to the normal intro theme or leave it empty so that it's an option for people to not have any music (But who would choose that? Someone probably)
            // In order to use a custom song, it must be in .ogg format, and placed in mods/MyMod/music/songs directory (Thank the Videocult overlords it's that simple)
            if (json.TryGet("music") is JsonAny music) { Music = new SlideshowMusic(music.AsObject()); }

            Process = new ProcessManager.ProcessID(json.GetString("next_process"), false);
        }

        /// <summary>
        /// A scene from a <see cref="CustomSlideshow"/> that holds data about when to appear and what images to use for what amount of time
        /// </summary>
        public class CustomSlideshowScene : CustomScene
        {

            /// <summary>
            /// The second that this scene will start fading in
            /// </summary>
            public int StartAt { get; set; }

            /// <summary>
            /// The second that this image will finish fading in
            /// </summary>
            public int FadeInDoneAt { get; set; }

            /// <summary>
            /// The second that this image will start fading out at
            /// </summary>
            public int FadeOutStartAt { get; set; }

            /// <summary>
            /// The positions that the images will try to go to, if they are not in flatMode (Determined by the game)
            /// </summary>
            public Vector2[] Movement { get; set; }

            /// <summary>
            /// Creates a new Scene from JSON.
            /// </summary>
            /// <param name="json">The JSON data to load from.</param>
            public CustomSlideshowScene (JsonObject json) : base(new Menu.MenuScene.SceneID(json.GetString("name"), false), json)
            {
                StartAt = json.TryGet("fade_in")?.AsInt() ?? 0;
                FadeInDoneAt = json.TryGet("fade_in_finish")?.AsInt() ?? 3;
                FadeOutStartAt = json.TryGet("fade_out_start")?.AsInt() ?? 8;
                Movement = json.TryGet("movements")?.AsList().Select(vec => ToVector2(vec)).ToArray() ?? new Vector2[1]{new(0,0)};
            }
        }

        /// <summary>
        /// Data about a song from a <see cref="CustomSlideshow"/>.
        /// </summary>
        public class SlideshowMusic{

            /// <summary>
            /// The file name of the sound to use. This comes from the 'StreamingAssets/music/songs' folder.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The amount of time the sound will fade in for, until it is at full volume.
            /// </summary>
            public float FadeIn { get; set; }
            
            /// <summary>
            /// Creates new data about a song to play.
            /// </summary>
            /// <param name="name">The sound name.</param>
            public SlideshowMusic(string name)
            {
                Name = name;
            }

            /// <summary>
            /// Creates data about a song to play.
            /// </summary>
            /// <param name="name">The sound name.</param>
            /// <param name="fadeIn">The time for the music to fade in to full volume.</param>
            public SlideshowMusic(string name, float fadeIn) : this(name)
            {
                FadeIn = fadeIn;
            }

            /// <summary>
            /// Creates data about a song to play from a JSON
            /// </summary>
            /// <param name="json">The JSON data to load from.</param>
            public SlideshowMusic(JsonObject json) : this(json.GetString("name"))
            {
                if (json.TryGet("fadein") is JsonAny fadeIn)
                {
                    FadeIn = fadeIn.AsFloat();
                }
                else
                {
                    FadeIn = 40f;
                }
            }
        }
    }
}
