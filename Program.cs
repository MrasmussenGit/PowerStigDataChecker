using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ReadXML
{
    internal class Program
    {
        static List<object> FindDuplicates(ArrayList arrayList)
        {
            Dictionary<object, int> countDict = new Dictionary<object, int>();
            List<object> duplicates = new List<object>();

            foreach (var item in arrayList)
            {
                if (countDict.ContainsKey(item))
                {
                    countDict[item]++;
                }
                else
                {
                    countDict[item] = 1;
                }
            }

            foreach (var kvp in countDict)
            {
                if (kvp.Value > 1)
                {
                    for (int i = 0; i < kvp.Value - 1; i++)
                    {
                        duplicates.Add(kvp.Key);
                    }
                    
                }
            }

            return duplicates;
        }

        static void GetFileInfo(string FilePath)
        {
            XDocument xmlDoc = XDocument.Load(FilePath);
            XElement root = xmlDoc.Root;
            ArrayList rules = new ArrayList();

            void TraverseElement(XElement element, int level = 0)
            {

                foreach (XAttribute attribute in element.Attributes())
                {
                    if (attribute.Name.ToString().Trim().Equals("id"))
                    {
                        rules.Add(attribute.Value);
                    }
                }

                foreach (XElement child in element.Elements())
                {
                    TraverseElement(child, level + 1);
                }
            }

            TraverseElement(root);

            List<object> dupList = FindDuplicates(rules);
            List<object> usedList = new List<object>();
            int count = 0;
            Console.WriteLine($"{FilePath}");
            if(dupList.Count <= 0)
            {
                Console.WriteLine("\tNo Duplicates");        
            }
            foreach (var dup in dupList)
            {

                count = dupList.Count(item => item == dup);

                if (!usedList.Contains(dup))
                {
                    Console.WriteLine($"\tRule '{dup}' appears {count + 1} total times.");
                    usedList.Add(dup);
                }

            }

        }

        static void GetFolderInfo(string DirectoryPath)
        {
            if(!Directory.Exists(DirectoryPath))
            {
                Console.WriteLine($"Directory: {DirectoryPath} does not exist");
                return;
            }
            else
            {
                string[] files = Directory.GetFiles(DirectoryPath, "*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    GetFileInfo(file);
                }
            }
        }

        static void ListRules(string FilePath)
        {
            if (!File.Exists(FilePath)) { }

        }

        static void Main(string[] args)
        {
            var argDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            string filePath = String.Empty;
            string folderPath = String.Empty;

            for (int i = 0; i < args.Length; i += 2)
            {
                if (i + 1 < args.Length)
                {
                    argDictionary[args[i]] = args[i + 1];
                }
            }

            if (argDictionary.TryGetValue("--FilePath", out string filePathArg))
            {
                filePath = filePathArg;
            }

            if (argDictionary.TryGetValue("--FolderPath", out string folderPathArg))
            {
                folderPath = folderPathArg;
            }

            if (filePath.Length <= 0 && folderPath.Length <= 0)
            {
                Console.WriteLine("Enter a --FilePath or --FolderPath to continue.");
            }
            else
            {
                if (filePath.Length > 0)
                {
                    GetFileInfo(filePath);
                }
                else if (folderPath.Length > 0)
                {
                    GetFolderInfo(folderPath);
                }
                else
                {
                    Console.WriteLine("Command line invalid, Enter a --FilePath or --FolderPath with a value.";
                }
            }
        }
    }
}
