using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Battleship.Scenes;

namespace Battleship.Handlers
{
    public class SceneHandler : Handler
    {
        public Scene? CurrentScene;

        /// <summary>
        /// Starts/switches scenes
        /// </summary>
        /// <param name="scene"></param>
        public void Start(Scene scene)
        {
            CurrentScene = scene;
            CurrentScene.Draw();
        }

        public void Update()
        {
            if (CurrentScene != null)
            {
                if (CurrentScene.IsAlive)
                {
                    CurrentScene.Update();

                    if (CurrentScene.RequestedScene != null)
                    {
                        Start(CurrentScene.RequestedScene);
                    }
                    else
                    {
                        CurrentScene.Draw();
                    }
                } else
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}
