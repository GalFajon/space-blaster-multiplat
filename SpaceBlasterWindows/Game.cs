using System;
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

namespace SpaceBlasterWindows
{
    public class SpaceBlasterWindows : SpaceBlasterGame
    {
        public SpaceBlasterWindows() : base() {
            Settings.getStream = (writing) => {
                if (writing)
                {
                    File.WriteAllText("./Content/settings.json", "");
                    return File.OpenWrite("./Content/settings.json");
                }
                else return File.OpenRead("./Content/settings.json");
            };

            RoomLayouts.GetStream = (int i) => {
                return File.OpenRead("./Content/room-"+i+".json");
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
