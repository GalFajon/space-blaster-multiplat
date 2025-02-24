﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Gameplay.Audio;
using GameEngine;
using GameSpecific.LevelGenerator;
using GameSpecific;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using SpaceBlaster;
using System.IO;

namespace SpaceBlasterAndroid
{
    public class SpaceBlasterAndroid : SpaceBlasterGame
    {
        public SpaceBlasterAndroid() : base() {
            Settings.getStream = (writing) => {
                if (!File.Exists(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "/settings.json"))) {
                    if (!Directory.Exists(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Personal)))) 
                        Directory.CreateDirectory(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Personal)));

                    var s = File.Create(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "/settings.json"));
                    s.Close();

                    Settings.Save();
                }

                if (writing)
                {
                    File.WriteAllText(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "/settings.json"),"");
                    return new FileStream(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "/settings.json"), FileMode.Open, FileAccess.ReadWrite);
                }
                else return new FileStream(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "/settings.json"), FileMode.Open, FileAccess.Read);
            };

            RoomLayouts.GetStream = (int i) => {
                return Game.Activity.Assets.Open("Content/room-"+i+".json");
            };
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
