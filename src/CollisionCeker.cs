using System.Numerics;

namespace Space_Shooter;

/// <summary>
/// Provides methods to check for collisions between game entities.
/// </summary>
internal abstract class CollisionCeker
{
    /// <summary>
    /// Checks if two rectangles, defined by their positions and sizes, are colliding.
    /// </summary>
    /// <param name="position1">The position of the first rectangle.</param>
    /// <param name="size1">The size of the first rectangle.</param>
    /// <param name="position2">The position of the second rectangle.</param>
    /// <param name="size2">The size of the second rectangle.</param>
    /// <returns>True if the rectangles are colliding, otherwise false.</returns>
    public static bool CheckCollision(Vector2 position1, Vector2 size1, Vector2 position2, Vector2 size2)
    {
        return position1.X < position2.X + size2.X &&
               position1.X + size1.X > position2.X &&
               position1.Y < position2.Y + size2.Y &&
               position1.Y + size1.Y > position2.Y;
    }
}