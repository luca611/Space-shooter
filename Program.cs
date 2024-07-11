using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Stuck_in_a_loop_challange
{
    public class GameWindow
    {
        //------screen settings---------
        public const int ScreenWidth = 800;
        public const int ScreenHeight = 450;
        
        public static int Main()
        {
            //-----main character--------
            InitWindow(ScreenWidth, ScreenHeight, "Space Shooter");
            SetTargetFPS(60); // ⚠ ️the game speed is based on this value ⚠  ️
            //--------actual game loop-------
            while (!WindowShouldClose())
            {
                BeginDrawing();
                EndDrawing();
            }

            CloseWindow();

            return 0;
        }
    }
}