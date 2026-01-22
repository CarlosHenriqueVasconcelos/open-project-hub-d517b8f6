import { useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Button } from "@/components/ui/button";
import { useToast } from "@/hooks/use-toast";
import { fetchGeneralConfig, GeneralConfig } from "@/services/generalConfigService";
import { fetchRankings, updateRankingStatus, RankingEntry } from "@/services/rankingService";
import { cn } from "@/lib/utils";

const SUBJECTS = [
  { value: 1, label: "Engineering Design Process (ENG01B) - Campus Ponta Grossa" },
  { value: 4, label: "Industry 4.0 e 5.0 (ENG01C) - Campus Ponta Grossa" },
  { value: 2, label: "Design de Soluções para Problemas Reais (DPR01MEIU) - Campus Apucarana" },
  { value: 8, label: "Engenharia Colaborativa (OPETH003) - Campus Toledo" },
  { value: 16, label: "Processo de Projeto em Engenharia (OP69B) - Campus Londrina" },
  { value: 32, label: "Manutenção 4.0 - Desafios Colaborativos (DC46M) - Campus Pato Branco" },
];

const statusLabel = (status: string) => {
  switch (status) {
    case "confirmado":
      return "Confirmado";
    case "desistencia":
      return "Desistência";
    case "expirado":
      return "Expirado";
    case "espera":
      return "Lista de espera";
    default:
      return "Pendente";
  }
};

const classificationLabel = (classification: string) =>
  classification === "classificado" ? "Classificado" : "Lista de espera";

