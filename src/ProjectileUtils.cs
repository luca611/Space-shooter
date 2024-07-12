using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
namespace Space_Shooter;

public abstract class ProjectileUtils
{
    public static List<Projectile> UpdateProjectiles(List<Projectile> projectiles)
    {
        for (var i = 0; i < projectiles.Count; i++)
        {
            projectiles[i].Update();
            if (projectiles[i].IsOutOfBounds()) projectiles.RemoveAt(i);
        }
        return projectiles;
    }
    
    public static void DrawProjectiles(List<Projectile> projectiles)
    {
        foreach (var projectile in projectiles) projectile.Draw();
    }
}