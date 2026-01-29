import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { FormField } from "@/components/form/FormField";
import { SCORE_LIMITS } from "@/config/scoreLimits";

const ScoresInfo = ({ formData, setFormData }) => {
  const isOutOfRange = (value, limit) => {
    if (value === "" || value === null || value === undefined) {
      return false;
    }

    if (Number.isNaN(value)) {
      return false;
    }

    if (value < limit.min) {
      return true;
    }

    if (limit.max !== null && value > limit.max) {
      return true;
    }

    return false;
  };

  const getInputClassName = (invalid) =>
    invalid ? "border-red-500 focus:border-red-500 focus:ring-red-500" : "";

  const handleChange = (e) => {
    const name = e.target.name;
    let value = e.target.value;
    if (e.target.type === "number") {
      value = value === "" ? "" : parseFloat(value);
    }

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
        <FormField
          label="Coeficiente de Rendimento Normalizado (CR) - Valor retirado do Histórico Escolar multiplicado por 2, utilizado somente em caso de empate"
          required
        >
          <Input
            type="number"
            min={SCORE_LIMITS.performanceCoefficient.min}
            max={SCORE_LIMITS.performanceCoefficient.max ?? undefined}
            step={SCORE_LIMITS.performanceCoefficient.step}
            name="performanceCoefficient"
            value={
              formData.studentRegistrationScore?.performanceCoefficient ?? ""
            }
            aria-invalid={isOutOfRange(
              formData.studentRegistrationScore?.performanceCoefficient,
              SCORE_LIMITS.performanceCoefficient
            )}
            className={getInputClassName(
              isOutOfRange(
                formData.studentRegistrationScore?.performanceCoefficient,
                SCORE_LIMITS.performanceCoefficient
              )
            )}
            onChange={handleChange}
            placeholder="1.50"
          />
        </FormField>

        <FormField
          label="Ter participado de programa Institucionalizado de iniciação científica e/ou iniciação tecnológica e/ou extensão e/ou PET - 02 pontos a cada 6 meses comprovados (Máximo 06)."
        >
          <Input
            type="number"
            name="scientificInitiationProgramScore"
            min={SCORE_LIMITS.scientificInitiationProgramScore.min}
            max={SCORE_LIMITS.scientificInitiationProgramScore.max ?? undefined}
            value={
              formData.studentRegistrationScore?.scientificInitiationProgramScore ??
              ""
            }
            aria-invalid={isOutOfRange(
              formData.studentRegistrationScore?.scientificInitiationProgramScore,
              SCORE_LIMITS.scientificInitiationProgramScore
            )}
            className={getInputClassName(
              isOutOfRange(
                formData.studentRegistrationScore?.scientificInitiationProgramScore,
                SCORE_LIMITS.scientificInitiationProgramScore
              )
            )}
            onChange={handleChange}
            placeholder="0"
          />
        </FormField>

        <FormField
          label="Ter participado de programa de monitoria institucional - 03 pontos a cada semestre letivo completo comprovado (Máximo 06)."
        >
          <Input
            type="number"
            name="institutionalMonitoringProgramScore"
            min={SCORE_LIMITS.institutionalMonitoringProgramScore.min}
            max={SCORE_LIMITS.institutionalMonitoringProgramScore.max ?? undefined}
            value={
              formData.studentRegistrationScore
                ?.institutionalMonitoringProgramScore ?? ""
            }
            aria-invalid={isOutOfRange(
              formData.studentRegistrationScore?.institutionalMonitoringProgramScore,
              SCORE_LIMITS.institutionalMonitoringProgramScore
            )}
            className={getInputClassName(
              isOutOfRange(
                formData.studentRegistrationScore?.institutionalMonitoringProgramScore,
                SCORE_LIMITS.institutionalMonitoringProgramScore
              )
            )}
            onChange={handleChange}
            placeholder="0"
          />
        </FormField>

        <FormField
          label="Participado da diretoria de empresa júnior e/ou centro acadêmico e/ou DCE - 01 ponto a cada 12 meses comprovados (Máximo 02)."
        >
          <Input
            type="number"
            name="juniorEnterpriseExperienceScore"
            min={SCORE_LIMITS.juniorEnterpriseExperienceScore.min}
            max={SCORE_LIMITS.juniorEnterpriseExperienceScore.max ?? undefined}
            value={
              formData.studentRegistrationScore?.juniorEnterpriseExperienceScore ??
              ""
            }
            aria-invalid={isOutOfRange(
              formData.studentRegistrationScore?.juniorEnterpriseExperienceScore,
              SCORE_LIMITS.juniorEnterpriseExperienceScore
            )}
            className={getInputClassName(
              isOutOfRange(
                formData.studentRegistrationScore?.juniorEnterpriseExperienceScore,
                SCORE_LIMITS.juniorEnterpriseExperienceScore
              )
            )}
            onChange={handleChange}
            placeholder="0"
          />
        </FormField>

        <FormField
          label="Ter projeto no Hotel Tecnológico ou Incubadora Tecnológica - 01 ponto a cada 12 meses comprovado (Máximo 02)."
        >
          <Input
            type="number"
            name="projectInTechnologicalHotelScore"
            min={SCORE_LIMITS.projectInTechnologicalHotelScore.min}
            max={SCORE_LIMITS.projectInTechnologicalHotelScore.max ?? undefined}
            value={
              formData.studentRegistrationScore?.projectInTechnologicalHotelScore ??
              ""
            }
            aria-invalid={isOutOfRange(
              formData.studentRegistrationScore?.projectInTechnologicalHotelScore,
              SCORE_LIMITS.projectInTechnologicalHotelScore
            )}
            className={getInputClassName(
              isOutOfRange(
                formData.studentRegistrationScore?.projectInTechnologicalHotelScore,
                SCORE_LIMITS.projectInTechnologicalHotelScore
              )
            )}
            onChange={handleChange}
            placeholder="0"
          />
        </FormField>

        <FormField
          label="Comprovar realização de estágio/emprego na sua área de formação - 02 pontos a cada 100 horas comprovadas (Máximo 08)."
        >
          <Input
            type="number"
            name="internshipEmploymentScore"
            min={SCORE_LIMITS.internshipEmploymentScore.min}
            max={SCORE_LIMITS.internshipEmploymentScore.max ?? undefined}
            value={
              formData.studentRegistrationScore?.internshipEmploymentScore ??
              ""
            }
            aria-invalid={isOutOfRange(
              formData.studentRegistrationScore?.internshipEmploymentScore,
              SCORE_LIMITS.internshipEmploymentScore
            )}
            className={getInputClassName(
              isOutOfRange(
                formData.studentRegistrationScore?.internshipEmploymentScore,
                SCORE_LIMITS.internshipEmploymentScore
              )
            )}
            onChange={handleChange}
            placeholder="0"
          />
        </FormField>

        <FormField
          label="Atividade de voluntariado, devidamente comprovado por declaração da instituição onde a atividade foi realizada - 0,5 ponto a cada mês comprovado (Máximo 02)."
        >
          <Input
            type="number"
            name="volunteeringScore"
            min={SCORE_LIMITS.volunteeringScore.min}
            max={SCORE_LIMITS.volunteeringScore.max ?? undefined}
            step={SCORE_LIMITS.volunteeringScore.step}
            value={
              formData.studentRegistrationScore?.volunteeringScore ?? ""
            }
            aria-invalid={isOutOfRange(
              formData.studentRegistrationScore?.volunteeringScore,
              SCORE_LIMITS.volunteeringScore
            )}
            className={getInputClassName(
              isOutOfRange(
                formData.studentRegistrationScore?.volunteeringScore,
                SCORE_LIMITS.volunteeringScore
              )
            )}
            onChange={handleChange}
            placeholder="0"
          />
        </FormField>

        <FormField
          label="Nota em uma das disciplinas Engineering Design Process, Industry 4.0 e 5.0, Design de Soluções para Problemas Reais, Engenharia Colaborativa ou Processo de Projeto em Engenharia a partir do período letivo 2021-1, superior a 9,0- 30 pontos por disciplina (sem limite)."
        >
          <Input
            type="number"
            name="highGradeDisciplineScore"
            min={SCORE_LIMITS.highGradeDisciplineScore.min}
            value={
              formData.studentRegistrationScore?.highGradeDisciplineScore ?? ""
            }
            aria-invalid={isOutOfRange(
              formData.studentRegistrationScore?.highGradeDisciplineScore,
              SCORE_LIMITS.highGradeDisciplineScore
            )}
            className={getInputClassName(
              isOutOfRange(
                formData.studentRegistrationScore?.highGradeDisciplineScore,
                SCORE_LIMITS.highGradeDisciplineScore
              )
            )}
            onChange={handleChange}
            placeholder="0"
          />
        </FormField>

        <FormField
          label="Comprovação através de Boletim Acadêmico com notas entre 6,0 a 8,0 nas disciplinas C e/ou C++ e/ou Java e/ou Java Script e/ou Redes Neurais e/ou Banco de dados e/ou Matlab e/ou linguagens de baixo nível e/ou linguagens para inteligência artificial e/ou linguagens para Ciência de Dados, e/ou Séries temporais, e/ou Gestão de projetos e/ou Controle Estatísticos de Processos, e/ou Engenharia da Qualidade e/ou Estatística, Automação, e/ou Robótica, e/ou Simulação, Eletrônica Digital, Redes, Projetos de Instalações, Dispositivos de Programação, Comunicações Sem Fio, Controle Digital, Eletrônica, Microcontroladores, Computação I Computação II, Teoria de Controle, Instrumentação, Energia e Eficiência Energética - 02 ponto por cada disciplina (Máximo 06)."
        >
          <Input
            type="number"
            name="highGradeCoursesScore"
            min={SCORE_LIMITS.highGradeCoursesScore.min}
            max={SCORE_LIMITS.highGradeCoursesScore.max ?? undefined}
            value={
              formData.studentRegistrationScore?.highGradeCoursesScore ?? ""
            }
            aria-invalid={isOutOfRange(
              formData.studentRegistrationScore?.highGradeCoursesScore,
              SCORE_LIMITS.highGradeCoursesScore
            )}
            className={getInputClassName(
              isOutOfRange(
                formData.studentRegistrationScore?.highGradeCoursesScore,
                SCORE_LIMITS.highGradeCoursesScore
              )
            )}
            onChange={handleChange}
            placeholder="0"
          />
        </FormField>

        <FormField
          label="Comprovação através de certificado em cursos de Phyton e/ou Yolo e/ou TensorFlow e/ou Swift e/ou Julia e/ou R e/ou Linguagens PHP e Flutter(dart) e/ou cursos em Inteligência Artificial, e/ou Machine Learning, e/ou Deep Learning, Matlab, e/ou Estatística, e/ou Scrum, e/ou Design Thinking, e/ou Lean Manufacturing, e/ou Automação, Redes Industriais, Projetos, Gestão de Projetos, FPGA - 02 pontos por curso (Máximo 06)."
        >
          <Input
            type="number"
            name="technologyCertificationScore"
            min={SCORE_LIMITS.technologyCertificationScore.min}
            max={SCORE_LIMITS.technologyCertificationScore.max ?? undefined}
            value={
              formData.studentRegistrationScore?.technologyCertificationScore ??
              ""
            }
            aria-invalid={isOutOfRange(
              formData.studentRegistrationScore?.technologyCertificationScore,
              SCORE_LIMITS.technologyCertificationScore
            )}
            className={getInputClassName(
              isOutOfRange(
                formData.studentRegistrationScore?.technologyCertificationScore,
                SCORE_LIMITS.technologyCertificationScore
              )
            )}
            onChange={handleChange}
            placeholder="0"
          />
        </FormField>

        <FormField
          label="Comprovação através de Boletim Acadêmico com notas superior a 8,0 nas disciplinas C e/ou C++ e/ou Java e/ou Java Script e/ou Redes Neurais e/ou Banco de dados e/ou Matlab e/ou linguagens de baixo nível e/ou linguagens para inteligência artificial e/ou linguagens para Ciência de Dados, e/ou Séries temporais, e/ou Gestão de projetos e/ou Controle Estatísticos de Processos, e/ou Engenharia da Qualidade e/ou Estatística, Automação, e/ou Robótica, e/ou Simulação, Eletrônica Digital, Redes, Projetos de Instalações, Dispositivos de Programação, Comunicações Sem Fio, Controle Digital, Eletrônica, Microcontroladores, Computação I Computação II, Teoria de Controle, Instrumentação, Energia e Eficiência Energética - 08 pontos por cada disciplina (Máximo 24)."
        >
          <Input
            type="number"
            name="lowLevelTechScore"
            min={SCORE_LIMITS.lowLevelTechScore.min}
            max={SCORE_LIMITS.lowLevelTechScore.max ?? undefined}
            value={
              formData.studentRegistrationScore?.lowLevelTechScore ?? ""
            }
            aria-invalid={isOutOfRange(
              formData.studentRegistrationScore?.lowLevelTechScore,
              SCORE_LIMITS.lowLevelTechScore
            )}
            className={getInputClassName(
              isOutOfRange(
                formData.studentRegistrationScore?.lowLevelTechScore,
                SCORE_LIMITS.lowLevelTechScore
              )
            )}
            onChange={handleChange}
            placeholder="0"
          />
        </FormField>

        <FormField
          label="Participação em projetos em Inteligência Artificial, Machine Learning, Ciência de Dados, Banco de Dados, Redes Neurais, Drone, lean manufacturing, Automação, Robótica, Simulação, Projetos tecnológicos, Sistemas embarcados (automotivos e industriais), Sistemas de automação industrial, Instalações elétricas industriais, Sistemas da Internet das Coisas (IoT), Energias Renováveis, com comprovação - 10 pontos por cada projeto (Máximo 30)."
        >
          <Input
            type="number"
            name="aiProjectsScore"
            min={SCORE_LIMITS.aiProjectsScore.min}
            max={SCORE_LIMITS.aiProjectsScore.max ?? undefined}
            value={
              formData.studentRegistrationScore?.aiProjectsScore ?? ""
            }
            aria-invalid={isOutOfRange(
              formData.studentRegistrationScore?.aiProjectsScore,
              SCORE_LIMITS.aiProjectsScore
            )}
            className={getInputClassName(
              isOutOfRange(
                formData.studentRegistrationScore?.aiProjectsScore,
                SCORE_LIMITS.aiProjectsScore
              )
            )}
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
