using System;

namespace SpopadTitanov
{
#if WINDOWS || XBOX
    static class Program1
    {
        /// <summary>
        /// Vstopna to�ka za XNA igro
        /// </summary>
        static void Main(string[] args)
        {
            using (Game game = new Game())
            {
                game.Run();
            }
        }
    }
#endif
}

