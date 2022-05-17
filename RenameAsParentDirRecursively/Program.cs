using System;

namespace RenameAsParentDirRecursively
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
#if DEBUG
            Rename(@"c:\Test\Named\", "");
#else
            Rename(@".", "");
#endif
            Console.ReadKey();
        }

        /*

          - root
          - root\sub
          - root\sub\file1
          - root\folder
          - root\folder\file2
          - root\folder\sub2
          - root\folder\sub2\file2

            rename only files
          - root
          - root\sub
          - root\sub\root sub file1
          - root\folder
          - root\folder\root folder file2
          - root\folder\sub2
          - root\folder\sub2\root folder sub2 file2

            rename folders and files
          - root
          - root\sub
          - root\root sub\root sub file1
          - root\folder
          - root\root folder\root folder file2
          - root\folder\sub2
          - root\root folder\root folder sub2\root folder sub2 file2
             
        */

        public static int recurse = 0;

        public static void Rename(string path, string parent)
        {
            var dirs = System.IO.Directory.GetDirectories(path);
            foreach (var dir in dirs)
            {
                var d = System.IO.Path.GetFileName(dir);
                recurse++;
                //Rename(dir, GetNonDuplicatePrefix(d, parent));
                Rename(dir, string.IsNullOrWhiteSpace(parent) ? d : parent);
                recurse--;
            }

            if (!string.IsNullOrWhiteSpace(parent))
            {
                //Console.WriteLine("dir: " + path);
                //Console.WriteLine("Recurse: " + recurse + " in " + parent);

                var dir = System.IO.Path.GetFileName(path);
                var files = System.IO.Directory.GetFiles(path);
                foreach (var file in files)
                {
                    var f = System.IO.Path.GetFileNameWithoutExtension(file);
                    //Console.WriteLine("file name: " + f);

                    var e = System.IO.Path.GetExtension(file);
                    //Console.WriteLine("file extension: " + e);

                    var cleanFile = f;
                    cleanFile = CleanFileName(cleanFile, parent);
                    cleanFile = CleanFileName(cleanFile.ToLower(), parent.ToLower());
                    dir = CleanFileName(dir, parent);
                    dir = CleanFileName(dir, parent.ToLower());
                    var newFileName = GetNonDuplicatePrefix(cleanFile, dir);
                    newFileName = GetNonDuplicatePrefix(newFileName, parent);

                    //Console.WriteLine("prepend: " + newFileName);

                    var newFullFileName = System.IO.Path.Combine(path, newFileName + e);
                    //Console.WriteLine("renaming " + file + " to " + newFullFileName);
                    for (int i = 1; System.IO.File.Exists(newFullFileName); i++)
                        newFullFileName = System.IO.Path.Combine(path, newFileName + i + e);

#if DEBUG
                    newFullFileName = System.IO.Path.Combine(path, f + ".test");
                    //create empty files at destination
                    if (!System.IO.File.Exists(newFullFileName))
                    {
                        using (System.IO.File.Create(newFullFileName))
                        {
                            // just create the file
                        }
                    }
                    else
                    {
                        Console.WriteLine("file already exists: " + newFullFileName);
                    }
#else
                    System.IO.File.Move(file, newFullFileName);
#endif

                    Console.WriteLine("1 " + file);
                    Console.WriteLine("2 " + path);
                    Console.WriteLine("3 " + newFileName);
                    Console.WriteLine(" - ");
                }

                //Console.WriteLine(" ---------------- ");
            }
        }

        private static string CleanFileName(string cleanFile, string parent)
        {
            cleanFile = cleanFile.Replace(parent, "");
            cleanFile = cleanFile.Replace(".", " ");
            cleanFile = cleanFile.Replace("!", " ");
            cleanFile = cleanFile.Replace("&", " ");
            cleanFile = cleanFile.Replace("'", " ");
            cleanFile = cleanFile.Replace("(", " ");
            cleanFile = cleanFile.Replace(")", " ");
            cleanFile = cleanFile.Replace("[", " ");
            cleanFile = cleanFile.Replace("]", " ");
            cleanFile = cleanFile.Replace(",", " ");
            cleanFile = cleanFile.Replace("-", " ");
            cleanFile = cleanFile.Replace("_", " ");
            cleanFile = cleanFile.Replace("  ", " ");
            cleanFile = cleanFile.Replace("  ", " ");
            cleanFile = cleanFile.Replace("  ", " ");
            cleanFile = cleanFile.Replace("  ", " ");
            cleanFile = cleanFile.Replace("  ", " ");
            cleanFile = cleanFile.Replace("  ", " ");
            cleanFile = cleanFile.Replace(parent, "");
            cleanFile = cleanFile.Replace("  ", " ");
            cleanFile = cleanFile.Replace("  ", " ");
            cleanFile = cleanFile.Replace("  ", " ");
            cleanFile = cleanFile.Replace("  ", " ");
            cleanFile = cleanFile.Replace("  ", " ");
            cleanFile = cleanFile.Replace("  ", " ");
            return cleanFile.Trim();
        }

        private static string GetNonDuplicatePrefix(string name, string prefix)
        {
            name = name.ToLower().Trim();
            prefix = prefix.ToLower().Trim();
            if (!string.IsNullOrWhiteSpace(prefix)) name = name.Replace(prefix, "").Trim();
            if (name.StartsWith(prefix))
            {
                return name.Trim();
            }
            else
            {
                return (prefix + " " + name).Trim();
            }
        }
    }
}