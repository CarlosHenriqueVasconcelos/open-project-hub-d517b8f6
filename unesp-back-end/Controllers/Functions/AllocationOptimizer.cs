using System;
using System.Collections.Generic;
using System.Linq;
using PlataformaGestaoIA.Extensions;

namespace PlataformaGestaoIA.Controllers.Functions
{
    public class AllocationOptimizer
    {
        private Dictionary<int, double[]> studentsSkills;
        private Dictionary<int, double[]> projectsSkills;
        private List<int> projectsList;
        private List<int> studentsList;
        private int maxStudentsPerProject;

        public AllocationOptimizer(Dictionary<int, double[]> studentsSkills, Dictionary<int, double[]> projectsSkills, List<int> projectsList, List<int> studentsList, int maxStudentsPerProject)
        {
            this.studentsSkills = studentsSkills;
            this.projectsSkills = projectsSkills;
            this.projectsList = projectsList;
            this.studentsList = studentsList;
            this.maxStudentsPerProject = maxStudentsPerProject;
        }

        public Dictionary<int, List<int>> OptimizeAllocation()
        {
            var projectsAllocationDict = RandomAllocation();
            var idealMean = IdealSimilarityMeanScoreOfAllProjects();
            var currentMean = SimilarityMeanScoreOfAllProjects(projectsAllocationDict);
            var loss = idealMean.Select((x, i) => Math.Abs(x - currentMean[i])).ToList();

            var lossList = new List<double>();
            lossList.Add(loss.Sum());

            for (int i = 0; i < 1000; i++)
            {
                var minProjectLoss = projectsList[loss.IndexOf(loss.Min())];
                var projectsAllocationCopy = projectsAllocationDict.ToDictionary(entry => entry.Key, entry => entry.Value.ToList());
                var newAllocation = projectsAllocationDict.ToDictionary(entry => entry.Key, entry => entry.Value.ToList());
                projectsAllocationCopy.Remove(minProjectLoss);
                newAllocation = RandomSwap(newAllocation);
                foreach (var kvp in newAllocation)
                {
                    projectsAllocationCopy[kvp.Key] = kvp.Value;
                }
                var newCurrentMean = SimilarityMeanScoreOfAllProjects(projectsAllocationCopy);
                var newLoss = idealMean.Select((x, j) => Math.Abs(x - newCurrentMean[j])).ToList();
                if (newLoss.Sum() < loss.Sum())
                {
                    projectsAllocationDict = projectsAllocationCopy.ToDictionary(entry => entry.Key, entry => entry.Value.ToList());
                    loss = newLoss;
                }
                lossList.Add(loss.Sum());
            }

            return projectsAllocationDict;
        }

        private double EuclideanDistance(double[] a, double[] b)
        {
            if (a.Length != b.Length)
            {
                throw new ArgumentException("Arrays a and b must have the same length");
            }

            double sumSquaredDifferences = 0.0;

            for (int i = 0; i < a.Length; i++)
            {
                double difference = a[i] - b[i];
                sumSquaredDifferences += difference * difference;
            }

            return Math.Sqrt(sumSquaredDifferences);
        }

        private double SimilarityScore(int studentId, int projectId)
        {
            var student = studentsSkills[studentId];
            var project = projectsSkills[projectId];
            return EuclideanDistance(student, project);
        }

        private List<double> SimilarityScoreOfProjectStudents(int projectId)
        {
            var similarityScoreList = new List<double>();
            foreach (var student in studentsList)
            {
                similarityScoreList.Add(SimilarityScore(student, projectId));
            }
            return similarityScoreList;
        }

        private double SimilarityMeanScoreOfProject(int projectId, Dictionary<int, List<int>> projectsAllocationDict)
        {
            var sum = 0.0;
            foreach (var student in projectsAllocationDict[projectId])
            {
                sum += SimilarityScore(student, projectId);
            }
            return sum / projectsAllocationDict[projectId].Count;
        }

        private List<double> SimilarityMeanScoreOfAllProjects(Dictionary<int, List<int>> projectsAllocationDict)
        {
            var similarityMeanScoreList = new List<double>();
            foreach (var projectId in projectsAllocationDict.Keys)
            {
                similarityMeanScoreList.Add(SimilarityMeanScoreOfProject(projectId, projectsAllocationDict));
            }
            return similarityMeanScoreList;
        }

        private double IdealSimilarityMeanScoreOfProject(int projectId)
        {
            var idealSimilarityScoreList = SimilarityScoreOfProjectStudents(projectId);
            var minMean = idealSimilarityScoreList.OrderBy(x => x).Take(maxStudentsPerProject).Sum() / maxStudentsPerProject;
            var maxMean = idealSimilarityScoreList.OrderByDescending(x => x).Take(maxStudentsPerProject).Sum() / maxStudentsPerProject;
            return (minMean + maxMean) / 2;
        }

        private List<double> IdealSimilarityMeanScoreOfAllProjects()
        {
            var idealSimilarityMeanScoreList = new List<double>();
            foreach (var projectId in projectsList)
            {
                idealSimilarityMeanScoreList.Add(IdealSimilarityMeanScoreOfProject(projectId));
            }
            return idealSimilarityMeanScoreList;
        }

        private Dictionary<int, List<int>> RandomAllocation()
        {
            var projectsAllocationDict = new Dictionary<int, List<int>>();
            var students = studentsList.ToList();
            var projects = projectsList.ToList();

            // Embaralhar a lista de estudantes
            students.Shuffle();

            foreach (var project in projects)
            {
                projectsAllocationDict.Add(project, new List<int>());

                // Adicionar apenas maxStudentsPerProject estudantes ao projeto atual
                for (int j = 0; j < maxStudentsPerProject && students.Any(); j++)
                {
                    var randomIndex = new Random().Next(students.Count);
                    var student = students[randomIndex];
                    projectsAllocationDict[project].Add(student);
                    students.RemoveAt(randomIndex);
                }
            }

            return projectsAllocationDict;
        }

        private Dictionary<int, List<int>> RandomSwap(Dictionary<int, List<int>> projectsAllocationDict)
        {
            var projects = projectsAllocationDict.Keys.ToList();
            var randomProject = projects[new Random().Next(projects.Count)];
            projects.Remove(randomProject);
            var randomProject2 = projects[new Random().Next(projects.Count)];
            var randomStudent = projectsAllocationDict[randomProject][new Random().Next(projectsAllocationDict[randomProject].Count)];
            var randomStudent2 = projectsAllocationDict[randomProject2][new Random().Next(projectsAllocationDict[randomProject2].Count)];
            projectsAllocationDict[randomProject].Remove(randomStudent);
            projectsAllocationDict[randomProject2].Remove(randomStudent2);
            projectsAllocationDict[randomProject].Add(randomStudent2);
            projectsAllocationDict[randomProject2].Add(randomStudent);

            return projectsAllocationDict;
        }
    }
}