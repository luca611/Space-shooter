namespace Space_Shooter;

/// <summary>
/// Provides utility methods for handling projectiles in the game.
/// </summary>
public abstract class ProjectileUtils
{
    /// <summary>
    /// Updates the position and state of each projectile in the list.
    /// Removes projectiles that are out of bounds.
    /// </summary>
    /// <param name="projectiles">The list of projectiles to update.</param>
    /// <returns>The updated list of projectiles.</returns>
    public static List<Projectile> UpdateProjectiles(List<Projectile> projectiles)
    {
        for (var i = 0; i < projectiles.Count; i++)
        {
            projectiles[i].Update();
            if (projectiles[i].IsOutOfBounds()) projectiles.RemoveAt(i);
        }
        return projectiles;
    }
    
    /// <summary>
    /// Draws each projectile in the list.
    /// </summary>
    /// <param name="projectiles">The list of projectiles to draw.</param>
    public static void DrawProjectiles(List<Projectile> projectiles)
    {
        foreach (var projectile in projectiles) projectile.Draw();
    }
}