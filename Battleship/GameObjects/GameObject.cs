using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Battleship.Handlers;
using Battleship.Interfaces;

namespace Battleship.GameObjects
{
    public abstract class GameObject : IGameObject
    {
        public GameObject() 
        {
            Grid = ConfigHandler.GetInt("Grid");
        }

        internal ConfigHandler ConfigHandler = new();

        internal int Grid { get; }
    }
}
