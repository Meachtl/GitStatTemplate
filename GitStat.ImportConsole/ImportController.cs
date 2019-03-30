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

            //Split read in text into blocks
            string[] block = text.Split("\n\r");

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
