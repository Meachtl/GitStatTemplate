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

            List<(string, int, int, int, int)> tuplesList = new List<(string, int, int, int, int)>();

            //var developers = _dbContext.Developers;

            //int commits = 0;
            //int fileChanges = 0;
            //int insertions = 0;
            //int deletions = 0;

            //foreach (Developer developer in developers)
            //{
            //    //commits = developer.Commits.Count();


            //    fileChanges = developer.Commits.Sum(x => x.FilesChanges);

            //    insertions = developer.Commits.Sum(x => x.Insertions);

            //    deletions = developer.Commits.Sum(x => x.Deletions);

                //tuplesList.Add(/*Tuple.Create<Developer, int, int, int, int>*/(developer, commits, fileChanges, insertions, deletions));
            //}

            /*SELECT
	            Developers.Name
	            ,COUNT(*) AS 'Commits'
	            ,SUM(Commits.FilesChanges)
	            ,SUM(Commits.Insertions)
	            ,SUM(Commits.Deletions)
              FROM Commits
              JOIN Developers ON Developers.Id = Commits.DeveloperId
              GROUP BY Developers.Name
              ORDER BY 'Commits' DESC;*/

            //var result = from dev in _dbContext.Developers
            //             join com in _dbContext.Commits on
            //             dev.Id equals com.Id
            //             group dev by new
            //             {
            //                 Developer = dev.Name,
            //                 Commits = dev.Commits,
            //                 com.FilesChanges,
            //                 com.Insertions,
            //                 com.Deletions
            //             }
            //           into g
            //             select new
            //             {
            //                 Developer = g.Key.Developer,
            //                 Commits = g.Key.Commits.Count(),
            //                 FileChanged = g.Sum(g.Key.FilesChanges),

            //             }

            //var query = from dev in _dbContext.Developers
            //            join com in _dbContext.Commits on dev.Id equals com.DeveloperId
            //            group dev.Name by new
            //            {
            //                dev.Name,
            //                com.FilesChanges,
            //                com.Insertions,
            //                com.Deletions
            //            } into g
            //            select new
            //            {
            //                g.Key.Name,
            //                g.Sum(x => x.)
            //                g.Key.FilesChanges,
            //                g.Key.Insertions,
            //                g.Key.Deletions
            //            };


            var query = _dbContext.Developers.Join(// outer sequence
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

            var result = query.GroupBy(x => x.Developer)
                .Select(g => new
                {
                    Developer=g.Key,
                    Commits = g.Count(),
                    FileChanges = g.Sum(x => x.FileChanged),
                    Insertions = g.Sum(x => x.Insertions),
                    Deletions = g.Sum(x => x.Deletions)
                });

            var finalResult = result.OrderByDescending(x => x.Commits);

            //tuplesList.Sort();

            foreach (var item in finalResult)
            {
                tuplesList.Add(/*Tuple.Create<Developer, int, int, int, int>*/(item.Developer, item.Commits, item.FileChanges, item.Insertions, item.Deletions));
            }

            //tuplesList.Sort();


            //return tuplesList.ToArray();
            return tuplesList.ToArray();
            //var query=_dbContext
            //    .Developers
            //    .Include(com => com.Commits)
            //    .Select(dev => new ValueTuple.Create
            //    (
            //        dev,
            //        dev.Commits.Count(),
            //        dev.))


            //var query=_dbContext
            //    .Developers
            //    Include(com => com.Commits)
            //    .Select(developer => ValueTuple.Create
            //    (
            //        developer,
            //        developer.Commits
            //        .Count(),
                    
            //        ))

            


            //var query = _dbContext
            //    .Developers
            //    .Include(c => c.Commits)
            //    .GroupBy(n => n.Name)
            //    .Select(x => new QueryDTO
            //    {
            //        Commits = x.Count(),
            //        FilesChanged = x
            //    })




        }

        //public List<QueryDTO> GetStatisticOfAllDevelopers()
        //{
        //    var query=_dbContext
        //        .Developers
        //        .Include(c => c.Commits)
        //        .GroupBy(n => n.Name)
        //        .Select(x => new QueryDTO
        //        {
        //            Commits = x.Count(),
        //            FilesChanged= x
        //        })




        //}
    }
}