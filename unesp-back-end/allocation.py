import pandas as pd
from scipy.spatial.distance import euclidean
import random

class Allocation:
    def __init__(self, projects_skills: str, students_skills: str):
        self.projects_skills = pd.read_csv(projects_skills, index_col=[0])
        self.students_skills = pd.read_csv(students_skills, index_col=[0])
        
        self.projects_list = self.projects_skills.index.tolist()
        self.students_list = self.students_skills.index.tolist()
        
        self.num_projects = self.projects_skills.shape[0]
        self.num_students = self.students_skills.shape[0]
        
        self.max_students_per_project = self.num_students // self.num_projects
        self.num_students_missing = self.num_students % self.num_projects
        
    def _calculate_similarity_score(self, ra: int, project_name: str):
        student = self.students_skills.loc[ra].values
        project = self.projects_skills.loc[project_name].values
        return euclidean(student, project)
    
    def _calculate_similarity_score_of_project(self, project_name: str):
        similarity_scores = []
        for ra in self.students_list:
            similarity_scores.append(self._calculate_similarity_score(ra, project_name))
        return similarity_scores
    
    def _calculate_mean_similarity_score_of_project_allocated(self, project_name: str, projects_allocation_dict: dict):
        sum = 0
        for ra in projects_allocation_dict[project_name]:
            sum += self._calculate_similarity_score(ra, project_name)
        return sum / len(projects_allocation_dict[project_name])
    
    def _calculate_mean_similarity_score_of_all_projects_allocated(self, projects_allocation_dict: dict):
        mean_similarity_scores = []
        for project_name in projects_allocation_dict:
            mean_similarity_scores.append(self._calculate_mean_similarity_score_of_project_allocated(project_name, projects_allocation_dict))
        return mean_similarity_scores
    
    def _calculate_ideal_mean_similarity_score_of_project(self, project_name: str):
        similarity_score_of_project = self._calculate_similarity_score_of_project(project_name)
        min_mean = sum(sorted(similarity_score_of_project)[:self.max_students_per_project]) / self.max_students_per_project
        max_mean = sum(sorted(similarity_score_of_project, reverse=True)[:self.max_students_per_project]) / self.max_students_per_project
        return min_mean + max_mean / 2
    
    def _calculate_ideal_mean_similarity_score_of_all_projects(self):
        ideal_mean_similarity_scores = []
        for project_name in self.projects_list:
            ideal_mean_similarity_scores.append(self._calculate_ideal_mean_similarity_score_of_project(project_name))
        return ideal_mean_similarity_scores
    
    def _random_allocation(self, projects_allocation_dict: dict):
        students = self.students_list.copy()
        projects = self.projects_list.copy()
        for project in projects_allocation_dict.keys():
            for _ in range(self.max_students_per_project):
                random_index = random.randint(0, len(students) - 1)
                student = students.pop(random_index)
                projects_allocation_dict[project].append(student)
        for missing_student in students:
            random_index = random.randint(0, len(projects) - 1)
            projects_allocation_dict[projects[random_index]].append(missing_student)
        return projects_allocation_dict
    
    def _random_swap(self, projects_allocation_dict: dict):
        projects = list(projects_allocation_dict.keys())
        random_project = random.choice(projects)
        projects.remove(random_project)
        random_project_2 = random.choice(projects)
        random_student = random.choice(projects_allocation_dict[random_project])
        random_student_2 = random.choice(projects_allocation_dict[random_project_2])
        projects_allocation_dict[random_project].remove(random_student)
        projects_allocation_dict[random_project_2].remove(random_student_2)
        projects_allocation_dict[random_project].append(random_student_2)
        projects_allocation_dict[random_project_2].append(random_student)
        return projects_allocation_dict   
    
    def allocate(self):
        projects_allocation_dict = {project_name: [] for project_name in self.projects_list}
        projects_allocation_dict = self._random_allocation(projects_allocation_dict)
        
        ideal_mean = self._calculate_ideal_mean_similarity_score_of_all_projects()
        current_mean = self._calculate_mean_similarity_score_of_all_projects_allocated(projects_allocation_dict)
        loss = [abs(ideal_mean[i] - current_mean[i]) for i in range(len(ideal_mean))]
        
        for _ in range(1000):
            min_project_loss = self.projects_skills.index[loss.index(min(loss))]
            projects_allocation_copy = projects_allocation_dict.copy()
            new_allocation = projects_allocation_dict.copy()
            del projects_allocation_copy[min_project_loss]
            new_allocation = self._random_swap(new_allocation)
            projects_allocation_copy.update(new_allocation)
            new_current_mean = self._calculate_mean_similarity_score_of_all_projects_allocated(projects_allocation_copy)
            new_loss = [abs(ideal_mean[i] - new_current_mean[i]) for i in range(len(ideal_mean))]
            if sum(new_loss) < sum(loss):
                projects_allocation_dict = projects_allocation_copy
                loss = new_loss
        self._transform_dict_to_csv(projects_allocation_dict)
    
    def _transform_dict_to_csv(self, projects_allocation_dict: dict):
        rows = []
        for project_name, students in projects_allocation_dict.items():
            for student in students:
                rows.append([student, project_name])
        df = pd.DataFrame(rows, columns=['ra', 'project_name'])
        df.to_csv('allocation.csv')