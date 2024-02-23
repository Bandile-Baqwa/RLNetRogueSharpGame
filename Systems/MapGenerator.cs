using RLNETConsoleGame.Core;
using RogueSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLNETConsoleGame.Systems
{
    internal class MapGenerator
    {
        private readonly int _width;
        private readonly int _height;

        private readonly DungeonMap _map;

        // constructing a new MapGenerator requires the demensions of the maps it will create
        public MapGenerator(int width, int height)
        {
            _width = width;
            _height = height;
            _map = new DungeonMap();
        }
        public DungeonMap CreateMap()
        {
            //inistailize the cells in the map by setting walkable, transparency and explored to true
            _map.Initialize(_width, _height);
            foreach ( Cell cell in _map.GetAllCells())
            {
                _map.SetCellProperties(cell.X, cell.Y, true, true, true); //the end properties are bool for is wallkable, transparent & explored
            }
            foreach (Cell cell in _map.GetCellsInRows(0, _height - 1))
            {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }
            foreach (Cell cell in _map.GetCellsInColumns(0, _width - 1))
            {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }

            return _map;
        }
    }

}
