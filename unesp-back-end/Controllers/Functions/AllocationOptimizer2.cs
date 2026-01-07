using System;
using System.Collections.Generic;
using System.Linq;
using PlataformaGestaoIA.Extensions;

namespace PlataformaGestaoIA.Controllers.Functions
{
    /*public class AllocationOptimizer2
    {
        private readonly Dictionary<int, double[]> studentsSkills;
        private readonly Dictionary<int, double[]> projectsSkills;
        private readonly List<int> projectsList;
        private readonly List<int> studentsList;
        private readonly int maxStudentsPerProject;

        public AllocationOptimizer2(Dictionary<int, double[]> studentsSkills, Dictionary<int, double[]> projectsSkills, List<int> projectsList, List<int> studentsList, int maxStudentsPerProject)
        {
            this.studentsSkills = studentsSkills;
            this.projectsSkills = projectsSkills;
            this.projectsList = projectsList;
            this.studentsList = studentsList;
            this.maxStudentsPerProject = maxStudentsPerProject;
        }

        public Dictionary<int, List<int>> OptimizeAllocation()
        {
            var bestAllocation = RandomAllocation();
            var bestLoss = CalculateLoss(bestAllocation);
            var lossList = new List<double> { bestLoss };

            for (int i = 0; i < 10; i++)
            {
                var newAllocation = RandomSwap(bestAllocation);
                var newLoss = CalculateLoss(newAllocation);

                if (newLoss < bestLoss)
                {
                    bestAllocation = newAllocation;
                    bestLoss = newLoss;
                }

                lossList.Add(bestLoss);
            }

            return bestAllocation;
        }

        private double CalculateLoss(Dictionary<int, List<int>> allocation)
        {
            var idealMeans = projectsList.Select(projectId => IdealSimilarityMeanScoreOfProject(projectId)).ToList();
            var currentMeans = projectsList.Select(projectId => SimilarityMeanScoreOfProject(projectId, allocation)).ToList();

            return idealMeans.Zip(currentMeans, (idealMean, currentMean) => Math.Abs(idealMean - currentMean)).Sum();
        }

        private Dictionary<int, List<int>> RandomAllocation()
        {
            var allocation = new Dictionary<int, List<int>>();
            var students = studentsList.ToList();

            students.Shuffle();

            foreach (var projectId in projectsList)
            {
                allocation[projectId] = new List<int>();

                for (int j = 0; j < maxStudentsPerProject && students.Any(); j++)
                {
                    var randomIndex = new Random().Next(students.Count);
                    var student = students[randomIndex];
                    allocation[projectId].Add(student);
                    students.RemoveAt(randomIndex);
                }
            }

            return allocation;
        }

        private Dictionary<int, List<int>> RandomSwap(Dictionary<int, List<int>> allocation)
        {
            var newAllocation = allocation.ToDictionary(entry => entry.Key, entry => entry.Value.ToList());

            var projectIds = newAllocation.Keys.ToList();
            var projectIndex = new Random().Next(projectIds.Count);
            var project1 = projectIds[projectIndex];
            projectIds.RemoveAt(projectIndex);
            var project2 = projectIds[new Random().Next(projectIds.Count)];

            var studentIndex1 = new Random().Next(newAllocation[project1].Count);
            var studentIndex2 = new Random().Next(newAllocation[project2].Count);

            var student1 = newAllocation[project1][studentIndex1];
            var student2 = newAllocation[project2][studentIndex2];

            newAllocation[project1][studentIndex1] = student2;
            newAllocation[project2][studentIndex2] = student1;

            return newAllocation;
        }

        private double SimilarityMeanScoreOfProject(int projectId, Dictionary<int, List<int>> allocation)
        {
            var similarityScores = allocation[projectId].Select(studentId => SimilarityScore(studentId, projectId));
            return similarityScores.Sum() / allocation[projectId].Count;
        }

        private double IdealSimilarityMeanScoreOfProject(int projectId)
        {
            var similarityScores = studentsList.Select(studentId => SimilarityScore(studentId, projectId));
            var sortedScores = similarityScores.OrderBy(score => score).ToList();
            var minMean = sortedScores.Take(maxStudentsPerProject).Average();
            var maxMean = sortedScores.TakeLast(maxStudentsPerProject).Average();

            return (minMean + maxMean) / 2;
        }

        private double SimilarityScore(int studentId, int projectId)
        {
            var studentSkills = studentsSkills[studentId];
            var projectSkills = projectsSkills[projectId];

            return EuclideanDistance(studentSkills, projectSkills);
        }

        private double EuclideanDistance(double[] a, double[] b)
        {
            if (a.Length != b.Length)
                throw new ArgumentException("Arrays a and b must have the same length");

            double sumSquaredDifferences = 0.0;

            for (int i = 0; i < a.Length; i++)
            {
                double difference = a[i] - b[i];
                sumSquaredDifferences += difference * difference;
            }

            return Math.Sqrt(sumSquaredDifferences);
        }
    }*/
}