using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GitStat.Core.DataTransferObjects;
using GitStat.Core.Entities;

namespace GitStat.Core.Contracts
{
    public interface ICommitRepository
    {
        void AddRange(Commit[] commits);
        List<QueryDTO> GetCommitsOfLastFourWeeks(DateTime to);
        QueryDTO GetCommitOfId(int id);
    }
}
