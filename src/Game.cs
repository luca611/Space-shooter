using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Space_Shooter
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
            Player player = new (ScreenWidth / 2, ScreenHeight / 2, 5,100);
            while (!WindowShouldClose())
            {
                player.Update();
                BeginDrawing();
                ClearBackground(Color.Blue);
                player.Draw();
                EndDrawing();
            }

            CloseWindow();

            return 0;
        }
    }
}