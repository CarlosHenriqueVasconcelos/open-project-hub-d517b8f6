
import { useState } from "react";
import { useToast } from "@admin/hooks/use-toast";
import { Button } from "@admin/components/ui/button";
import { Input } from "@admin/components/ui/input";
import { Label } from "@admin/components/ui/label";
import { Textarea } from "@admin/components/ui/textarea";
import { GeneralConfig } from "@admin/types";
import { useQueryClient } from "@tanstack/react-query";
import axios from "axios";
import { API_BASE_URL } from "@admin/config/api";

interface GeneralConfigFormProps {
  config?: GeneralConfig;
  onSuccess?: () => void;
  onCancel?: () => void;
}

const GeneralConfigForm = ({ config, onSuccess, onCancel }: GeneralConfigFormProps) => {
  const { toast } = useToast();
  const queryClient = useQueryClient();
  const [configHeader, setConfigHeader] = useState(config?.config_header || "");
  const [configBody, setConfigBody] = useState(config?.config_body || "");
  const [configEmailDomainAvaliable, setConfigEmailDomainAvaliable] = useState(
    config?.config_email_domain_avaliable || ""
  );

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const token = localStorage.getItem("token");

    try {
      if (config?.id) {
        await axios.put(
          `${API_BASE_URL}/general-configs/${config.id}`,
          { 
            config_header: configHeader, 
            config_body: configBody,
            config_email_domain_avaliable: configEmailDomainAvaliable 
          },
          { headers: { Authorization: `Bearer ${token}` } }
        );
        toast({ title: "Sucesso", description: "Configuração atualizada com sucesso" });
      } else {
        await axios.post(
          `${API_BASE_URL}/general-configs`,
          { 
            config_header: configHeader, 
            config_body: configBody,
            config_email_domain_avaliable: configEmailDomainAvaliable 
          },
          { headers: { Authorization: `Bearer ${token}` } }
        );
        toast({ title: "Sucesso", description: "Configuração criada com sucesso" });
      }
      queryClient.invalidateQueries({ queryKey: ["general-configs"] });
      onSuccess?.();
    } catch (error) {
      toast({
        title: "Erro",
        description: "Falha ao salvar configuração",
        variant: "destructive",
      });
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div className="space-y-2">
        <Label htmlFor="config_header">Cabeçalho da Configuração</Label>
        <Input
          id="config_header"
          value={configHeader}
          onChange={(e) => setConfigHeader(e.target.value)}
          required
        />
      </div>
      <div className="space-y-2">
        <Label htmlFor="config_body">Corpo da Configuração</Label>
        <Textarea
          id="config_body"
          value={configBody}
          onChange={(e) => setConfigBody(e.target.value)}
          required
        />
      </div>
      <div className="space-y-2">
        <Label htmlFor="config_email_domain_avaliable">
          Domínios permitidos para o cadastro, separados por ';'
        </Label>
        <Input
          id="config_email_domain_avaliable"
          value={configEmailDomainAvaliable}
          onChange={(e) => setConfigEmailDomainAvaliable(e.target.value)}
          placeholder="exemplo.com;outro-dominio.com"
          required
        />
      </div>
      <div className="flex justify-end space-x-2">
        <Button type="button" variant="outline" onClick={onCancel}>
          Cancelar
        </Button>
        <Button type="submit">{config ? "Atualizar" : "Criar"}</Button>
      </div>
    </form>
  );
};

export default GeneralConfigForm;
