import { useState, useEffect } from "react";
import { useToast } from "../hooks/use-toast";
import { Button } from "../components/ui/button";
import { Label } from "../components/ui/label";
import { Editor } from "@tinymce/tinymce-react";
import axios from "axios";
import { useQueryClient } from "@tanstack/react-query";
import { GeneralConfig } from "../types";
import { Input } from "../components/ui/input";
import { API_BASE_URL } from "../config/api";

const GeneralConfigs = () => {
  const { toast } = useToast();
  const queryClient = useQueryClient();
  const [config, setConfig] = useState<GeneralConfig | null>(null);
  const [configHeader, setConfigHeader] = useState("");
  const [configBody, setConfigBody] = useState("");
  const [configConsent, setConfigConsent] = useState("");
  const [configEmailDomainAvaliable, setConfigEmailDomainAvaliable] = useState(
    config?.config_email_domain_avaliable || ""
  );
  const [stage, setStage] = useState("inscricoes");
  const [confirmationDeadline, setConfirmationDeadline] = useState("");
  const [confirmationDeadlinePhase2, setConfirmationDeadlinePhase2] = useState("");

  const formatDatetimeLocal = (value?: string) => {
    if (!value) return "";
    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return "";
    const pad = (num: number) => String(num).padStart(2, "0");
    return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}T${pad(date.getHours())}:${pad(date.getMinutes())}`;
  };

  const normalizeDeadline = (value: string) => {
    if (!value) return null;
    return value.length === 16 ? `${value}:00` : value;
  };

  useEffect(() => {
    const fetchConfig = async () => {
      try {
        const { data } = await axios.get(`${API_BASE_URL}/general-configs`);
        setConfig(data.data);
        setConfigHeader(data.data.configHeader || "");
        setConfigBody(data.data.configBody || "");
        setConfigConsent(data.data.configConsent || "");
        setConfigEmailDomainAvaliable(data.data.configEmailDomainAvaliable || "");
        setStage(data.data.stage || "inscricoes");
        setConfirmationDeadline(formatDatetimeLocal(data.data.confirmationDeadline));
        setConfirmationDeadlinePhase2(formatDatetimeLocal(data.data.confirmationDeadlinePhase2));
      } catch (error) {
        toast({
          title: "Erro",
          description: "Falha ao carregar a configuração",
          variant: "destructive",
        });
      }
    };

    fetchConfig();
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const token = localStorage.getItem("token");

    try {
      if (config?.id) {
        await axios.post(
          `${API_BASE_URL}/general-configs`,
          {
            configHeader,
            configBody,
            configConsent,
            configEmailDomainAvaliable,
            stage,
            confirmationDeadline: normalizeDeadline(confirmationDeadline),
            confirmationDeadlinePhase2: confirmationDeadlinePhase2
              ? normalizeDeadline(confirmationDeadlinePhase2)
              : null,
          },
          { headers: { Authorization: `Bearer ${token}` } }
        );
        toast({
          title: "Sucesso",
          description: "Configuração atualizada com sucesso",
        });
      } else {
        await axios.post(
          `${API_BASE_URL}/general-configs`,
          {
            configHeader,
            configBody,
            configConsent,
            configEmailDomainAvaliable,
            stage,
            confirmationDeadline: normalizeDeadline(confirmationDeadline),
            confirmationDeadlinePhase2: confirmationDeadlinePhase2
              ? normalizeDeadline(confirmationDeadlinePhase2)
              : null,
          },
          { headers: { Authorization: `Bearer ${token}` } }
        );
        toast({
          title: "Sucesso",
          description: "Configuração criada com sucesso",
        });
      }
      queryClient.invalidateQueries({ queryKey: ["general-configs"] });
    } catch (error) {
      toast({
        title: "Erro",
        description: "Falha ao salvar configuração",
        variant: "destructive",
      });
    }
  };

  const handleDelete = async () => {
    const token = localStorage.getItem("token");

    try {
      await axios.delete(`${API_BASE_URL}/general-configs`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      toast({
        title: "Sucesso",
        description: "Configuração deletada com sucesso",
      });
      setConfig(null);
    } catch (error) {
      toast({
        title: "Erro",
        description: "Falha ao deletar configuração",
        variant: "destructive",
      });
    }
  };

  const calculateSemester = () => {
    const now = new Date();
    const year = now.getFullYear();
    const semester = now.getMonth() + 1 <= 6 ? `${year}-1` : `${year}-2`;
    return semester;
  };

  const handleGenerateRanking = async () => {
    const token = localStorage.getItem("token");
    try {
      await axios.post(
        `${API_BASE_URL}/rankings/generate`,
        { semester: calculateSemester() },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      toast({
        title: "Sucesso",
        description: "Classificação gerada com sucesso.",
      });
    } catch (error) {
      toast({
        title: "Erro",
        description: "Falha ao gerar classificação.",
        variant: "destructive",
      });
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-6">
      <div className="space-y-2">
        <Label htmlFor="configHeader">Cabeçalho da Configuração</Label>
        <Editor
          apiKey="qug694cmk76aemmggb8798l8f9r6gg4mj4s6d8vhxfqc8vd9"
          value={configHeader}
          onEditorChange={(content) => setConfigHeader(content)}
          init={{
            height: 200,
            menubar: false,
            plugins: [
              'advlist', 'autolink', 'lists', 'link', 'image', 'charmap', 'preview',
              'anchor', 'searchreplace', 'visualblocks', 'code', 'fullscreen',
              'insertdatetime', 'media', 'table', 'code', 'help', 'wordcount'
            ],
            toolbar: 'undo redo | blocks | ' +
              'bold italic forecolor | alignleft aligncenter ' +
              'alignright alignjustify | bullist numlist outdent indent | ' +
              'removeformat | help',
          }}
        />
      </div>
      <div className="space-y-2">
        <Label htmlFor="configBody">Corpo da Configuração</Label>
        <Editor
          apiKey="qug694cmk76aemmggb8798l8f9r6gg4mj4s6d8vhxfqc8vd9"
          value={configBody}
          onEditorChange={(content) => setConfigBody(content)}
          init={{
            height: 400,
            menubar: false,
            plugins: [
              'advlist', 'autolink', 'lists', 'link', 'image', 'charmap', 'preview',
              'anchor', 'searchreplace', 'visualblocks', 'code', 'fullscreen',
              'insertdatetime', 'media', 'table', 'code', 'help', 'wordcount'
            ],
            toolbar: 'undo redo | blocks | ' +
              'bold italic forecolor | alignleft aligncenter ' +
              'alignright alignjustify | bullist numlist outdent indent | ' +
              'removeformat | help',
          }}
        />
      </div>
      <div className="space-y-2">
        <Label htmlFor="configConsent">Termo de Consentimento</Label>
        <Editor
          apiKey="qug694cmk76aemmggb8798l8f9r6gg4mj4s6d8vhxfqc8vd9"
          value={configConsent}
          onEditorChange={(content) => setConfigConsent(content)}
          init={{
            height: 300,
            menubar: false,
            plugins: [
              'advlist', 'autolink', 'lists', 'link', 'image', 'charmap', 'preview',
              'anchor', 'searchreplace', 'visualblocks', 'code', 'fullscreen',
              'insertdatetime', 'media', 'table', 'code', 'help', 'wordcount'
            ],
            toolbar: 'undo redo | blocks | ' +
              'bold italic forecolor | alignleft aligncenter ' +
              'alignright alignjustify | bullist numlist outdent indent | ' +
              'removeformat | help',
          }}
        />
      </div>
      <div className="space-y-2">
        <Label htmlFor="configEmailDomainAvaliable">Domínio de Email Disponível, separados por ";"</Label>
        <Input
          id="configEmailDomainAvaliable"
          value={configEmailDomainAvaliable}
          onChange={(e) => setConfigEmailDomainAvaliable(e.target.value)}
          required
        />
      </div>
      <div className="space-y-2">
        <Label>Etapa do Processo</Label>
        <div className="flex flex-wrap gap-4">
          <label className="flex items-center gap-2 text-sm">
            <input
              type="radio"
              name="stage"
              value="inscricoes"
              checked={stage === "inscricoes"}
              onChange={() => setStage("inscricoes")}
            />
            Etapa de Inscrições
          </label>
          <label className="flex items-center gap-2 text-sm">
            <input
              type="radio"
              name="stage"
              value="confirmacao"
              checked={stage === "confirmacao"}
              onChange={() => setStage("confirmacao")}
            />
            Etapa de Confirmação
          </label>
        </div>
      </div>
      <div className="space-y-2">
        <Label htmlFor="confirmationDeadline">Prazo global de confirmação</Label>
        <Input
          id="confirmationDeadline"
          type="datetime-local"
          value={confirmationDeadline}
          onChange={(e) => setConfirmationDeadline(e.target.value)}
        />
      </div>
      <div className="space-y-2">
        <Label htmlFor="confirmationDeadlinePhase2">Prazo da 2ª etapa de confirmação</Label>
        <Input
          id="confirmationDeadlinePhase2"
          type="datetime-local"
          value={confirmationDeadlinePhase2}
          onChange={(e) => setConfirmationDeadlinePhase2(e.target.value)}
        />
      </div>
      <div className="flex justify-end space-x-2">
        <Button type="button" variant="outline" onClick={() => setConfig(null)}>
          Cancelar
        </Button>
        <Button type="button" variant="outline" onClick={handleGenerateRanking}>
          Gerar classificação
        </Button>
        <Button type="submit">{config ? "Atualizar" : "Criar"}</Button>
        {config?.id && (
          <Button
            type="button"
            variant="destructive"
            onClick={handleDelete}
            className="ml-2"
          >
            Deletar
          </Button>
        )}
      </div>
    </form>
  );
};

export default GeneralConfigs;
