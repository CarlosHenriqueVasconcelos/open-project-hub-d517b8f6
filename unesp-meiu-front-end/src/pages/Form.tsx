import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { Button } from "@/components/ui/button";
import { Progress } from "@/components/ui/progress";
import PersonalInfo from "@/components/form/PersonalInfo";
import Introduction from "@/components/form/Introduction";
import Registration from "@/components/form/Registration";
import AcademicInfo from "@/components/form/AcademicInfo";
import SkillsInfo from "@/components/form/SkillsInfo";
import ScoresInfo from "@/components/form/ScoresInfo";
import ConsentInfo from "@/components/form/ConsentInfo";
import { useToast } from "@/hooks/use-toast";
import { API_CONFIG } from "@/config/api";
import { SCORE_LIMITS } from "@/config/scoreLimits";

const Form = () => {
  const [step, setStep] = useState(1);
  const [formData, setFormData] = useState({});
  const navigate = useNavigate();
  const { toast } = useToast();

  const totalSteps = 7;
  const progress = (step / totalSteps) * 100;

  useEffect(() => {
    // Verifique se o usuário está autenticado verificando a sessão
    const userEmail = sessionStorage.getItem("userEmail");

    // Se não houver sessão, redirecione para a página de login
    if (!userEmail) {
      navigate("/"); // Redireciona para a página de login
    }
  }, [navigate]);

  const validateForm = (data) => {
    const requiredFields = {
      name: "Nome",
      ra: "RA",
      cpf: "CPF",
      rg: "RG",
      cellphone: "Celular",
      email: "Email",
      courseMode: "Modalidade do Curso",
      courseDescription: "Descrição do Curso",
      period: "Período",
      campus: "Campus",
      file_registration: "Documento de Registro",
      cursarUmaOuDuas: "Cursar ambas as disciplinas",
      agreed: "Consentimento",
    };

    for (const [field, label] of Object.entries(requiredFields)) {
      const value = data[field];
      if (value === undefined || value === null || value === "") {
        throw new Error(`O campo ${label} é obrigatório`);
      }
    }

    return true;
  };

  const formatFormDataForSubmission = (data) => {
    const formattedData = new FormData();
    const normalizedScores = { ...(data.studentRegistrationScore || {}) };
    Object.keys(SCORE_LIMITS).forEach((key) => {
      if (key === "performanceCoefficient") {
        return;
      }
      const value = normalizedScores[key];
      if (value === "" || value === undefined || value === null || Number.isNaN(value)) {
        normalizedScores[key] = 0;
      }
    });

    // Adiciona os campos JSON individualmente
    Object.entries({
      "student.name": data.name,
      "student.ra": data.ra,
      "student.cpf": data.cpf,
      "student.rg": data.rg,
      "student.cellphone": data.cellphone,
      "student.currentCourse.mode": data.courseMode,
      "student.currentCourse.description": data.courseDescription,
      "student.currentCourse.period": data.period,
      "student.currentCourse.campus": data.campus,
      "student.user.email": data.email,
      "registrationDate": new Date().toISOString(),
      "subject": data.subject,
      "choicePriority": data.choicePriority,
      "doesNotMeetRequirements": data.doesNotMeetRequirements,
      "cursarUmaOuDuas": data.cursarUmaOuDuas,
      "semester": data.semester,
      "skillsDescription": data.skillsDescription,
      "agreed": data.agreed
    }).forEach(([key, value]) => {
      if (value !== undefined && value !== null) {
        formattedData.append(key, value);
      }
    });

    // Serializa e adiciona objetos e arrays
    if (data.studentSkills) {
      formattedData.append("studentSkills", JSON.stringify(data.studentSkills));
    }

    if (data.studentRegistrationScore) {
      formattedData.append("studentRegistrationScore", JSON.stringify(normalizedScores));
    }

    // Adiciona o arquivo, se existir
    if (data.file_registration) {
      formattedData.append("file_registration", data.file_registration);
    }

    return formattedData;
  };

  const handleSubmit = async () => {
    try {
      validateForm(formData);
      const formattedData = formatFormDataForSubmission(formData);
      
      const response = await fetch(`${API_CONFIG.baseUrl}/student-registrations`, {
        method: 'POST',
        body: formattedData,
      });

      if (!response.ok) {
        throw new Error(`Erro ao enviar formulário: ${response.statusText}`);
      }

      toast({
        title: "Sucesso!",
        description: "Seu formulário foi enviado com sucesso.",
      });
      
      navigate("/success", { state: { formData } });
    } catch (error) {
      console.error('Submission error:', error);
      toast({
        variant: "destructive",
        title: "Erro ao enviar formulário",
        description: error.message || "Ocorreu um erro ao enviar seus dados. Por favor, tente novamente.",
      });
    }
  };

  const handleNext = () => {
    if (step < totalSteps) {
      if (step === 6) {
        const scores = formData.studentRegistrationScore || {};
        const errors = Object.entries(SCORE_LIMITS).filter(([field, limit]) => {
          const value = scores[field];
          const isEmpty =
            value === "" || value === undefined || value === null || Number.isNaN(value);
          if (isEmpty) {
            return field === "performanceCoefficient";
          }

          if (value < limit.min) {
            return true;
          }

          if (limit.max !== null && value > limit.max) {
            return true;
          }

          return false;
        });

        if (errors.length > 0) {
          toast({
            variant: "destructive",
            title: "Pontuação inválida",
            description:
              "Revise os campos da etapa de pontuação. Alguns valores estão fora do limite permitido.",
          });
          return;
        }
      }

      setStep(step + 1);
      window.scrollTo(0, 0);
    }
  };

  const handlePrevious = () => {
    if (step > 1) {
      setStep(step - 1);
      window.scrollTo(0, 0);
    }
  };

  const renderStep = () => {
    switch (step) {
      case 1: 
        return <Introduction formData={formData} setFormData={setFormData}/>;
      case 2:
        return <PersonalInfo formData={formData} setFormData={setFormData} />;
      case 3:
        return <AcademicInfo formData={formData} setFormData={setFormData} />;
      case 4:
        return <Registration formData={formData} setFormData={setFormData} />;
      case 5:
        return <SkillsInfo formData={formData} setFormData={setFormData} />;
      case 6:
        return <ScoresInfo formData={formData} setFormData={setFormData} />;
      case 7:
        return <ConsentInfo formData={formData} setFormData={setFormData} />;
      default:
        return null;
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-b from-gray-50 to-gray-100 py-8 px-4">
      <div className="max-w-3xl mx-auto">
        <div className="bg-white rounded-xl shadow-lg p-8 space-y-8">
          <div className="space-y-4">
            <h1 className="text-2xl font-semibold text-gray-900 text-center">
              Formulário de Registro
            </h1>
            <Progress value={progress} className="h-2" />
            <div className="text-sm text-gray-600 text-center">
              Etapa {step} de {totalSteps}
            </div>
          </div>

          <div className="space-y-8">{renderStep()}</div>

          <div className="flex justify-between pt-6">
            <Button
              variant="outline"
              onClick={handlePrevious}
              disabled={step === 1}
            >
              Anterior
            </Button>
            {step === totalSteps ? (
              <Button onClick={handleSubmit}>Enviar</Button>
            ) : (
              <Button onClick={handleNext}>Próximo</Button>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default Form;
