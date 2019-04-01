using System;
using System.Collections.Generic;
using System.Linq;
using GitStat.Core.Contracts;
using GitStat.Core.DataTransferObjects;
using GitStat.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GitStat.Persistence
{
    public class DeveloperRepository : IDeveloperRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DeveloperRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        

        public (string, int, int, int, int)[] GetStatisticOfAllDevelopers()
        {
            //create new list of tupel
            List<(string, int, int, int, int)> tuplesList = new List<(string, int, int, int, int)>();

            //get innerJoin of Developers and Commits
            var innerJoin = _dbContext.Developers.Join(// outer sequence
                _dbContext.Commits, //inner sequence
                dev => dev.Id, //outer Key Selector
                com => com.DeveloperId, //inner Key Selector
                (dev, com) => new //result selector
                {
                    Developer = dev.Name,
                    FileChanged=com.FilesChanges,
                    Insertions=com.Insertions,
                    Deletions=com.Deletions
                });

            //group innerJoin by Developer and calc results
            var result = innerJoin.GroupBy(x => x.Developer)
                .Select(g => new
                {
                    Developer=g.Key,
                    Commits = g.Count(),
                    FileChanges = g.Sum(x => x.FileChanged),
                    Insertions = g.Sum(x => x.Insertions),
                    Deletions = g.Sum(x => x.Deletions)
                });

            //order result
            var finalResult = result.OrderByDescending(x => x.Commits);

            //add each result to tuple list
            foreach (var item in finalResult)
            {
                tuplesList.Add((item.Developer, item.Commits, item.FileChanges, item.Insertions, item.Deletions));
            }

            //return tupleList as array
            return tuplesList.ToArray();
        }
    }
}