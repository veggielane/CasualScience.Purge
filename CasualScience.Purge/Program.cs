using System;
using System.IO;
using System.Linq;
namespace CasualScience.Purge
{
    class Program
    {
        static void Main(string[] args)
        {
            var dir = args.Length == 1 ? args[0] : Directory.GetCurrentDirectory();
            if(Directory.Exists(dir))
            {
                Console.WriteLine("Purging: {0}", dir);
                var purged = new Purge(dir).Run();
                Console.WriteLine("Purged: {0} Files", purged.Count());
            }else
            {
                Console.WriteLine("Directory '{0}' Doesn't Exist",dir);
            }
        }
    }
}
