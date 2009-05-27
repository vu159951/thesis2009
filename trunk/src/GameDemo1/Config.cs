using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDemo1.HelperObjects;
using System.Drawing;
using Microsoft.Xna.Framework;

namespace GameDemo1
{
    public class Config
    {
        /// <summary>
        /// Size of a map cell in pixel.
        /// </summary>
        /// 
        public static Size SCREEN_SIZE = new Size(1000, 600);
        public static Size MENU_PANEL_SIZE = new Size(1000, 170);
        public static Size CURRENT_CELL_SIZE;   // get in map initialization
        public static Vector2 SPEED_SCROLL = new Vector2(20, 20);
        public static Size MAP_SIZE_IN_CELL = new Size(80, 80);
        public static Size CURSOR_SIZE = new Size(13, 16);

        public static string PATH_TO_RHOMBUS_MAP_IMAGE = "Background\\RhombusMap\\";
        public static string PATH_TO_SQUARE_MAP_IMAGE = "Background\\SquareMap\\";
        public static string PATH_TO_STRUCTURE_XML = "Specification\\Structure\\";
        public static string PATH_TO_UNIT_XML = "Specification\\Unit\\";
        public static string PATH_TO_TERRAIN_XML = "Specification\\Terrain\\";
        public static string PATH_TO_RESOURCECENTER_XML = "Specification\\ResourceCenter\\";
        public static string PATH_TO_MENU_IMAGE = "MenuPanel\\";
        public static string PATH_TO_CURSOR_IMAGE = "Cursor\\";
        public static string PATH_TO_FONT = "Font\\Mangal";
        public static string PATH_TO_MAP = "Map\\";
        public static string PATH_TO_BATTLEFIELD = "BattleField\\";
        public static string PATH_TO_SELECTEDIMAGE = "Select\\Red_Magic_Circle";

        public static string CURSOR = "cursor2";
        public static string MENU_PANEL = "menuPanel2";
        public static Vector2 START_COORDINATE = new Vector2(2000, 2000);
        public static Vector2 CURRENT_COORDINATE = START_COORDINATE;
    }
}
