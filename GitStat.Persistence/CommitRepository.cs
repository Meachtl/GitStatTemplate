using System;
using System.Collections.Generic;
using System.Linq;
using GitStat.Core.Contracts;
using GitStat.Core.DataTransferObjects;
using GitStat.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GitStat.Persistence
{
    public class CommitRepository : ICommitRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CommitRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddRange(Commit[] commits)
        {
            _dbContext.Commits.AddRange(commits);
        }

        public QueryDTO GetCommitOfId(int id)
        {
            var query = _dbContext
                .Commits
                .Include(d => d.Developer)
                .Where(i => i.Id == id)
                .Select(x => new QueryDTO
                {
                    Developer = x.Developer,
                    DateTime = x.Date,
                    FilesChanged = x.FilesChanges,
                    Insertions = x.Insertions,
                    Deletions = x.Deletions
                }).ToList()
                .Single();

            return query;

        }

        public List<QueryDTO> GetCommitsOfLastFourWeeks(DateTime to)
        {
            DateTime from = to.AddDays(-28);
            var query = _dbContext
                .Commits
                .Include(d => d.Developer)
                .Where(time => time.Date >= from && time.Date <= to)
                .OrderBy(d => d.Date)
                .Select(x => new QueryDTO
                {
                    Developer = x.Developer,
                    DateTime = x.Date,
                    FilesChanged = x.FilesChanges,
                    Insertions = x.Insertions,
                    Deletions = x.Deletions
                }).ToList();

            return query;
        }

        
    }
}