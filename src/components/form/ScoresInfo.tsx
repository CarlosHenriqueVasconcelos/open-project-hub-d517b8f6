import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { FormField } from "@/components/form/FormField";

const ScoresInfo = ({ formData, setFormData }) => {
  const handleChange = (e) => {
    const name = e.target.name;
    let value = e.target.type === "number" ? parseFloat(e.target.value) : e.target.value;

    if (name === "performanceCoefficient") {
      if (typeof value === "number" && !Number.isNaN(value)) {
        value = Math.min(2, Math.max(1, value));
      }
    }

    setFormData({
      ...formData,
      studentRegistrationScore: {
        ...(formData.studentRegistrationScore || {}),
        [name]: value,
      },
    });
  };

  return (
    <div className="space-y-6">
      <h2 className="text-xl font-semibold text-gray-900">
        Pontuações e Certificações
      </h2>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <FormField label="Coeficiente de Rendimento (entre 1 e 2)" required>
          <Input
            type="number"
            min="0"
            max="1"
            step="0.01"
            name="performanceCoefficient"
            value={
              formData.studentRegistrationScore?.performanceCoefficient||
              ""
            }
            onChange={handleChange}
            placeholder="0.75"
          />
        </FormField>

        <FormField
          label="Ter participado de programa Institucionalizado de iniciação científica e/ou iniciação tecnológica e/ou extensão e/ou PET (Máximo de 6 pontos)."
        >
          <Input
            type="number"
            name="scientificInitiationProgramScore"
            min="0"
            max="6"
            value={
              formData.studentRegistrationScore?.scientificInitiationProgramScore ||
              ""
            }
            onChange={handleChange}
            placeholder="0"
          />
        </FormField>

        <FormField
          label="Ter participado de programa de monitoria institucional (Máximo de 6 pontos)."
        >
          <Input
            type="number"
            name="institutionalMonitoringProgramScore"
            min="0"
            max="6"
            value={
              formData.studentRegistrationScore
                ?.institutionalMonitoringProgramScore || ""
            }
            onChange={handleChange}
            placeholder="0"
          />
        </FormField>

        <FormField
          label="Participado da diretoria de empresa júnior e/ou centro acadêmico e/ou DCE."
        >
          <Input
            type="number"
            name="juniorEnterpriseExperienceScore"
            min="0"
            max="3"
            value={
              formData.studentRegistrationScore?.juniorEnterpriseExperienceScore ||
              ""
            }
            onChange={handleChange}
            placeholder="0"
          />
        </FormField>

        <FormField
          label="Ter projeto no Hotel Tecnológico ou Incubadora Tecnológica (Máximo de 3 pontos)."
        >
          <Input
            type="number"
            name="projectInTechnologicalHotelScore"
            min="0"
            max="3"
            value={
              formData.studentRegistrationScore?.projectInTechnologicalHotelScore ||
              ""
            }
            onChange={handleChange}
            placeholder="0"
          />
        </FormField>

        <FormField
          label="Comprovar realização de estágio/emprego na sua área de formação (Máximo de 8 pontos)."
        >
          <Input
            type="number"
            name="internshipEmploymentScore"
            min="0"
            max="8"
            value={
              formData.studentRegistrationScore?.internshipEmploymentScore ||
              ""
            }
            onChange={handleChange}
            placeholder="0"
          />
        </FormField>

        <FormField
          label="Atividade de voluntariado, devidamente comprovado por declaração da instituição onde a atividade foi realizada.."
          tooltip="Pontuação em atividade de voluntariado"
        >
          <Input
            type="number"
            name="volunteeringScore"
            min="0"
            max="8"
            value={
              formData.studentRegistrationScore?.volunteeringScore ||
              ""
            }
            onChange={handleChange}
            placeholder="0"
          />
        </FormField>

        <FormField
          label="Pontuação por disciplina com notas entre 8,0 e 10,0 (máximo de 3 pontos)."
        >
          <Input
            type="number"
            name="highGradeDisciplineScore"
            min="0"
            max="3"
            value={
              formData.studentRegistrationScore?.highGradeDisciplineScore ||
              ""
            }
            onChange={handleChange}
            placeholder="0"
          />
        </FormField>

        <FormField
          label="Pontuação por disciplina com notas entre 6,0 e 8,0 (máximo de 3 pontos)."
        >
          <Input
            type="number"
            name="midGradeDisciplineScore"
            min="0"
            max="3"
            value={
              formData.studentRegistrationScore?.midGradeDisciplineScore ||
              ""
            }
            onChange={handleChange}
            placeholder="0"
          />
        </FormField>

        <FormField
          label="Comprovação através de certificado em cursos de Phyton e/ou Yolo e/ou TensorFlow e/ou Swift e/ou Julia e/ou R e/ou Linguagens PHP e Flutter(dart) e/ou cursos em Inteligência Artificial."
        >
          <Input
            type="number"
            name="technologyCertificationScore"
            min="0"
            max="8"
            value={
              formData.studentRegistrationScore?.technologyCertificationScore ||
              ""
            }
            onChange={handleChange}
            placeholder="0"
          />
        </FormField>

        <FormField
          label="Comprovação através de Boletim Acadêmico com notas superior a 8,0 nas disciplinas C e/ou C++ e/ou Java e/ou Java Script e/ou Redes Neurais e/ou Banco de dados e/ou Matlab e/ou linguagens de baixo nível e/ou linguagens para inteligência artificial e/ou linguagens para Ciência de Dados, e/ou Séries temporais, e/ou Gestão de projetos e/ou Controle Estatísticos de Processos e/ou Estatística, Automação, e/ou Robótica, e/ou Simulação.."
        >
          <Input
            type="number"
            name="lowLevelTechScore"
            min="0"
            max="8"
            value={
              formData.studentRegistrationScore?.lowLevelTechScore ||
              ""
            }
            onChange={handleChange}
            placeholder="0"
          />
        </FormField>

        <FormField
          label="Participação em projetos em Inteligência Artificial, Machine Learning, Ciência de Dados, Banco de Dados, Redes Neurais, Drone, Lean Manufacturing, Automação, Robótica, Simulação com comprovação."
        >
          <Input
            type="number"
            name="aiProjectsScore"
            min="0"
            max="8"
            value={
              formData.studentRegistrationScore?.aiProjectsScore ||
              ""
            }
            onChange={handleChange}
            placeholder="0"
          />
        </FormField>

        <FormField
          label="Se pontuou na categoria de cursos, escreva os cursos aqui:"
          tooltip="Descreva os cursos e certificações relevantes"
        >
          <Textarea
            name="scoreCoursesDescription"
            value={
              formData.studentRegistrationScore?.scoreCoursesDescription || ""
            }
            onChange={handleChange}
            placeholder="Liste seus cursos e certificações..."
            className="h-32"
          />
        </FormField>
      </div>
    </div>
  );
};

export default ScoresInfo;
