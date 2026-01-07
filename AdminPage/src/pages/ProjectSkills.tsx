import { useEffect, useState } from "react";
import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import { Card, CardContent, CardHeader, CardTitle } from "@admin/components/ui/card";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@admin/components/ui/select";
import { Button } from "@admin/components/ui/button";
import { useToast } from "@admin/hooks/use-toast";
import { Project, Skill } from "@admin/types";
import { StarRating } from "@admin/components/StarRating";
import { Download } from "lucide-react";
import { API_BASE_URL } from "@admin/config/api";

interface ProjectSkill {
  id: number;
  skill: {
    id: number;
    name: string;
    tag: string;
  };
  level: number;
  project: any[];
}

const ProjectSkills = () => {
  const { toast } = useToast();
  const [selectedProject, setSelectedProject] = useState<Project | null>(null);
  const [skillLevels, setSkillLevels] = useState<{ [key: number]: number }>({});
  const [previousProjectSkills, setPreviousProjectSkills] = useState<ProjectSkill[]>([]);

  const { data: projects } = useQuery({
    queryKey: ["projects"],
    queryFn: async () => {
      const token = localStorage.getItem("token");
      const response = await axios.get(`${API_BASE_URL}/projects`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      return response.data.data as Project[];
    },
  });

  const { data: skills } = useQuery({
    queryKey: ["skills"],
    queryFn: async () => {
      const token = localStorage.getItem("token");
      const response = await axios.get(`${API_BASE_URL}/skills`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      return response.data.data as Skill[];
    },
  });

  const { data: projectSkills } = useQuery({
    queryKey: ["projectSkills", selectedProject?.id],
    queryFn: async () => {
      if (!selectedProject) return null;
      const token = localStorage.getItem("token");
      const response = await axios.get(
        `${API_BASE_URL}/projects/project-skill/${selectedProject.id}`,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      return response.data.data;
    },
    enabled: !!selectedProject,
  });

  useEffect(() => {
    if (projectSkills?.projectSkill) {
      const skillLevelsMap: { [key: number]: number } = {};
      projectSkills.projectSkill.forEach((ps: ProjectSkill) => {
        if (ps.skill.id && ps.level) {
          skillLevelsMap[ps.skill.id] = ps.level;
        }
      });
      setSkillLevels(skillLevelsMap);
    }
  }, [projectSkills]);

  const handleProjectSelect = (projectId: string) => {
    if (projectSkills?.projectSkill) {
      setPreviousProjectSkills(projectSkills.projectSkill);
    }
    
    const project = projects?.find((p) => p.id === parseInt(projectId)) || null;
    setSelectedProject(project);
    
    setSkillLevels({});
  };

  const handleSkillLevelChange = (skillId: number, level: number) => {
    setSkillLevels((prev) => ({
      ...prev,
      [skillId]: level,
    }));
  };

  const handleSubmit = async () => {
    if (!selectedProject) return;

    const skillLevel = Object.entries(skillLevels).map(([skillId, level]) => ({
      skillId: parseInt(skillId),
      level,
    }));

    try {
      const token = localStorage.getItem("token");
      await axios.put(
        `${API_BASE_URL}/projects/project-skill/${selectedProject.id}`,
        {
          description: selectedProject.description,
          internalCode: selectedProject.internalCode,
          presencial: selectedProject.presencial,
          skillLevel,
        },
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      toast({
        title: "Sucesso",
        description: "Habilidades do projeto atualizadas com sucesso",
      });
    } catch (error) {
      toast({
        title: "Erro",
        description: "Falha ao atualizar habilidades do projeto",
        variant: "destructive",
      });
    }
  };

  const generateCSV = async () => {
    if (!projects || !skills) return;

    // Create headers
    const headers = ['Código do Projeto', 'Nome do Projeto', 'Presencial'];
    const skillHeaders = skills.map(skill => skill.name);
    const allHeaders = [...headers, ...skillHeaders].join(',');

    // Get all project skills data
    const token = localStorage.getItem("token");
    const projectsData = await Promise.all(
      projects.map(async (project) => {
        try {
          const response = await axios.get(
            `${API_BASE_URL}/projects/project-skill/${project.id}`,
            {
              headers: { Authorization: `Bearer ${token}` },
            }
          );
          return {
            project,
            skills: response.data.data.projectSkill || []
          };
        } catch (error) {
          console.error(`Error fetching skills for project ${project.id}:`, error);
          return {
            project,
            skills: []
          };
        }
      })
    );

    // Create rows data
    const rows = projectsData.map(({ project, skills: projectSkills }) => {
      const skillLevels: { [key: number]: number } = {};
      projectSkills.forEach((ps: ProjectSkill) => {
        if (ps.skill.id && ps.level) {
          skillLevels[ps.skill.id] = ps.level;
        }
      });

      return [
        project.internalCode,
        project.description,
        project.presencial ? 'Sim' : 'Não',
        ...skills.map(skill => skillLevels[skill.id] || '0')
      ].join(',');
    });

    // Combine headers and data
    const csvContent = `${allHeaders}\n${rows.join('\n')}`;
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    
    // Create download link
    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.setAttribute('download', `projetos_habilidades.csv`);
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  };

  return (
    <Card>
      <CardHeader className="flex flex-row items-center justify-between">
        <CardTitle>Gerenciamento de Habilidades do Projeto</CardTitle>
        <Button onClick={generateCSV} className="flex items-center gap-2">
          <Download className="h-4 w-4" />
          Gerar Planilha de Habilidades
        </Button>
      </CardHeader>
      <CardContent className="space-y-6">
        <div className="space-y-2">
          <label className="text-sm font-medium">Selecione o Projeto</label>
          <Select onValueChange={handleProjectSelect}>
            <SelectTrigger>
              <SelectValue placeholder="Selecione um projeto" />
            </SelectTrigger>
            <SelectContent>
              {projects?.map((project) => (
                <SelectItem key={project.id} value={project.id.toString()}>
                  {project.description}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>

        {selectedProject && (
          <div className="space-y-4">
            <h3 className="font-medium">Avalie as Habilidades Necessárias</h3>
            <div className="space-y-4">
              {skills?.map((skill) => (
                <div
                  key={skill.id}
                  className="flex items-center justify-between p-4 border rounded"
                >
                  <div>
                    <span className="font-medium">{skill.name}</span>
                    <span className="ml-2 text-sm text-gray-500">({skill.tag})</span>
                  </div>
                  <StarRating
                    value={skillLevels[skill.id] || 0}
                    onChange={(value) => handleSkillLevelChange(skill.id, value)}
                  />
                </div>
              ))}
            </div>
            <Button onClick={handleSubmit} className="w-full">
              Salvar Habilidades do Projeto
            </Button>
          </div>
        )}
      </CardContent>
    </Card>
  );
};

export default ProjectSkills;
