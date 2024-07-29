using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Space_Shooter;

/// <summary>
/// Manages power-ups in the game, including their generation, updating, and drawing.
/// </summary>
public class PowerUpSystem
{
    /// <summary>
    /// List of active power-ups in the game.
    /// </summary>
    private static List<PowerUp> _powerUps;

    /// <summary>
    /// Initializes a new instance of the <see cref="PowerUpSystem"/> class.
    /// </summary>
    /// <param name="player">The player to whom the power-ups apply.</param>
    public PowerUpSystem(Player player)
    {
        _powerUps = new List<PowerUp>();
    }

    /// <summary>
    /// Updates the state of all active power-ups, removing any that are out of bounds or used.
    /// </summary>
    public void Update()
    {
        for (var i = 0; i < _powerUps.Count; i++)
        {
            _powerUps[i].Update();
            if (_powerUps[i].IsOutOfBounds() || _powerUps[i].IsUsed())
            {
                _powerUps.RemoveAt(i);
                i--; // Decrement index to account for the removed item.
            }
        }
    }

    /// <summary>
    /// Draws all active power-ups on the screen.
    /// </summary>
    public void Draw()
    {
        foreach (var powerUp in _powerUps)
        {
            powerUp.Draw();
        }
    }

    /// <summary>
    /// Generates a new power-up at the specified position and adds it to the list of active power-ups.
    /// </summary>
    /// <param name="position">The position where the power-up will be generated.</param>
    /// <param name="_player">The player to whom the power-up will apply.</param>
    public static void GeneratePowerUp(Vector2 position, Player _player)
    {
        var random = new Random();
        var type = random.Next(0, 4);
        var powerUp = new PowerUp(position.X, position.Y, type, _player);
        _powerUps.Add(powerUp);
    }
}