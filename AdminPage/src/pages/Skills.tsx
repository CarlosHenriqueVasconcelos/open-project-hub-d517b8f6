import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import { Card, CardContent, CardHeader, CardTitle } from "@admin/components/ui/card";
import { Button } from "@admin/components/ui/button";
import { Plus } from "lucide-react";
import { useToast } from "@admin/hooks/use-toast";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@admin/components/ui/dialog";
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from "@admin/components/ui/alert-dialog";
import SkillForm from "@admin/components/SkillForm";
import { Skill } from "@admin/types";
import { API_BASE_URL } from "@admin/config/api";

const Skills = () => {
  const { toast } = useToast();
  const [skills, setSkills] = useState<Skill[]>([]);
  const [isCreateOpen, setIsCreateOpen] = useState(false);
  const [editingSkill, setEditingSkill] = useState<Skill | null>(null);
  const [deletingSkill, setDeletingSkill] = useState<Skill | null>(null);

  const { isLoading } = useQuery({
    queryKey: ["skills"],
    queryFn: async () => {
      try {
        const token = localStorage.getItem("token");
        const response = await axios.get(`${API_BASE_URL}/skills`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        setSkills(response.data.data);
        return response.data.data;
      } catch (error) {
        toast({
          title: "Error",
          description: "Failed to fetch skills",
          variant: "destructive",
        });
        return [];
      }
    },
  });

  const handleDelete = async (id: number) => {
    try {
      const token = localStorage.getItem("token");
      await axios.delete(`${API_BASE_URL}/skills/${id}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      toast({ title: "Success", description: "Skill deleted successfully" });
      setSkills(skills.filter((skill) => skill.id !== id));
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to delete skill",
        variant: "destructive",
      });
    }
    setDeletingSkill(null);
  };

  if (isLoading) {
    return <div>Loading...</div>;
  }

  return (
    <div className="container mx-auto p-4">
      <Card>
        <CardHeader className="flex flex-row items-center justify-between">
          <CardTitle>Habilidades</CardTitle>
          <Button onClick={() => setIsCreateOpen(true)}>
            <Plus className="mr-2 h-4 w-4" /> Adicionar Habilidade
          </Button>
        </CardHeader>
        <CardContent>
          {skills.length === 0 ? (
            <p>No skills found</p>
          ) : (
            <div className="grid gap-4">
              {skills.map((skill) => (
                <div
                  key={skill.id}
                  className="flex items-center justify-between p-4 border rounded"
                >
                  <div>
                    <h3 className="font-medium">{skill.name}</h3>
                    <p className="text-sm text-gray-500">Tag: {skill.tag}</p>
                  </div>
                  <div className="space-x-2">
                    <Button
                      variant="outline"
                      size="sm"
                      onClick={() => setEditingSkill(skill)}
                    >
                      Editar
                    </Button>
                    <Button
                      variant="destructive"
                      size="sm"
                      onClick={() => setDeletingSkill(skill)}
                    >
                      Deletar
                    </Button>
                  </div>
                </div>
              ))}
            </div>
          )}
        </CardContent>
      </Card>

      <Dialog open={isCreateOpen} onOpenChange={setIsCreateOpen}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Criar Habilidade</DialogTitle>
          </DialogHeader>
          <SkillForm
            onSuccess={() => setIsCreateOpen(false)}
            onCancel={() => setIsCreateOpen(false)}
          />
        </DialogContent>
      </Dialog>

      <Dialog open={!!editingSkill} onOpenChange={() => setEditingSkill(null)}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Editar Habilidade</DialogTitle>
          </DialogHeader>
          {editingSkill && (
            <SkillForm
              skill={editingSkill}
              onSuccess={() => setEditingSkill(null)}
              onCancel={() => setEditingSkill(null)}
            />
          )}
        </DialogContent>
      </Dialog>

      <AlertDialog
        open={!!deletingSkill}
        onOpenChange={() => setDeletingSkill(null)}
      >
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Are you sure?</AlertDialogTitle>
            <AlertDialogDescription>
              This action cannot be undone. This will permanently delete the skill.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancel</AlertDialogCancel>
            <AlertDialogAction
              onClick={() => deletingSkill && handleDelete(deletingSkill.id)}
            >
              Deletar
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </div>
  );
};

export default Skills;