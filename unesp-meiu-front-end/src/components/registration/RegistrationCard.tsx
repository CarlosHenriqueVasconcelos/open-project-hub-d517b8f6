import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { CheckCircle, Download, Edit, FileText } from "lucide-react";
import { StudentRegistration, downloadRegistrationPDF } from "@/services/registrationService";
import { useToast } from "@/components/ui/use-toast";

interface RegistrationCardProps {
  registration: StudentRegistration;
  onUpdate: () => void;
  onNewRegistration: () => void;
}

export function RegistrationCard({ registration, onUpdate, onNewRegistration }: RegistrationCardProps) {
  const { toast } = useToast();

  const handleDownloadPDF = async () => {
    try {
      const blob = await downloadRegistrationPDF(registration.id);
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement("a");
      a.href = url;
      a.download = `comprovante-inscricao-${registration.student.ra}.pdf`;
      document.body.appendChild(a);
      a.click();
      window.URL.revokeObjectURL(url);
      document.body.removeChild(a);
      
      toast({
        title: "Download concluído",
        description: "O comprovante foi baixado com sucesso.",
      });
    } catch (error) {
      toast({
        variant: "destructive",
        title: "Erro",
        description: "Não foi possível baixar o comprovante.",
      });
    }
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString("pt-BR", {
      day: "2-digit",
      month: "2-digit",
      year: "numeric",
    });
  };

  return (
    <Card className="w-full max-w-lg">
      <CardHeader className="pb-3">
        <div className="flex items-center justify-between">
          <CardTitle className="text-xl">Inscrição Confirmada</CardTitle>
          <Badge className="bg-green-100 text-green-800 hover:bg-green-100">
            <CheckCircle className="w-3 h-3 mr-1" />
            Confirmado
          </Badge>
        </div>
      </CardHeader>
      <CardContent className="space-y-4">
        <div className="grid grid-cols-2 gap-4 text-sm">
          <div>
            <p className="text-muted-foreground">Nome</p>
            <p className="font-medium">{registration.student.name}</p>
          </div>
          <div>
            <p className="text-muted-foreground">RA</p>
            <p className="font-medium">{registration.student.ra}</p>
          </div>
          <div>
            <p className="text-muted-foreground">Semestre</p>
            <p className="font-medium">{registration.semester}</p>
          </div>
          <div>
            <p className="text-muted-foreground">Data de Inscrição</p>
            <p className="font-medium">{formatDate(registration.registrationDate)}</p>
          </div>
          {registration.student.currentCourse && (
            <div className="col-span-2">
              <p className="text-muted-foreground">Curso</p>
              <p className="font-medium">{registration.student.currentCourse.description}</p>
            </div>
          )}
          <div>
            <p className="text-muted-foreground">Modalidade</p>
            <p className="font-medium">{registration.presencial ? "Presencial" : "Remoto"}</p>
          </div>
        </div>

        <div className="flex flex-col gap-2 pt-4">
          <Button onClick={handleDownloadPDF} className="w-full">
            <Download className="w-4 h-4 mr-2" />
            Baixar Comprovante
          </Button>
          <Button variant="outline" onClick={onUpdate} className="w-full">
            <Edit className="w-4 h-4 mr-2" />
            Atualizar Dados
          </Button>
          <Button variant="ghost" onClick={onNewRegistration} className="w-full">
            <FileText className="w-4 h-4 mr-2" />
            Nova Inscrição
          </Button>
        </div>
      </CardContent>
    </Card>
  );
}
