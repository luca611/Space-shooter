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
                 player = new Player(ScreenWidth / 2, ScreenHeight / 2, 5,100, 48, 48, 0.5, 10, LoadTexture("./assets/ship.png"));
                 enemySystem = new EnemySystem(2, 5, 1, player);
            }
            catch (ArgumentException e)
            {
                CloseWindow();
                throw new ArgumentException(e.Message);
            } 
            
            var powerUpSystem = new PowerUpSystem(player);
            while (!WindowShouldClose())
            {
                if (!player.IsDead())
                {
                    powerUpSystem.Update();
                    player.Update();
                    enemySystem.Update();
                    BeginDrawing();
                    ClearBackground(Color.Blue);
                    UiManager.DrawBackground();
                    powerUpSystem.Draw();
                    enemySystem.Draw();
                    player.Draw();
                    EndDrawing();
                }
                else
                {
                    BeginDrawing();
                    ClearBackground(Color.Black);
                    UiManager.DrawGameOver();
                    EndDrawing();
                    if (IsKeyPressed(KeyboardKey.Enter))
                    {
                        player.Reset();
                        enemySystem.Reset();
                    }
                    
                }
            }

            CloseWindow();

            return 0;
        }
    }
}