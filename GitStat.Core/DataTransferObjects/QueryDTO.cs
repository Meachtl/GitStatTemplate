using GitStat.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GitStat.Core.DataTransferObjects
{
    class QueryDTO
    {
        public Developer Developer { get; set; }
        public DateTime DateTime { get; set; }
        public int FilesChanged { get; set; }
        public int Insertions { get; set; }
        public int Deletions { get; set; }
    }
}
