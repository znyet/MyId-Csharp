using Funcular.IdGenerators.Base36;

namespace MyIdServer
{
    public class Base36Id
    {
        public static readonly Base36IdGenerator Base16 = new Base36IdGenerator(10, 2, 3);
        public static readonly Base36IdGenerator Base20 = new Base36IdGenerator();
        public static readonly Base36IdGenerator Base25 = new Base36IdGenerator(12, 6, 7);

    }
}
