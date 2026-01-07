import { useState, useEffect } from "react";
import { useToast } from "@admin/hooks/use-toast";
import { Button } from "@admin/components/ui/button";
import { Label } from "@admin/components/ui/label";
import { Editor } from "@tinymce/tinymce-react";
import axios from "axios";
import { useQueryClient } from "@tanstack/react-query";
import { GeneralConfig } from "@admin/types";
import { Input } from "@admin/components/ui/input";
import { API_BASE_URL } from "@admin/config/api";

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

  useEffect(() => {
    const fetchConfig = async () => {
      try {
        const { data } = await axios.get(`${API_BASE_URL}/general-configs`);
        setConfig(data.data);
        setConfigHeader(data.data.configHeader || "");
        setConfigBody(data.data.configBody || "");
        setConfigConsent(data.data.configConsent || "");
        setConfigEmailDomainAvaliable(data.data.configEmailDomainAvaliable || "");
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
            configEmailDomainAvaliable
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
            configEmailDomainAvaliable
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
      <div className="flex justify-end space-x-2">
        <Button type="button" variant="outline" onClick={() => setConfig(null)}>
          Cancelar
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