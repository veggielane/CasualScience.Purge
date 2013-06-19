using System;
using System.IO;
using System.Linq;
namespace CasualScience.Purge
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Purging: {0}", Directory.GetCurrentDirectory());
            var purged = new Purge(Directory.GetCurrentDirectory()).Run();
            Console.WriteLine("Purged: {0} Files", purged.Count());
        }
    }
}
