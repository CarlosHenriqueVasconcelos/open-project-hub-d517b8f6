
import { useState } from "react";
import { useToast } from "@admin/hooks/use-toast";
import { Button } from "@admin/components/ui/button";
import { Input } from "@admin/components/ui/input";
import { Label } from "@admin/components/ui/label";
import { Checkbox } from "@admin/components/ui/checkbox";
import { Skill } from "@admin/types";
import { useQueryClient } from "@tanstack/react-query";
import axios from "axios";
import { API_BASE_URL } from "@admin/config/api";

interface SkillFormProps {
  skill?: Skill;
  onSuccess?: () => void;
  onCancel?: () => void;
}

const SkillForm = ({ skill, onSuccess, onCancel }: SkillFormProps) => {
  const { toast } = useToast();
  const queryClient = useQueryClient();
  const [name, setName] = useState(skill?.name || "");
  const [tag, setTag] = useState(skill?.tag || "");
  const [isSoftSkill, setIsSoftSkill] = useState(skill?.isSoftSkill || false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const token = localStorage.getItem("token");

    try {
      if (skill?.id) {
        await axios.put(
          `${API_BASE_URL}/skills/${skill.id}`,
          { name, tag, isSoftSkill },
          { headers: { Authorization: `Bearer ${token}` } }
        );
        toast({ title: "Sucesso", description: "Habilidade atualizada com sucesso" });
      } else {
        await axios.post(
          `${API_BASE_URL}/skills`,
          { name, tag, isSoftSkill },
          { headers: { Authorization: `Bearer ${token}` } }
        );
        toast({ title: "Sucesso", description: "Habilidade criada com sucesso" });
      }
      queryClient.invalidateQueries({ queryKey: ["skills"] });
      onSuccess?.();
    } catch (error) {
      toast({
        title: "Erro",
        description: "Falha ao salvar habilidade",
        variant: "destructive",
      });
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div className="space-y-2">
        <Label htmlFor="name">Nome</Label>
        <Input
          id="name"
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
        />
      </div>
      <div className="space-y-2">
        <Label htmlFor="tag">Etiqueta</Label>
        <Input
          id="tag"
          value={tag}
          onChange={(e) => setTag(e.target.value)}
          required
        />
      </div>
      <div className="flex items-center space-x-2">
        <Checkbox
          id="isSoftSkill"
          checked={isSoftSkill}
          onCheckedChange={(checked) => setIsSoftSkill(checked as boolean)}
        />
        <Label htmlFor="isSoftSkill">É Soft Skill</Label>
      </div>
      <div className="flex justify-end space-x-2">
        <Button type="button" variant="outline" onClick={onCancel}>
          Cancelar
        </Button>
        <Button type="submit">{skill ? "Atualizar" : "Criar"}</Button>
      </div>
    </form>
  );
};

export default SkillForm;
