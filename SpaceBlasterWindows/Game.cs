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
                    if (!File.Exists(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "./SpaceBlaster/settings.json")))
                    {
                        if (!Directory.Exists(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "./SpaceBlaster/")))
                            Directory.CreateDirectory(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "./SpaceBlaster/"));

                        var s = File.Create(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "./SpaceBlaster/settings.json"));
                        s.Close();

                        Settings.Save();
                    }

                    File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "./SpaceBlaster/settings.json"), "");
                    return File.OpenWrite(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "./SpaceBlaster/settings.json"));
                }
                else return File.OpenRead(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "./SpaceBlaster/settings.json"));
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
