using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using GitStat.Core.Entities;
using Utils;

namespace GitStat.ImportConsole
{
    public class ImportController
    {
        const string Filename = "commits.txt";
        static Dictionary<string, Developer> _developers = new Dictionary<string, Developer>();

        /// <summary>
        /// Liefert die Messwerte mit den dazugehörigen Sensoren
        /// </summary>
        public static Commit[] ReadFromCsv()
        {
            string path = MyFile.GetFullNameInApplicationTree(Filename);
            string text = File.ReadAllText(path, Encoding.Default);

            List<Commit> commits = new List<Commit>();
            List<Commit> returnedCommits = new List<Commit>();

            //Split read in text into blocks
            string[] block = text.Split("\n\r\n");

            for (int i = 0; i < block.Length; i++)
            {
                //get each line out of the block
                string lineOfBlock = block[i];
                returnedCommits = GetCommitOfEachBlock(lineOfBlock);

                foreach (Commit currentCommit in returnedCommits)
                {
                    commits.Add(currentCommit);
                }
            }
            return commits.ToArray();
        }

        private static List<Commit> GetCommitOfEachBlock(string lineOfBlock)
        {
            List<Commit> commits = new List<Commit>();
            Commit currentCommit = new Commit();
            //const int of first line
            const int HASH = 0;
            const int DATE = 2;

            //cons tint of last line
            const int FILESCHANGED = 0;
            const int INSERTIONS = 1;
            const int DELETIONS = 2;
            
            ////get each line out of the block

            //split lineOfBlock into it rows
            string[] firstLineBlock = lineOfBlock.Split("\n");

            //get firstline and lastline out of the block
            string firstLine = firstLineBlock.First();
            string lastLine = firstLineBlock.Last();
            /*
            filechanges	7868
            insert	646134
            delet	25959
            */

            string tests = firstLineBlock.First();
            string[] test1 = tests.Split(',');

            //check for multiple commit rows in block
            var commitLines = firstLineBlock.TakeWhile(x => !x.StartsWith(' '));

            string[] test = commitLines.ToArray();

            int length = test.Count();

            string[] splittedFirstLine;
            string[] splittedLastLine;

            Developer developer;

            for (int i = 0; i < length; i++)
            {
                //splitt each line
                splittedFirstLine = test[i].Split(',');

                //multiple rows in header
                if (i < length - 1)
                {
                    developer = GetDeveloper(splittedFirstLine);

                    DateTime.TryParse(splittedFirstLine[DATE], out DateTime timestamp);
                    string hash = splittedFirstLine[HASH];

                    string message = "";
                    for (int j = 3; j < splittedFirstLine.Length; j++)
                    {
                        if (j == 3)
                        {
                            message += splittedFirstLine[j];
                        }
                        else
                        {
                            message += ", " + splittedFirstLine[j];
                        }
                    }

                    int filesChanges = 0;
                    int insertions = 0;
                    int deletions = 0;

                    currentCommit = new Commit
                    {
                        Developer = developer,
                        Date = timestamp,
                        HashCode = hash,
                        Message = message,
                        FilesChanges = filesChanges,
                        Insertions = insertions,
                        Deletions = deletions
                    };

                    commits.Add(currentCommit);
                }
                //last row or single header
                else
                {
                    splittedLastLine = lastLine.Split(',');
                    developer = GetDeveloper(splittedFirstLine);

                    DateTime.TryParse(splittedFirstLine[DATE], out DateTime timestamp);
                    string hash = splittedFirstLine[HASH];

                    string message = "";
                    for (int j = 3; j < splittedFirstLine.Length; j++)
                    {
                        if (j == 3)
                        {
                            message += splittedFirstLine[j];
                        }
                        else
                        {
                            message += ", " + splittedFirstLine[j];
                        }
                    }

                    int filesChanges = GetNumberOutOfString(splittedLastLine[FILESCHANGED]);

                    int insertions = 0;
                    int deletions = 0;

                    if (splittedLastLine[INSERTIONS].Contains('+'))
                    {
                        insertions = GetNumberOutOfString(splittedLastLine[INSERTIONS]);
                        if (splittedLastLine.Length == 3 && splittedLastLine[DELETIONS].Contains('-'))
                        {
                            deletions = GetNumberOutOfString(splittedLastLine[DELETIONS]);
                        }
                    }
                    else if (splittedLastLine[INSERTIONS].Contains('-'))
                    {
                        deletions = GetNumberOutOfString(splittedLastLine[INSERTIONS]);
                    }

                    currentCommit = new Commit
                    {
                        Developer = developer,
                        Date = timestamp,
                        HashCode = hash,
                        Message = message,
                        FilesChanges = filesChanges,
                        Insertions = insertions,
                        Deletions = deletions
                    };
                    commits.Add(currentCommit);
                }
            }
            return commits;
        }

        private static Developer GetDeveloper(string[] splittedFirstLine)
        {
            Developer developer;
            const int DEVELOPER = 1;

            if (_developers.ContainsKey(splittedFirstLine[DEVELOPER]))
            {
                _developers.TryGetValue(splittedFirstLine[DEVELOPER], out developer);
            }
            else
            {
                developer = new Developer { Name = splittedFirstLine[DEVELOPER] };
                _developers.Add(splittedFirstLine[DEVELOPER], developer);
            }

            return developer;
        }

        private static int GetNumberOutOfString(string line)
        {
            var query = line
                .SkipWhile(x => x < '0' || x > '9');
            //var query = line
            //    .TakeWhile(x => x >= '0' && x <= '9');

            var number = query
                .TakeWhile(x => x >= '0' && x <= '9').Reverse().ToArray();

            int result = -1;

            for (int i = 0; i < number.Count(); i++)
            {
                if (i == 0)
                {
                    result = 0;
                }
                result += (number[i]-48) * (int)Math.Pow(10, i);
            }

            return result;
        }
    }
}
