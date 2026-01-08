import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Button } from "@/components/ui/button";
import { useToast } from "@/components/ui/use-toast";
import { Loader2, FileX } from "lucide-react";
import { checkRegistrationByEmail, StudentRegistration } from "@/services/registrationService";
import { RegistrationCard } from "@/components/registration/RegistrationCard";

const Verify = () => {
  const navigate = useNavigate();
  const { toast } = useToast();
  const [loading, setLoading] = useState(true);
  const [registration, setRegistration] = useState<StudentRegistration | null>(null);

  useEffect(() => {
    const verifyRegistration = async () => {
      const email = sessionStorage.getItem("userEmail");

      if (!email) {
        toast({
          variant: "destructive",
          title: "Sessão expirada",
          description: "Por favor, faça login novamente.",
        });
        navigate("/");
        return;
      }

      try {
        const result = await checkRegistrationByEmail(email);
        setRegistration(result);
      } catch (error) {
        console.error("Erro ao verificar inscrição:", error);
        toast({
          variant: "destructive",
          title: "Erro",
          description: "Não foi possível verificar sua inscrição. Tente novamente.",
        });
      } finally {
        setLoading(false);
      }
    };

    verifyRegistration();
  }, [navigate, toast]);

  const handleUpdate = () => {
    if (registration) {
      sessionStorage.setItem("registrationData", JSON.stringify(registration));
      sessionStorage.setItem("isUpdate", "true");
    }
    navigate("/form");
  };

  const handleNewRegistration = () => {
    sessionStorage.removeItem("registrationData");
    sessionStorage.removeItem("isUpdate");
    navigate("/form");
  };

  if (loading) {
    return (
      <div className="min-h-screen flex flex-col items-center justify-center bg-gradient-to-b from-gray-50 to-gray-100 p-4">
        <div className="text-center space-y-4">
          <Loader2 className="w-12 h-12 animate-spin text-primary mx-auto" />
          <p className="text-lg text-gray-600">Verificando inscrição...</p>
        </div>
      </div>
    );
  }

  if (!registration) {
    return (
      <div className="min-h-screen flex flex-col items-center justify-center bg-gradient-to-b from-gray-50 to-gray-100 p-4">
        <div className="w-full max-w-md space-y-6 bg-white p-8 rounded-xl shadow-lg text-center">
          <FileX className="w-16 h-16 text-muted-foreground mx-auto" />
          <div className="space-y-2">
            <h2 className="text-2xl font-semibold text-gray-900">Nenhuma Inscrição Encontrada</h2>
            <p className="text-gray-600">
              Não encontramos nenhuma inscrição associada ao seu email. Deseja iniciar uma nova inscrição?
            </p>
          </div>
          <Button onClick={handleNewRegistration} className="w-full py-6">
            Iniciar Inscrição
          </Button>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-gradient-to-b from-gray-50 to-gray-100 p-4">
      <div className="w-full max-w-lg space-y-6">
        <div className="text-center space-y-2">
          <h1 className="text-2xl font-semibold text-gray-900">Verificação de Inscrição</h1>
          <p className="text-gray-600">Encontramos sua inscrição no sistema</p>
        </div>
        <RegistrationCard
          registration={registration}
          onUpdate={handleUpdate}
          onNewRegistration={handleNewRegistration}
        />
      </div>
    </div>
  );
};

export default Verify;
