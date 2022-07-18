using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotDownwell.Scenes
{
    public abstract class Scene
    {
        public Scene()
        {
            DoInit();
        }
        public virtual void DoInit() { }
        public virtual void DoExit() { }
        public virtual void Update() { }
        public virtual void Draw() { }
        public static void MoveScene(Scene current, Scene next)
        {
            Main.currentScene = next;
        }
    }
}
