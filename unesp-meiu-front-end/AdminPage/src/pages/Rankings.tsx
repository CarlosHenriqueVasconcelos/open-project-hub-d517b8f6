import { useEffect, useMemo, useState } from "react";
import axios from "axios";
import { useToast } from "../hooks/use-toast";
import { API_BASE_URL } from "../config/api";
import { RankingEntry } from "../types";
import { Button } from "../components/ui/button";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "../components/ui/table";
import { cn } from "../lib/utils";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "../components/ui/select";

const SUBJECTS = [
  { value: 1, label: "Engineering Design Process (ENG01B) - Campus Ponta Grossa" },
  { value: 4, label: "Industry 4.0 e 5.0 (ENG01C) - Campus Ponta Grossa" },
  { value: 2, label: "Design de Soluções para Problemas Reais (DPR01MEIU) - Campus Apucarana" },
  { value: 8, label: "Engenharia Colaborativa (OPETH003) - Campus Toledo" },
  { value: 16, label: "Processo de Projeto em Engenharia (OP69B) - Campus Londrina" },
  { value: 32, label: "Manutenção 4.0 - Desafios Colaborativos (DC46M) - Campus Pato Branco" },
];

const Rankings = () => {
  const { toast } = useToast();
  const [subjectValue, setSubjectValue] = useState(String(SUBJECTS[0].value));
  const [rankings, setRankings] = useState<RankingEntry[]>([]);
  const [loading, setLoading] = useState(false);

  const semester = useMemo(() => {
    const now = new Date();
    const year = now.getFullYear();
    return now.getMonth() + 1 <= 6 ? `${year}-1` : `${year}-2`;
  }, []);

  const fetchRankings = async (options?: { silent?: boolean }) => {
    try {
      if (!options?.silent) {
        setLoading(true);
      }
      const { data } = await axios.get(`${API_BASE_URL}/rankings`, {
        params: { subject: subjectValue, semester },
      });
      setRankings(data.data || []);
    } catch (error) {
      toast({
        title: "Erro",
        description: "Falha ao carregar a classificação.",
        variant: "destructive",
      });
    } finally {
      if (!options?.silent) {
        setLoading(false);
      }
    }
  };

  useEffect(() => {
    fetchRankings();

    const intervalId = window.setInterval(() => {
      fetchRankings({ silent: true });
    }, 10000);

    return () => window.clearInterval(intervalId);
  }, [subjectValue, semester]);

  const downloadCsv = () => {
    if (!rankings.length) return;
    const headers = [
      "Posição",
      "Nome",
      "RA",
      "Email",
      "Curso",
      "Campus",
      "Celular",
      "Periodo",
      "Pontuação",
      "CR",
      "Classificação",
      "Status",
      "Semestre",
    ];

    const rows = rankings.map((entry) => [
      entry.rankPosition,
      entry.student?.name || "",
      entry.student?.ra || "",
      entry.student?.email || "",
      entry.student?.currentCourse?.description || "",
      entry.student?.currentCourse?.campus || "",
      entry.student?.cellphone || "",
      entry.student?.currentCourse?.period || "",
      entry.totalScore,
      entry.performanceCoefficient,
      entry.classification,
      entry.status,
      entry.semester,
    ]);

    const delimiter = ";";
    const csvContent = [headers, ...rows]
      .map((row) =>
        row
          .map((value) => (value === null || value === undefined ? "" : String(value)))
          .join(delimiter)
      )
      .join("\r\n");

    const blob = new Blob([csvContent], { type: "text/csv;charset=utf-8;" });
    const link = document.createElement("a");
    link.href = URL.createObjectURL(blob);
    link.setAttribute("download", `classificacao_${subjectValue}_${semester}.csv`);
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  };

  return (
    <div className="space-y-6">
      <div className="flex flex-wrap items-center gap-4 justify-between">
        <div className="space-y-2">
          <h1 className="text-2xl font-semibold text-gray-900">Classificação por Matéria</h1>
          <p className="text-gray-600">Semestre: {semester}</p>
        </div>
        <div className="flex flex-wrap gap-2">
          <Select value={subjectValue} onValueChange={setSubjectValue}>
            <SelectTrigger className="w-[320px] bg-white">
              <SelectValue placeholder="Selecione a matéria" />
            </SelectTrigger>
            <SelectContent>
              {SUBJECTS.map((subject) => (
                <SelectItem key={subject.value} value={String(subject.value)}>
                  {subject.label}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
          <Button variant="outline" onClick={downloadCsv} disabled={!rankings.length}>
            Baixar CSV
          </Button>
        </div>
      </div>

      <div className="bg-white rounded-xl shadow-lg p-6">
        {loading ? (
          <p className="text-gray-600">Carregando...</p>
        ) : (
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Posição</TableHead>
                <TableHead>Nome</TableHead>
                <TableHead>RA</TableHead>
                <TableHead>Email</TableHead>
                <TableHead>Curso</TableHead>
                <TableHead>Campus</TableHead>
                <TableHead>Celular</TableHead>
                <TableHead>Periodo</TableHead>
                <TableHead>Pontuação</TableHead>
                <TableHead>CR</TableHead>
                <TableHead>Classificação</TableHead>
                <TableHead>Status</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {rankings.length === 0 && (
                <TableRow>
                  <TableCell colSpan={12} className="text-center text-muted-foreground">
                    Nenhum registro encontrado.
                  </TableCell>
                </TableRow>
              )}
              {rankings.map((entry) => (
                <TableRow
                  key={entry.id}
                  className={cn(
                    entry.status === "desistencia" && "bg-rose-50",
                    entry.status !== "desistencia" &&
                      entry.classification === "classificado" &&
                      "bg-emerald-50"
                  )}
                >
                  <TableCell>{entry.rankPosition}</TableCell>
                  <TableCell>{entry.student?.name || "-"}</TableCell>
                  <TableCell>{entry.student?.ra || "-"}</TableCell>
                  <TableCell>{entry.student?.email || "-"}</TableCell>
                  <TableCell>{entry.student?.currentCourse?.description || "-"}</TableCell>
                  <TableCell>{entry.student?.currentCourse?.campus || "-"}</TableCell>
                  <TableCell>{entry.student?.cellphone || "-"}</TableCell>
                  <TableCell>{entry.student?.currentCourse?.period || "-"}</TableCell>
                  <TableCell>{entry.totalScore}</TableCell>
                  <TableCell>{entry.performanceCoefficient}</TableCell>
                  <TableCell>{entry.classification}</TableCell>
                  <TableCell>{entry.status}</TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        )}
      </div>
    </div>
  );
};

export default Rankings;
