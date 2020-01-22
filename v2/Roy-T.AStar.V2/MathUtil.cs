using System;

namespace Roy_T.AStar.V2
{
    public static class MathUtil
    {
        public static Duration ExpectedTime(float sX, float sY, float eX, float eY, Velocity velocity)
        {
            var distance = Distance(sX, sY, eX, eY);
            return Duration.FromSeconds(distance / velocity.MetersPerSecond);
        }

        public static float Distance(float sX, float sY, float eX, float eY)
        {
            var d0 = (eX - sX) * (eX - sX);
            var d1 = (eY - sY) * (eY - sY);

            return (float)Math.Sqrt(d0 + d1);
        }
    }
}
