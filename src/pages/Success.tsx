
import { useLocation, useNavigate } from "react-router-dom";
import { Button } from "@/components/ui/button";
import { Download, ArrowLeft } from "lucide-react";
import jsPDF from "jspdf";

const Success = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const formData = location.state?.formData;

  const generatePDF = async () => {
    const doc = new jsPDF();
    
    // Configuração inicial do PDF
    doc.setFont("helvetica");
    doc.setFontSize(20);
    doc.text("Confirmação de Inscrição", 20, 20);
    
    doc.setFontSize(12);
    let yPosition = 40;
    
    const addField = (label: string, value: string | number | undefined) => {
      if (value) {
        const text = `${label}: ${value}`;
        const splitText = doc.splitTextToSize(text, 170); // Quebra o texto se for muito longo
        doc.text(splitText, 20, yPosition);
        yPosition += (splitText.length * 7); // Ajusta o espaçamento baseado no número de linhas
      }
    };

    const addSection = (title: string) => {
      yPosition += 10;
      doc.setFont("helvetica", "bold");
      doc.text(title, 20, yPosition);
      doc.setFont("helvetica", "normal");
      yPosition += 10;
    };

    // Informações Pessoais
    addSection("Informações Pessoais");
    addField("Nome Completo", formData?.name);
    addField("RA", formData?.ra);
    addField("CPF", formData?.cpf);
    addField("RG", formData?.rg);
    addField("Celular", formData?.cellphone);
    addField("Email", formData?.email);

    // Informações Acadêmicas
    addSection("Informações Acadêmicas");
    addField("Modalidade do Curso", formData?.courseMode);
    addField("Curso", formData?.courseDescription);
    addField("Período", formData?.period);
    addField("Campus", formData?.campus);

    // Disciplina Selecionada
    addSection("Disciplina Selecionada");

    // Mapeamento dos valores das matérias
    const subjectsMap = {
      1: "Engineering Design Process",
      2: "Design de Soluções Reais",
      4: "Industry 4.0 e 5.0",
      8: "Engenharia Colaborativa",
    };

    // Obtém o valor armazenado das disciplinas selecionadas
    const selectedSubjects = Object.entries(subjectsMap)
      .map(([key, name]) => ({ value: parseInt(key, 10), label: name }))
      .filter(({ value }) => (formData?.subject ?? 0) & value) // Verifica quais matérias estão na soma
      .map(({ label }) => label); // Obtém os nomes correspondentes

    // Adiciona os nomes das matérias selecionadas ao formulário
    addField("Disciplina", selectedSubjects.length > 0 ? selectedSubjects.join(", ") : "Nenhuma selecionada");

    // Habilidades
    if (formData?.studentSkills && formData.studentSkills.length > 0) {
      addSection("Habilidades");
      formData.studentSkills.forEach((skill: { skill: { name: string }, level: number }) => {
        addField(skill.skill.name, `Nível ${skill.level}`);
      });
    }

    // Descrição de Habilidades
    if (formData?.skillsDescription) {
      addSection("Descrição de Habilidades");
      addField("Descrição", formData.skillsDescription);
    }

    // Pontuações
    if (formData?.studentRegistrationScore) {
      addSection("Pontuações");
      const scores = formData.studentRegistrationScore;
      addField("Coeficiente de Rendimento", scores.performanceCoefficient);
      addField("Pontuação em Iniciação Científica", scores.scientificInitiationProgramScore);
      addField("Pontuação em Monitoria", scores.institutionalMonitoringProgramScore);
      addField("Pontuação em Empresa Júnior", scores.juniorEnterpriseExperienceScore);
      addField("Pontuação em Hotel Tecnológico", scores.projectInTechnologicalHotelScore);
      addField("Pontuação em Estágio", scores.internshipEmploymentScore);
      addField("Pontuação em Voluntariado", scores.volunteeringScore);
      
      if (scores.scoreCoursesDescription) {
        addField("Descrição dos Cursos", scores.scoreCoursesDescription);
      }
    }

    // Dados do Request
    addSection("Dados da Requisição");
    const requestData = {
      student: {
        name: formData.name,
        ra: formData.ra,
        cpf: formData.cpf,
        rg: formData.rg,
        cellphone: formData.cellphone,
        user: {
          email: formData.email
        },
        currentCourse: {
          mode: formData.courseMode,
          description: formData.courseDescription,
          period: formData.period,
          campus: formData.campus
        }
      },
      subject: formData.subject,
      studentSkills: formData.studentSkills,
      studentRegistrationScore: formData.studentRegistrationScore,
      skillsDescription: formData.skillsDescription,
      semester: "2024.1",
      registrationDate: new Date().toISOString()
    };

    // Adiciona o JSON formatado
    yPosition += 10;
    const jsonString = JSON.stringify(requestData, null, 2);
    const jsonLines = doc.splitTextToSize(jsonString, 170);
    
    // Se o conteúdo for muito grande, cria uma nova página
    if (yPosition + (jsonLines.length * 7) > 270) {
      doc.addPage();
      yPosition = 20;
    }
    
    doc.setFontSize(8); // Fonte menor para o JSON
    doc.text(jsonLines, 20, yPosition);
    doc.setFontSize(12); // Restaura o tamanho da fonte

    // Adiciona o documento enviado pelo usuário se existir
    if (formData.file_registration && formData.file_registration instanceof File) {
      try {
        const fileReader = new FileReader();
        
        await new Promise<void>((resolve, reject) => {
          fileReader.onload = async function() {
            try {
              const existingPdfBytes = fileReader.result as ArrayBuffer;
              const existingPdf = new Uint8Array(existingPdfBytes);
              
              // Cria um novo documento combinando o atual com o enviado
              const newDoc = new jsPDF();
              
              // Primeiro adiciona todas as páginas do documento atual
              for (let i = 1; i <= doc.getNumberOfPages(); i++) {
                if (i > 1) newDoc.addPage();
                newDoc.setPage(i);
                newDoc.addPage();
              const pdfOutput = doc.output('arraybuffer');
              newDoc.addFileToVFS(`page${i}.pdf`, new Uint8Array(pdfOutput).reduce((data, byte) => data + String.fromCharCode(byte), ''));
              }
              
              // Adiciona o documento enviado pelo usuário
              newDoc.addPage();
              const uploadedBytes = new Uint8Array(existingPdfBytes);
              newDoc.addFileToVFS('uploaded.pdf', uploadedBytes.reduce((data, byte) => data + String.fromCharCode(byte), ''));
              
              // Salva o documento combinado
              newDoc.save("confirmacao-inscricao.pdf");
              resolve();
            } catch (error) {
              reject(error);
            }
          };
          
          fileReader.onerror = reject;
          fileReader.readAsArrayBuffer(formData.file_registration);
        });
      } catch (error) {
        console.error('Erro ao combinar PDFs:', error);
        // Se houver erro na combinação, salva apenas o documento principal
        doc.save("confirmacao-inscricao.pdf");
      }
    } else {
      // Se não houver documento para combinar, salva apenas o documento principal
      doc.save("confirmacao-inscricao.pdf");
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-b from-gray-50 to-gray-100 py-8 px-4">
      <div className="max-w-3xl mx-auto">
        <div className="bg-white rounded-xl shadow-lg p-8 space-y-8 text-center">
          <div className="space-y-4">
            <div className="h-16 w-16 bg-green-100 rounded-full flex items-center justify-center mx-auto">
              <svg
                className="h-8 w-8 text-green-600"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M5 13l4 4L19 7"
                />
              </svg>
            </div>
            <h1 className="text-3xl font-semibold text-gray-900">
              Sua inscrição foi realizada com sucesso!
            </h1>
            <p className="text-gray-600">
              Baixe sua confirmação de inscrição abaixo
            </p>
          </div>

          <div className="space-y-4">
            <Button
              onClick={generatePDF}
              className="w-full sm:w-auto flex items-center justify-center"
            >
              <Download className="mr-2 h-4 w-4" />
              Baixar PDF
            </Button>

            <div>
              <Button
                variant="outline"
                onClick={() => navigate("/")}
                className="w-full sm:w-auto flex items-center justify-center mt-4"
              >
                <ArrowLeft className="mr-2 h-4 w-4" />
                Voltar ao Início
              </Button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Success;
