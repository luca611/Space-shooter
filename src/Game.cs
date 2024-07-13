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
            Player player;
            EnemySystem enemySystem;
            try
            {
                 player = new Player(ScreenWidth / 2, ScreenHeight / 2, 5,100, 20, 20);
                 enemySystem = new EnemySystem(2, 5, 1);
            }
            catch (ArgumentException e)
            {
                CloseWindow();
                throw new ArgumentException(e.Message);
            } 
            
            while (!WindowShouldClose())
            {
                player.Update();
                enemySystem.Update();
                BeginDrawing();
                ClearBackground(Color.Blue);
                enemySystem.Draw();
                player.Draw();
                EndDrawing();
            }

            CloseWindow();

            return 0;
        }
    }
}