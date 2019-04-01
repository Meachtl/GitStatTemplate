using System.Collections.Generic;
using GitStat.Core.DataTransferObjects;
using GitStat.Core.Entities;

namespace GitStat.Core.Contracts
{
    public interface IDeveloperRepository
    {
         (string, int, int, int, int)[]GetStatisticOfAllDevelopers();
    }
}
