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
            #region MyRegion
            //string path = MyFile.GetFullNameInApplicationTree(Filename);

            //string[] lines = File.ReadAllLines(path, Encoding.Default);

            ////string[] firstLine;
            ////string[] lastLine;



            //int cntBlankLines = 0;

            //List<int> positionOfBlankLine = new List<int>();

            //for (int i = 0; i < lines.Length; i++)
            //{
            //    string blankLine = lines[i];

            //    if (string.IsNullOrEmpty(blankLine))
            //    {
            //        positionOfBlankLine.Add(i);
            //        cntBlankLines++;
            //    }
            //}

            //Dictionary<int, int> relevantLines = new Dictionary<int, int>();

            //List<string> testBothLines = new List<string>();

            //for (int i = 0; i < lines.Length; i++)
            //{
            //    string currentLine = lines[i];



            //    if (string.IsNullOrEmpty(currentLine) == false)
            //    {
            //        if (currentLine[0] != ' ')
            //        {
            //            int cnt = 0;
            //            string helpLine = lines[i];

            //            do
            //            {
            //                helpLine = lines[i + cnt];
            //                cnt++;
            //                //if (lines[i + cnt] != null)
            //                //{
            //                //    helpLine = lines[i + cnt]; 
            //                //}
            //                //else
            //                //{
            //                //    helpLine = null;
            //                //}
            //                //cnt++;

            //            } while (helpLine != null && !string.IsNullOrEmpty(helpLine));

            //            string endLine = lines[i + cnt - 1];

            //            //if (true)
            //            //{

            //            //}

            //            testBothLines.Add(currentLine + "===>" + endLine);


            //        } 
            //    }


            //    //if (string.IsNullOrEmpty(lines[i]))
            //    //{
            //    //    relevantLines.Add()
            //    //}



            //}


            //if (true)
            //{

            //}

            ////int[] startLines = new int[cntBlankLines];
            ////int[] endLines = new int[cntBlankLines];







            //return null; 
            #endregion


            string path = MyFile.GetFullNameInApplicationTree(Filename);
            string text = File.ReadAllText(path, Encoding.Default);

            //string[] lines = File.ReadAllLines(path, Encoding.Default);



            //var query = lines
            //    .SkipWhile(x => x == "");

            //var query = lines
            //    .TakeWhile(x => x != "")
            //    .Select(x => new string[]
            //    {

            //    });

            //Split read in text into blocks
            string[] block = text.Split("\n\r");


            //string[] block = text.Split("\r\n");

            //var query = lines
            //    .TakeWhile(x => x != "")
            //    .ToArray();

            //get each line out of the block
            string lineOfBlock = block[0];

            //split lineOfBlock into it rows
            string[] firstLineBlock = lineOfBlock.Split("\n");
            
            //get firstline and lastline out of the block
            string firstLine = firstLineBlock.First();
            string lastLine = firstLineBlock.Last();




            if (true)
            {

            }

            return null;
        }

    }
}
