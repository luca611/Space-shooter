using Raylib_cs;

namespace Space_Shooter;

/// <summary>
/// Manages the UI elements of the game, including drawing lives, kills, background, and game over screen.
/// </summary>
public abstract class UiManager
{
    /// <summary>
    /// The texture used for displaying lives.
    /// </summary>
    private static readonly Texture2D LifeTexture = Raylib.LoadTexture("./assets/lifes.png");

    /// <summary>
    /// The texture used for displaying enemy kills.
    /// </summary>
    private static readonly Texture2D EnemyKillTexture = Raylib.LoadTexture("./assets/enemy.png");

    /// <summary>
    /// The background texture.
    /// </summary>
    private static readonly Texture2D Background = Raylib.LoadTexture("./assets/bg.png");

    /// <summary>
    /// The texture displayed when the game is over.
    /// </summary>
    private static readonly Texture2D GameOverTexture = Raylib.LoadTexture("./assets/gameOver.png");

    /// <summary>
    /// Draws the player's remaining lives on the screen.
    /// </summary>
    /// <param name="lives">The number of lives remaining.</param>
    public static void DrawLives(int lives)
    {
        for (var i = 0; i < lives; i++)
        {
            Raylib.DrawTextureEx(LifeTexture, new System.Numerics.Vector2(10 + i * 30, 10), 0.0f, 2.0f, Color.White);
        }
    }

    /// <summary>
    /// Draws the number of enemy kills on the screen.
    /// </summary>
    /// <param name="kills">The number of kills.</param>
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

    /// <summary>
    /// Draws the background texture on the screen.
    /// </summary>
    public static void DrawBackground()
    {
        double currentTime = Raylib.GetTime();
        int backgroundX = ((int)currentTime % 2) * 64;

        var destRect = new Rectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        var sourceRect = new Rectangle(backgroundX, 0, 64, 64);

        Raylib.DrawTexturePro(Background, sourceRect, destRect, new System.Numerics.Vector2(0, 0), 0f, Color.White);
    }

    /// <summary>
    /// Draws the game over screen with a message prompting the player to press Enter to play again.
    /// </summary>
    public static void DrawGameOver()
    {
        var newWidth = Raylib.GetScreenWidth() / 2;
        var newHeight = (int)(Raylib.GetScreenHeight() / 3);
        var centerX = (Raylib.GetScreenWidth() - newWidth) / 2;
        var centerY = (Raylib.GetScreenHeight() - newHeight) / 2;

        var destRect = new Rectangle(centerX, centerY, newWidth, newHeight);
        var sourceRect = new Rectangle(0, 0, GameOverTexture.Width, GameOverTexture.Height);
        Raylib.DrawTexturePro(GameOverTexture, sourceRect, destRect, new System.Numerics.Vector2(0, 0), 0f, Color.White);

        var text = "Press Enter to play again";
        var textSize = Raylib.MeasureText(text, 20);
        var textX = (Raylib.GetScreenWidth() - textSize) / 2;
        var textY = centerY + newHeight + 20;
        Raylib.DrawText(text, textX, textY, 20, Color.White);
    }
}
