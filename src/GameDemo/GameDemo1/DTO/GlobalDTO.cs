using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDemo1.DTO;
using System.Drawing;
using Microsoft.Xna.Framework;
using GameDemo1.Data;

namespace GameDemo1.DTO
{
    public class GlobalDTO
    {
        public static MainGame GAME;
        public static StructureDataReader STRUCTURE_DATA_READER = new StructureDataReader();
        public static UnitDataReader UNIT_DATA_READER = new UnitDataReader();
        
        /// <summary>
        /// Size of a map cell in pixel.
        /// </summary>
        /// 
        public static Size SCREEN_SIZE = new Size(1000, 700);
        public static Size MENU_PANEL_BOTTOM_SIZE = new Size(1000, 170);
        public static Size MENU_PANEL_TOP_SIZE = new Size(1000, 35);
        public static Size CURRENT_CELL_SIZE = new Size(97, 49);   // get in map initialization
        public static Vector2 SPEED_SCROLL = new Vector2(15, 15);
        public static Size MAP_SIZE_IN_CELL = new Size(80, 80);
        public static Size CURSOR_SIZE = new Size(13, 16);
        public static Vector2 START_COORDINATE = new Vector2(2000, 2000);
        public static Vector2 CURRENT_COORDINATE = START_COORDINATE;

        public static string SPEC_RESOURCECENTER_PATH = "Specification\\Sprites\\ResourceCenter\\";
        public static string SPEC_STRUCTURE_PATH = "Specification\\Sprites\\Structure\\";
        public static string SPEC_TERRAIN_PATH = "Specification\\Sprites\\Terrain\\";
        public static string SPEC_UNIT_PATH = "Specification\\Sprites\\Unit\\";
        public static string SPEC_MAP_PATH = "Specification\\Map\\";

        public static string RES_CURSOR_PATH = "Cursor\\";
        public static string RES_FONT_PATH = "Font\\Mangal";
        
        public static string RES_MINI_MAP_PATH = "Map\\MiniMap\\";
        public static string RES_RHOMBUS_MAP_PATH = "Map\\RhombusMap\\";
        public static string RES_SQUARE_MAP_PATH = "Map\\SquareMap\\";

        public static string RES_SELECTION_PATH = "Images\\Selection\\Red";
        public static string PATH_TO_HEALTHIMAGE = "Images\\Health\\Red";
        public static string PATH_TO_MENU_IMAGE = "Images\\MenuPanel\\";
        
        public static string MINIMAP = "MiniMap1";
        public static string CURSOR = "cursor2";
        public static string MENU_PANEL_BOTTOM = "menuPanel2";
        public static string MENU_PANEL_TOP = "menuPanel2";
    }
}
