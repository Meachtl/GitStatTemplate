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

        /// <summary>
        /// Liefert die Messwerte mit den dazugehörigen Sensoren
        /// </summary>
        public static Commit[] ReadFromCsv()
        {
            string path = MyFile.GetFullNameInApplicationTree(Filename);
            string text = File.ReadAllText(path, Encoding.Default);

            List<Commit> commits = new List<Commit>();

            //Split read in text into blocks
            string[] block = text.Split("\n\r\n");

            Commit currentCommit; // = GetCommitOfEachBlock(block);

            for (int i = 0; i < block.Length; i++)
            {
                //get each line out of the block
                string lineOfBlock = block[i];
                currentCommit = GetCommitOfEachBlock(lineOfBlock);
                commits.Add(currentCommit);
            }


            ////get each line out of the block
            //string lineOfBlock = block[0];

            ////split lineOfBlock into it rows
            //string[] firstLineBlock = lineOfBlock.Split("\n");

            ////get firstline and lastline out of the block
            //string firstLine = firstLineBlock.First();
            //string lastLine = firstLineBlock.Last();






            return commits.ToArray();
        }

        private static Commit GetCommitOfEachBlock(string lineOfBlock)
        {
            Commit currentCommit;
            //const int of first line
            const int HASH = 0;
            const int DEVELOPER = 1;
            const int DATE = 2;
            const int MESSAGEONE = 3;
            const int MESSAGETWO = 4;
            const int MESSAGETHREE = 5;

            //cons tint of last line
            const int FILESCHANGED = 0;
            const int INSERTIONS = 1;
            const int DELETIONS = 2;


            
            ////get each line out of the block
            //string lineOfBlock = block[i];

            //split lineOfBlock into it rows
            string[] firstLineBlock = lineOfBlock.Split("\n");

            //get firstline and lastline out of the block
            string firstLine = firstLineBlock.First();
            string lastLine = firstLineBlock.Last();
            //if (lastLine == null)
            //{
            //    lastLine = firstLineBlock.Last();
            //    //last point
            //}


            string[] splittedFirstLine = firstLine.Split(',');
            string[] splittedLastLine = lastLine.Split(',');

            Developer developer = new Developer { Name = splittedFirstLine[DEVELOPER] };
            DateTime.TryParse(splittedFirstLine[DATE], out DateTime timestamp);
            string hash = splittedFirstLine[HASH];
            //string message = splittedFirstLine[MESSAGEONE];
            string message = "";
            for (int i = 3; i < splittedFirstLine.Length; i++)
            {
                if (i == 3)
                {
                    message += splittedFirstLine[i];
                }
                else
                {
                    message += ", " + splittedFirstLine[i]; 
                }
            }

            int filesChanges = GetNumberOutOfString(splittedLastLine[FILESCHANGED]);
            //int insertions = GetNumberOutOfString(splittedLastLine[INSERTIONS]);
            //int deletions = GetNumberOutOfString(splittedLastLine[DELETIONS]);

            int insertions = 0;
            int deletions = 0;

            if (splittedLastLine[INSERTIONS].Contains('+'))
            {
                insertions = GetNumberOutOfString(splittedLastLine[INSERTIONS]);
                if (splittedLastLine.Length > 3 && splittedLastLine[DELETIONS].Contains('-'))
                {
                    deletions = GetNumberOutOfString(splittedLastLine[DELETIONS]);
                }
            }
            else if (splittedLastLine[INSERTIONS].Contains('-'))
            {
                deletions = GetNumberOutOfString(splittedLastLine[INSERTIONS]);
            }
            //else if (splittedLastLine[DELETIONS].Contains('+'))
            //{
            //    insertions = GetNumberOutOfString(splittedLastLine[DELETIONS]);
            //}
            //else
            //{
            //    deletions= GetNumberOutOfString(splittedLastLine[INSERTIONS]);
            //}

            

            //currentCommit = new Commit(developer, timestamp, hash, message, filesChanged, insertions, deletions);

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

            ////get each line out of the block
            //string lineOfBlock = block[0];

            ////split lineOfBlock into it rows
            //string[] firstLineBlock = lineOfBlock.Split("\n");

            ////get firstline and lastline out of the block
            //string firstLine = firstLineBlock.First();
            //string lastLine = firstLineBlock.Last();





            return currentCommit;
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

            //int test = (int) number[1];

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
