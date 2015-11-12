using System.Collections.Generic;

namespace Reversal
{
    public abstract class Direction
    {
        public static IEnumerable<Direction> All()
        {
            yield return new North();
            yield return new NorthEast();
            yield return new East();
            yield return new SouthEast();
            yield return new South();
            yield return new SouthWest();
            yield return new West();
            yield return new NorthWest();
        } 

        public abstract Position AwayFrom(Position position);

        public sealed class North : Direction
        {
            public override Position AwayFrom(Position position)
            {
                return new Position(position.X, position.Y + 1);
            }
        }

        public sealed class NorthEast : Direction
        {
            public override Position AwayFrom(Position position)
            {
                return new Position(position.X + 1, position.Y + 1);
            }
        }

        public sealed class East : Direction
        {
            public override Position AwayFrom(Position position)
            {
                return new Position(position.X + 1, position.Y);
            }
        }

        public sealed class SouthEast : Direction
        {
            public override Position AwayFrom(Position position)
            {
                return new Position(position.X + 1, position.Y - 1);
            }
        }

        public sealed class South : Direction
        {
            public override Position AwayFrom(Position position)
            {
                return new Position(position.X, position.Y - 1);
            }
        }

        public sealed class SouthWest : Direction
        {
            public override Position AwayFrom(Position position)
            {
                return new Position(position.X - 1, position.Y - 1);
            }
        }

        public sealed class West : Direction
        {
            public override Position AwayFrom(Position position)
            {
                return new Position(position.X - 1, position.Y);
            }
        }

        public sealed class NorthWest : Direction
        {
            public override Position AwayFrom(Position position)
            {
                return new Position(position.X - 1, position.Y + 1);
            }
        }
    }
}