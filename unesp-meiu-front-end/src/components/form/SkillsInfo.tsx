import { useState, useEffect } from "react";
import { Input } from "@/components/ui/input";
import { FormField } from "@/components/form/FormField";
import { StarRating } from "../ui/star-rating";
import { API_CONFIG } from "@/config/api";

const REQUIRED_SKILLS = 6;

const SkillsInfo = ({ formData, setFormData }) => {
  const [skills, setSkills] = useState({ softSkills: [], hardSkills: [] });
  
  const handleChange = (e) => {
    const rawValue = e.target.value;
    setFormData({ ...formData, [e.target.name]: rawValue });
  };

  const fetchSkills = async () => {
    try {
      const response = await fetch(`${API_CONFIG.baseUrl}/skills/`, {
        method: "GET",
        headers: API_CONFIG.headers
      });

      if (!response.ok) {
        throw new Error("Falha ao carregar as habilidades");
      }

      const data = await response.json();
      const categorizedSkills = data.data.reduce(
        (acc, skill) => {
          if (skill.isSoftSkill) {
            acc.softSkills.push(skill);
          } else {
            acc.hardSkills.push(skill);
          }
          return acc;
        },
        { softSkills: [], hardSkills: [] }
      );

      setSkills(categorizedSkills);
    } catch (error) {
      console.error(error.message);
    }
  };

  useEffect(() => {
    fetchSkills();
  }, []);

  const handleSkillLevelChange = (name, level) => {
    const currentSkills = [...(formData.studentSkills || [])];
    const existingSkillIndex = currentSkills.findIndex((s) => s.skill.name === name);
    const selectedCount = currentSkills.filter((s) => (s.level || 0) > 0).length;

    if (existingSkillIndex > -1) {
      const prevLevel = currentSkills[existingSkillIndex].level || 0;
      const isActivating = prevLevel === 0 && level > 0;
      
      if (isActivating && selectedCount >= REQUIRED_SKILLS) {
        alert("Você já selecionou 6 habilidades. Para adicionar outra, desmarque uma existente clicando na estrela marcada.");
        return;
      }
      currentSkills[existingSkillIndex].level = level;
    } else {
      if (level > 0 && selectedCount >= REQUIRED_SKILLS) {
        alert("Você já selecionou 6 habilidades. Para adicionar outra, desmarque uma existente clicando na estrela marcada.");
        return;
      }
      currentSkills.push({ skill: { name }, level });
    }

    setFormData({ ...formData, studentSkills: currentSkills });
  };

  const selectedCount = (formData.studentSkills || []).filter((s) => (s.level || 0) > 0).length;

  const renderSkills = (skillsList) =>
    skillsList.map((skill) => {
      const currentSkill = formData.studentSkills?.find(
        (s) => s.skill.name === skill.name
      );
      const currentLevel = currentSkill?.level || 0;
      const disableNewSelection = selectedCount >= REQUIRED_SKILLS && currentLevel === 0;

      return (
        <div
          key={skill.name}
          className="flex items-center justify-between bg-gray-50 p-3 rounded-lg"
        >
          <span className="font-medium">{skill.name}</span>
          <StarRating
            max={5}
            value={currentLevel}
            onChange={(value) => handleSkillLevelChange(skill.name, value)}
            disabled={disableNewSelection}
          />
        </div>
      );
    });

  return (
    <div className="space-y-6">
      <h2 className="text-xl font-semibold text-gray-900">
        Habilidades e Competências
      </h2>

      {/* Texto informativo */}
      <div className="bg-blue-50 border border-blue-200 rounded-lg p-4">
        <p className="text-blue-800 text-sm">
          <strong>Instruções:</strong> Preencher 6 habilidades obrigatoriamente. 
          Caso selecione uma habilidade errada, clicar na estrela marcada que a mesma será desmarcada.
        </p>
      </div>

      {/* Contador visual */}
      <div className="bg-gray-100 rounded-lg p-4">
        <div className="flex justify-between items-center mb-2">
          <span className="font-medium text-gray-700">Habilidades selecionadas:</span>
          <span className={`font-bold ${selectedCount === REQUIRED_SKILLS ? 'text-green-600' : 'text-orange-500'}`}>
            {selectedCount}/{REQUIRED_SKILLS}
          </span>
        </div>
        <div className="w-full bg-gray-300 rounded-full h-2">
          <div 
            className={`h-2 rounded-full transition-all ${selectedCount === REQUIRED_SKILLS ? 'bg-green-500' : 'bg-orange-500'}`}
            style={{ width: `${(selectedCount / REQUIRED_SKILLS) * 100}%` }}
          />
        </div>
      </div>

      <FormField label="">
        <div className="text-lg font-bold text-gray-900 mb-2">Hard Skills</div>
        <div className="space-y-2">{renderSkills(skills.hardSkills)}</div>
      </FormField>

      <FormField label="">
        <div className="text-lg font-bold text-gray-900 mb-2">Soft Skills</div>
        <div className="space-y-2">{renderSkills(skills.softSkills)}</div>
      </FormField>

      <FormField
        label="Descreva um pouco das suas habilidades"
        required
      >
        <Input
          name="skillsDescription"
          onChange={handleChange}
          value={formData.skillsDescription || ""}
          placeholder="Descrição de Habilidades"
        />
      </FormField>
    </div>
  );
};

export default SkillsInfo;
