using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GitStat.Core.Contracts;
using GitStat.Core.DataTransferObjects;
using GitStat.Core.Entities;
using GitStat.Persistence;

namespace GitStat.ImportConsole
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Import der Commits in die Datenbank");
            using (IUnitOfWork unitOfWorkImport = new UnitOfWork())
            {
                Console.WriteLine("Datenbank löschen");
                unitOfWorkImport.DeleteDatabase();
                Console.WriteLine("Datenbank migrieren");
                unitOfWorkImport.MigrateDatabase();
                Console.WriteLine("Commits werden von commits.txt eingelesen");
                var commits = ImportController.ReadFromCsv();
                if (commits.Length == 0)
                {
                    Console.WriteLine("!!! Es wurden keine Commits eingelesen");
                    return;
                }
                Console.WriteLine(
                    $"  Es wurden {commits.Count()} Commits eingelesen, werden in Datenbank gespeichert ...");
                unitOfWorkImport.CommitRepository.AddRange(commits);
                int countDevelopers = commits.GroupBy(c => c.Developer).Count();
                int savedRows = unitOfWorkImport.SaveChanges();
                Console.WriteLine(
                    $"{countDevelopers} Developers und {savedRows - countDevelopers} Commits wurden in Datenbank gespeichert!");
                Console.WriteLine();
                var csvCommits = commits.Select(c =>
                    $"{c.Developer.Name};{c.Date};{c.Message};{c.HashCode};{c.FilesChanges};{c.Insertions};{c.Deletions}");
                File.WriteAllLines("commits.csv", csvCommits, Encoding.UTF8);
            }
            Console.WriteLine("Datenbankabfragen");
            Console.WriteLine("=================");
            using (IUnitOfWork unitOfWork = new UnitOfWork())
            {
                

                DateTime to = new DateTime(2019, 03, 29);
                int id = 4;

                List<QueryDTO> lastFourWeeks = unitOfWork.CommitRepository.GetCommitsOfLastFourWeeks(to);

                QueryDTO commitOfIdFour = unitOfWork.CommitRepository.GetCommitOfId(id);

                (string, int,int,int,int)[] statisitcOfAllDevelopers = unitOfWork.DeveloperRepository.GetStatisticOfAllDevelopers();

                Console.WriteLine(
                    "Commits der letzten 4 Wochen\n" +
                    "----------------------------");
                PrintHeader();                
                PrintResultList(lastFourWeeks);
                Console.WriteLine();

                Console.WriteLine(
                    "Commit mit Id 4\n" +
                    "---------------");
                PrintResult(commitOfIdFour);
                Console.WriteLine();

                Console.WriteLine(
                    "Statistik der Commits der Developer\n" +
                    "-----------------------------------");
                PrintTupleResult(statisitcOfAllDevelopers);
                Console.WriteLine();


            }
            Console.Write("Beenden mit Eingabetaste ...");
            Console.ReadLine();
        }

        private static void PrintTupleResult((string, int, int, int, int)[] statisitcOfAllDevelopers)
        {
            Console.WriteLine(
                $"{"Developer",-20} {"Commits",10} {"FileChanges",11} {"Insertions",10} {"Deletions",9}");
            foreach ((string, int, int, int, int) item in statisitcOfAllDevelopers)
            {
                Console.WriteLine(
                  $"{item.Item1,-20} {item.Item2,10} {item.Item3,11} {item.Item4,10} {item.Item5,9}");
            }
            
        }

        private static void PrintResult(QueryDTO item)
        {
            Console.WriteLine(
                   $"{item.Developer.Name,-20} {item.DateTime.ToShortDateString(),-12} {item.FilesChanged,11} {item.Insertions,10} {item.Deletions,9}");
        }

        private static void PrintHeader()
        {
            Console.WriteLine(
                $"{"Developer",-20} {"Date",-12} {"FileChanges",-11} {"Insertions",-10} {"Deletions",-9}");
        }

        private static void PrintResultList(List<QueryDTO> lastFourWeeks)
        {
            foreach (QueryDTO item in lastFourWeeks)
            {
                Console.WriteLine(
                $"{item.Developer.Name,-20} {item.DateTime.ToShortDateString(),-12} {item.FilesChanged,11} {item.Insertions,10} {item.Deletions,9}");
            }
            
        }
    }
}
