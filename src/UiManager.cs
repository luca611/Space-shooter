using Raylib_cs;

namespace Space_Shooter;

public abstract class UiManager
{
    private static readonly Texture2D LifeTexture = Raylib.LoadTexture("./assets/lifes.png");
    private static readonly Texture2D EnemyKillTexture = Raylib.LoadTexture("./assets/enemy.png");
    private static readonly Texture2D Background = Raylib.LoadTexture("./assets/bg.png");
    private static readonly Texture2D GameOverTexture = Raylib.LoadTexture("./assets/gameOver.png");

    public static void DrawLives(int lives)
    {
        for (var i = 0; i < lives; i++)
        {
            Raylib.DrawTextureEx(LifeTexture, new System.Numerics.Vector2(10 + i * 30, 10), 0.0f, 2.0f, Color.White);
        }
    }

    public static void DrawKills(int kills)
    {
        var startX = 10;
        var startY = 50;
        var iconSize = 16;
        var scaleFactor = 2;

        var sourceRectIcon = new Rectangle(0, 0, iconSize, iconSize);
        var destRectIcon = new Rectangle(startX, startY, iconSize * scaleFactor, iconSize * scaleFactor);
        Raylib.DrawTexturePro(EnemyKillTexture, sourceRectIcon, destRectIcon, new System.Numerics.Vector2(0, 0), 0f,
            Color.White);

        var textX = startX + iconSize * scaleFactor + 5;
        var textY = startY + (iconSize * scaleFactor / 2) - 10;
        Raylib.DrawText(kills.ToString(), textX, textY, 20, Color.White);
    }

    public static void DrawBackground()
    {
        double currentTime = Raylib.GetTime();
        int backgroundX = ((int)currentTime % 2) * 64;

        var destRect = new Rectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        var sourceRect = new Rectangle(backgroundX, 0, 64, 64);

        Raylib.DrawTexturePro(Background, sourceRect, destRect, new System.Numerics.Vector2(0, 0), 0f, Color.White);
    }
}