const Confirmation = () => {
  const navigate = useNavigate();
  const { toast } = useToast();
  const [config, setConfig] = useState<GeneralConfig | null>(null);
  const [rankings, setRankings] = useState<Record<number, RankingEntry[]>>({});
  const [loading, setLoading] = useState(true);
  const [updatingId, setUpdatingId] = useState<number | null>(null);
  const userEmail = sessionStorage.getItem("userEmail");

  const semester = useMemo(() => {
    const now = new Date();
    const year = now.getFullYear();
    return now.getMonth() + 1 <= 6 ? `${year}-1` : `${year}-2`;
  }, []);

  const loadSubject = async (subjectValue: number) => {
    const data = await fetchRankings(subjectValue, semester);
    setRankings((prev) => ({ ...prev, [subjectValue]: data }));
  };

  useEffect(() => {
    if (!userEmail) {
      navigate("/");
      return;
    }

    const init = async () => {
      try {
        const configData = await fetchGeneralConfig();
        if (configData.stage?.toLowerCase() !== "confirmacao") {
          navigate("/verify");
          return;
        }
        setConfig(configData);

        await Promise.all(SUBJECTS.map((subject) => loadSubject(subject.value)));
      } catch (error) {
        console.error("Erro ao carregar classificação:", error);
        toast({
          variant: "destructive",
          title: "Erro",
          description: "Não foi possível carregar a classificação.",
        });
      } finally {
        setLoading(false);
      }
    };

    init();
  }, [navigate, semester, toast, userEmail]);

  const handleUpdateStatus = async (entry: RankingEntry, status: string) => {
    if (!userEmail) return;
    try {
      setUpdatingId(entry.id);
      await updateRankingStatus(entry.id, userEmail, status);
      await loadSubject(entry.subjectValue);
      toast({
        title: "Sucesso",
        description: "Status atualizado com sucesso.",
      });
    } catch (error) {
      toast({
        variant: "destructive",
        title: "Erro",
        description: error instanceof Error ? error.message : "Falha ao atualizar status.",
      });
    } finally {
      setUpdatingId(null);
    }
  };

  const formatDate = (value?: string) => {
    if (!value) return "-";
    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return "-";
    return date.toLocaleString("pt-BR");
  };

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gradient-to-b from-gray-50 to-gray-100">
        <p className="text-gray-600">Carregando classificação...</p>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gradient-to-b from-gray-50 to-gray-100 py-8 px-4">
      <div className="max-w-6xl mx-auto space-y-6">
        <div className="bg-white rounded-xl shadow-lg p-6 space-y-2">
          <h1 className="text-2xl font-semibold text-gray-900">Etapa de Confirmação</h1>
          <p className="text-gray-600">
            Prazo global: {config?.confirmationDeadline ? formatDate(config.confirmationDeadline) : "Não definido"}
          </p>
          <p className="text-gray-600">Semestre atual: {semester}</p>
        </div>

        <Tabs defaultValue={String(SUBJECTS[0].value)} className="space-y-4">
          <TabsList className="flex flex-wrap justify-start">
            {SUBJECTS.map((subject) => (
              <TabsTrigger key={subject.value} value={String(subject.value)}>
                {subject.label}
              </TabsTrigger>
            ))}
          </TabsList>

          {SUBJECTS.map((subject) => {
            const subjectRankings = rankings[subject.value] || [];
            return (
              <TabsContent key={subject.value} value={String(subject.value)}>
                <div className="bg-white rounded-xl shadow-lg p-6">
                  <div className="overflow-x-auto">
                    <Table>
                    <TableHeader>
                      <TableRow>
                        <TableHead className="px-4">Posição</TableHead>
                        <TableHead className="px-4">Nome</TableHead>
                        <TableHead className="px-4">RA</TableHead>
                        <TableHead className="px-4">Email</TableHead>
                        <TableHead className="px-4">Curso</TableHead>
                        <TableHead className="px-4">Campus</TableHead>
                        <TableHead className="px-4">Celular</TableHead>
                        <TableHead className="px-4">Periodo</TableHead>
                        <TableHead className="px-4">Pontuação</TableHead>
                        <TableHead className="px-4">CR</TableHead>
                        <TableHead className="px-4">Classificação</TableHead>
                        <TableHead className="px-4">Status</TableHead>
                        <TableHead className="px-4">Ação</TableHead>
                      </TableRow>
                    </TableHeader>
                    <TableBody>
                      {subjectRankings.length === 0 && (
                        <TableRow>
                          <TableCell colSpan={13} className="text-center text-muted-foreground">
                            Nenhum aluno encontrado para esta matéria.
                          </TableCell>
                        </TableRow>
                      )}
                      {subjectRankings.map((entry) => {
                        const isOwner = entry.student?.email?.toLowerCase() === userEmail?.toLowerCase();
                        const canConfirm =
                          isOwner &&
                          entry.classification === "classificado" &&
                          entry.status === "pendente";
                        const rowClassName = cn(
                          entry.status === "desistencia" && "bg-rose-50",
                          entry.status !== "desistencia" &&
                            entry.classification === "classificado" &&
                            "bg-emerald-50",
                          isOwner && "ring-1 ring-inset ring-primary/20"
                        );
                        return (
                          <TableRow key={entry.id} className={rowClassName}>
                            <TableCell className="px-4">{entry.rankPosition}</TableCell>
                            <TableCell className="px-4">{entry.student?.name || "-"}</TableCell>
                            <TableCell className="px-4">{entry.student?.ra || "-"}</TableCell>
                            <TableCell className="px-4">{entry.student?.email || "-"}</TableCell>
                            <TableCell className="px-4">{entry.student?.currentCourse?.description || "-"}</TableCell>
                            <TableCell className="px-4">{entry.student?.currentCourse?.campus || "-"}</TableCell>
                            <TableCell className="px-4">{entry.student?.cellphone || "-"}</TableCell>
                            <TableCell className="px-4">{entry.student?.currentCourse?.period || "-"}</TableCell>
                            <TableCell className="px-4">{entry.totalScore}</TableCell>
                            <TableCell className="px-4">{entry.performanceCoefficient}</TableCell>
                            <TableCell className="px-4">{classificationLabel(entry.classification)}</TableCell>
                            <TableCell className="px-4">{statusLabel(entry.status)}</TableCell>
                            <TableCell className="px-4">
                              {canConfirm ? (
                                <div className="flex flex-wrap gap-2">
                                  <Button
                                    size="sm"
                                    onClick={() => handleUpdateStatus(entry, "confirmado")}
                                    disabled={updatingId === entry.id}
                                  >
                                    Confirmar
                                  </Button>
                                  <Button
                                    size="sm"
                                    variant="outline"
                                    onClick={() => handleUpdateStatus(entry, "desistencia")}
                                    disabled={updatingId === entry.id}
                                  >
                                    Desistir
                                  </Button>
                                </div>
                              ) : (
                                <span className="text-sm text-muted-foreground">-</span>
                              )}
                            </TableCell>
                          </TableRow>
                        );
                      })}
                    </TableBody>
                  </Table>
                  </div>
                </div>
              </TabsContent>
            );
          })}
        </Tabs>
      </div>
    </div>
  );
};

export default Confirmation;
