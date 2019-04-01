using GitStat.Core.DataTransferObjects;
using GitStat.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GitStat.ImportConsole
{
    public class Query
    {
        public static QueryDTO GetCommitsOfLastFourWeeks()
        {
            using (UnitOfWork unit = new UnitOfWork())
            {
                

            }




            return null;
        }

        public static QueryDTO GetCommitWithId(int id)
        {
            using (UnitOfWork unit =new UnitOfWork())
            {
                //var query= unit.DeveloperRepository
                //    .Where




            }



            return null;
        }
    }
}
