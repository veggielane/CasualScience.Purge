using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CasualScience.Purge
{
    public class Purge
    {
        private readonly string _dir;

        public Purge(string dir)
        {
            _dir = dir;
        }

        public IEnumerable<string> Run()
        {
            return DeleteOldVersions()
                .Concat(DeleteMatch("*.bom.*"))
                .Concat(DeleteMatch("s2d*.sec.*"));
        }

        private IEnumerable<string> DeleteOldVersions()
        {
            var files = Directory.GetFiles(_dir);
            var groups = files.Where(IsCreoFile).OrderByDescending(f => f).GroupBy(f => GetActualFilename(Path.GetFileName(f)), (key, g) => new { File = key, Ext = Path.GetExtension(key), Files = g.ToList(), First = g.First() });
            foreach (var g in groups)
            {
                Trace.WriteLine(String.Format("Keeping: {0}", Path.GetFileName(g.First)));
                foreach (var file in g.Files.Skip(1))
                {
                    Trace.WriteLine("Deleting: " + Path.GetFileName(file));
                    File.Delete(file);
                    yield return file;
                }
                if (Path.GetFileName(g.First) != g.File)
                {
                    File.Move(g.First, Path.Combine(_dir, g.File));
                }
            }
        }

        private  IEnumerable<string> DeleteMatch(string match)
        {
            var files = Directory.GetFiles(_dir, match);
            foreach (var file in files)
            {
                File.Delete(file);
                yield return file;
            }
        }

        public bool IsCreoFile(string filename)
        {
            var extension = Path.GetExtension(GetActualFilename(filename));
            return extension != null && new[] {".prt", ".drw", ".asm"}.Any(extension.ToLower().Equals);
        }

        public string GetActualFilename(string filename)
        {
            int n;
            var extension = Path.GetExtension(filename);
            if (extension != null && int.TryParse(extension.Substring(1), out n))
            {
                return Path.GetFileNameWithoutExtension(filename);
            }
            return filename;
        }
    }
}