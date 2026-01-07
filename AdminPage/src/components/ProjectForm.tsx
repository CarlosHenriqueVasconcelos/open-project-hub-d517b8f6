
import { useState } from "react";
import { useToast } from "@admin/hooks/use-toast";
import { Button } from "@admin/components/ui/button";
import { Input } from "@admin/components/ui/input";
import { Label } from "@admin/components/ui/label";
import { Textarea } from "@admin/components/ui/textarea";
import { Project } from "@admin/types";
import { useQueryClient } from "@tanstack/react-query";
import { Checkbox } from "@admin/components/ui/checkbox";
import axios from "axios";
import { API_BASE_URL } from "@admin/config/api";

interface ProjectFormProps {
  project?: Project;
  onSuccess?: () => void;
  onCancel?: () => void;
}

const ProjectForm = ({ project, onSuccess, onCancel }: ProjectFormProps) => {
  const { toast } = useToast();
  const queryClient = useQueryClient();
  const [description, setDescription] = useState(project?.description || "");
  const [internalCode, setInternalCode] = useState(project?.internalCode || "");
  const [presencial, setPresencial] = useState(project?.presencial || false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const token = localStorage.getItem("token");

    try {
      if (project?.id) {
        await axios.put(
          `${API_BASE_URL}/projects/${project.id}`,
          { description, internalCode, presencial },
          { headers: { Authorization: `Bearer ${token}` } }
        );
        toast({ title: "Sucesso", description: "Projeto atualizado com sucesso" });
      } else {
        await axios.post(
          `${API_BASE_URL}/projects`,
          { description, internalCode, presencial },
          { headers: { Authorization: `Bearer ${token}` } }
        );
        toast({ title: "Sucesso", description: "Projeto criado com sucesso" });
      }
      queryClient.invalidateQueries({ queryKey: ["projects"] });
      onSuccess?.();
    } catch (error) {
      toast({
        title: "Erro",
        description: "Falha ao salvar projeto",
        variant: "destructive",
      });
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div className="space-y-2">
        <Label htmlFor="description">Descrição</Label>
        <Textarea
          id="description"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          required
        />
      </div>
      <div className="space-y-2">
        <Label htmlFor="internalCode">Código Interno</Label>
        <Input
          id="internalCode"
          value={internalCode}
          onChange={(e) => setInternalCode(e.target.value)}
          required
        />
      </div>
      <div className="flex items-center space-x-2">
        <Checkbox
          id="presencial"
          checked={presencial}
          onCheckedChange={(checked) => setPresencial(checked as boolean)}
        />
        <Label htmlFor="presencial">Presencial</Label>
      </div>
      <div className="flex justify-end space-x-2">
        <Button type="button" variant="outline" onClick={onCancel}>
          Cancelar
        </Button>
        <Button type="submit">{project ? "Atualizar" : "Criar"}</Button>
      </div>
    </form>
  );
};

export default ProjectForm;
