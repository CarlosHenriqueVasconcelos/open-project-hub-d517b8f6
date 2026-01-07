import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { getStudentRegistrations, getSkills } from "../services/api";
import { Card, CardContent, CardHeader, CardTitle } from "../components/ui/card";
import { ScrollArea } from "../components/ui/scroll-area";
import { Button } from "../components/ui/button";
import { Download } from "lucide-react";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "../components/ui/select";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "../components/ui/table";
import {
  Pagination,
  PaginationContent,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
} from "../components/ui/pagination";
import { getCurrentSemester, generateSemesterOptions } from "../utils/semesterUtils";
import RegistrationsChart from "../components/RegistrationsChart";
import SkillsDistributionChart from "../components/SkillsDistributionChart";
import StudentSearch from "../components/StudentSearch";
import PDFViewer from "../components/PDFViewer";
import { StudentRegistration } from "../types";
import { API_BASE_URL } from "../config/api";


const ITEMS_PER_PAGE = 5;

const Dashboard = () => {
  const [selectedSemester, setSelectedSemester] = useState(getCurrentSemester());
  const [currentPage, setCurrentPage] = useState(1);
  const [searchQuery, setSearchQuery] = useState("");
  const semesterOptions = generateSemesterOptions();

  const { data: registrations, isLoading: isLoadingRegistrations } = useQuery({
    queryKey: ["studentRegistrations", currentPage],
    queryFn: async () => {
      const response = await getStudentRegistrations(currentPage, ITEMS_PER_PAGE);
      return response.data;
    },
  });

  const { data: skills } = useQuery({
    queryKey: ["skills"],
    queryFn: async () => {
      const response = await getSkills();
      return response.data;
    },
  });

  const filteredRegistrations = registrations?.filter(
    (reg: StudentRegistration) => {
      const matchesSemester = reg.semester === selectedSemester;
      const matchesSearch = searchQuery
        ? reg.student.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
          reg.student.ra.toLowerCase().includes(searchQuery.toLowerCase())
        : true;
      return matchesSemester && matchesSearch;
    }
  ) || [];

  const totalPages = Math.ceil(filteredRegistrations.length / ITEMS_PER_PAGE);

  const calculateAverageScore = () => {
    if (!filteredRegistrations.length) return 0;
  
    const totalScore = filteredRegistrations.reduce((acc, reg) => {
      const scores = [
        reg.studentRegistrationScore?.scientificInitiationProgramScore,
        reg.studentRegistrationScore?.institutionalMonitoringProgramScore,
        reg.studentRegistrationScore?.juniorEnterpriseExperienceScore,
        reg.studentRegistrationScore?.performanceCoefficient,
      ];
  
      const validScores = scores.filter(score => score !== null && score !== undefined);
      const averageScore = validScores.length > 0 ? validScores.reduce((sum, score) => sum + score, 0) / validScores.length : 0;
  
      return acc + averageScore;
    }, 0);
  
    return (totalScore / filteredRegistrations.length).toFixed(2);
  };

  const generateCSV = () => {
    if (!filteredRegistrations || !skills) return;

    const headers = ['Nome do aluno', 'RA', 'Curso', 'Projetos tentados', 'Pode comparecer'];
    const skillHeaders = skills.map(skill => skill.name);
    const allHeaders = [...headers, ...skillHeaders].join(',');

    const rows = filteredRegistrations.map((registration) => {
      const skillLevels: { [key: number]: number } = {};
      registration.studentSkills.forEach((ss) => {
        if (ss.skill.id) {
          skillLevels[ss.skill.id] = ss.level;
        }
      });

      return [
        registration.student.name,
        registration.student.ra,
        registration.student.currentCourse?.description || 'N/A',
        registration.subject === 2 ? '2' : '1',
        registration.presencial ? 'Sim' : 'Não',
        ...skills.map(skill => skillLevels[skill.id] || '0')
      ].join(',');
    });

    const csvContent = `${allHeaders}\n${rows.join('\n')}`;
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.setAttribute('download', `alunos_habilidades_${selectedSemester}.csv`);
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  };

  if (isLoadingRegistrations) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <div className="animate-spin rounded-full h-32 w-32 border-b-2 border-primary"></div>
      </div>
    );
  }

  return (
    <div className="container mx-auto py-8 px-4 animate-fadeIn bg-gradient-to-tr from-white to-secondary/10">
      <div className="flex justify-between items-center mb-8">
        <div className="flex items-center gap-4">
          <h1 className="text-3xl font-bold text-primary">Inscrições dos Alunos</h1>
          <Select value={selectedSemester} onValueChange={setSelectedSemester}>
            <SelectTrigger className="w-[180px] bg-white">
              <SelectValue placeholder="Selecione o semestre" />
            </SelectTrigger>
            <SelectContent>
              {semesterOptions.map((semester) => (
                <SelectItem key={semester} value={semester}>
                  {semester}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>
        <div className="flex items-center gap-4">
          <StudentSearch onSearch={setSearchQuery} />
          <Button onClick={generateCSV} className="flex items-center gap-2 bg-primary hover:bg-primary/90">
            <Download className="h-4 w-4" />
            Gerar Planilha de Alunos
          </Button>
        </div>
      </div>

      <div className="grid gap-6 md:grid-cols-4 mb-8">
        <Card className="bg-gradient-to-br from-white to-secondary/20 border-secondary">
          <CardHeader>
            <CardTitle className="text-primary">Total de Inscrições</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-3xl font-bold text-primary">{filteredRegistrations.length}</p>
          </CardContent>
        </Card>
        <Card className="bg-gradient-to-br from-white to-[#E5DEFF]/30 border-[#E5DEFF]">
          <CardHeader>
            <CardTitle className="text-primary">Podem Comparecer</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-3xl font-bold text-primary">
              {filteredRegistrations.filter(r => r.presencial).length}
            </p>
          </CardContent>
        </Card>
        <Card className="bg-gradient-to-br from-white to-[#D3E4FD]/30 border-[#D3E4FD]">
          <CardHeader>
            <CardTitle className="text-primary">Média de Pontuação</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-3xl font-bold text-primary">
              {calculateAverageScore()}
            </p>
          </CardContent>
        </Card>
        <Card className="bg-gradient-to-br from-white to-[#FDE1D3]/30 border-[#FDE1D3]">
          <CardHeader>
            <CardTitle className="text-primary">Habilidades Únicas</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-3xl font-bold text-primary">
              {new Set(filteredRegistrations.flatMap(r => 
                r.studentSkills.map(s => s.skill.name)
              )).size}
            </p>
          </CardContent>
        </Card>
      </div>

      <div className="grid gap-6 md:grid-cols-4 mb-8">
        <RegistrationsChart registrations={filteredRegistrations} />
        <SkillsDistributionChart registrations={filteredRegistrations} />
      </div>

      <div className="grid gap-6">
        {filteredRegistrations.map((registration) => (
          <Card key={registration.student.id} className="overflow-hidden">
            <CardHeader className="bg-primary/5">
              <div className="flex justify-between items-center">
                <CardTitle className="text-lg">{registration.student.name}</CardTitle>
                <a
                  href={`${API_BASE_URL}/api/v1/student-registrations/${registration.id}/file`}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="text-blue-500 hover:underline"
                >
                  Baixar Arquivo
                </a>
              </div>
            </CardHeader>
            <CardContent className="p-6">
              <div className="space-y-4">
                <div>
                  <p className="text-sm font-medium text-gray-500">Detalhes da Inscrição</p>
                  <p>RA: {registration.student.ra}</p>
                  <p>Semestre: {registration.semester}</p>
                  <p>Data de Inscrição: {new Date(registration.registrationDate).toLocaleDateString()}</p>
                  <p>Pode Comparecer: {registration.presencial ? 'Sim' : 'Não'}</p>
                </div>
                <div>
                  <p className="text-sm font-medium text-gray-500 mb-2">Habilidades</p>
                  <div className="grid grid-cols-2 gap-2">
                    {registration.studentSkills.map((skill) => (
                      <div key={skill.skill.id} className="flex items-center justify-between bg-gray-50 p-2 rounded">
                        <span>{skill.skill.name}</span>
                        <span className="font-medium">Nível {skill.level}</span>
                      </div>
                    ))}
                  </div>
                </div>
              </div>
            </CardContent>
          </Card>
        ))}
      </div>
    </div>
  );
};

export default Dashboard;