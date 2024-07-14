using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
namespace Space_Shooter;

public class PowerUpSystem
{
    private static List<PowerUp> _powerUps;
    public PowerUpSystem(Player player)
    {
        _powerUps = [];
    }
    
    public void Update()
    {
        for (var i = 0; i < _powerUps.Count; i++)
        {
            _powerUps[i].Update();
            if (_powerUps[i].IsOutOfBounds()||_powerUps[i].IsUsed()) _powerUps.RemoveAt(i);
        }
    }
    
    public void Draw()
    {
        foreach (var powerUp in _powerUps) powerUp.Draw();
    }
    
    public static void GeneratePowerUp(Vector2 position, Player _player)
    {
        var random = new Random();
        var type = random.Next(0, 5);
        var powerUp = new PowerUp(position.X, position.Y, type, _player);
        _powerUps.Add(powerUp);
    }
}