using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDemo1.DTO;
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
        public static Size SCREEN_SIZE = new Size(1024, 700);
        public static Size MENU_PANEL_SIZE = new Size(1024, 150);
        public static Size CURRENT_CELL_SIZE = new Size(97, 49);   // get in map initialization
        public static Vector2 SPEED_SCROLL = new Vector2(20, 20);
        public static Size MAP_SIZE_IN_CELL = new Size(80, 80);
        public static int DELAY_TIME = 70;   // delay time for move sprite
        public static Size CURSOR_SIZE = new Size(32, 32);

        public static string PATH_TO_RHOMBUS_MAP_IMAGE = "MapCells\\RhombusMap\\";
        public static string PATH_TO_SQUARE_MAP_IMAGE = "MapCells\\SquareMap\\";
        public static string PATH_TO_STRUCTURE_XML = "Specification\\Structure\\";
        public static string PATH_TO_UNIT_XML = "Specification\\Unit\\";
        public static string PATH_TO_TERRAIN_XML = "Specification\\Terrain\\";
        public static string PATH_TO_MENU_IMAGE = "MenuPanel\\";
        public static string PATH_TO_CURSOR_IMAGE = "Cursor\\";
        public static string PATH_TO_FONT = "Font\\Courier New";
        public static string PATH_TO_MAP = "Map\\";
        public static string PATH_SAVE_FILE = "Save//";

        public static string CURSOR = "cursor1";
        public static string MENU_PANEL = "menuPanel2";
        public static Vector2 START_COORDINATE = new Vector2(2000, 2000);
        public static Vector2 CURRENT_COORDINATE = START_COORDINATE;

        public static string SPEC_MAP_CELL_GROUP_PATH = "Specification\\MapCellGroup.xml";
        public static String IMAGE_SPECIFICATION = @"Specification\MapCellData\\Details.xml";
        public static String IMAGE_PATH = "Preview\\";
        public static int[,] OccupiedMatrix;
    }
}
