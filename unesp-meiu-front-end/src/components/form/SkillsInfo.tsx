import { useState, useEffect } from "react";
import { Input } from "@/components/ui/input";
import { FormField } from "@/components/form/FormField";
import { StarRating } from "../ui/star-rating";
import { API_CONFIG } from "@/config/api";

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
      if (isActivating && selectedCount >= 5) {
        alert("Você pode selecionar no máximo 5 habilidades.");
        return;
      }
      currentSkills[existingSkillIndex].level = level;
    } else {
      if (level > 0 && selectedCount >= 5) {
        alert("Você pode selecionar no máximo 5 habilidades.");
        return;
      }
      currentSkills.push({ skill: { name }, level });
    }

    setFormData({ ...formData, studentSkills: currentSkills });
  };

  const renderSkills = (skillsList) =>
    skillsList.map((skill) => {
      const currentSkill = formData.studentSkills?.find(
        (s) => s.skill.name === skill.name
      );
      const currentLevel = currentSkill?.level || 0;
      const selectedCount = (formData.studentSkills || []).filter((s) => (s.level || 0) > 0).length;
      const disableNewSelection = selectedCount >= 5 && currentLevel === 0;

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

      <FormField label="">
        <div className="text-2xl font-bold text-gray-900">Hard Skills (Preencher ao menos um campo)</div>
        <div className="space-y-2">{renderSkills(skills.hardSkills)}</div>
      </FormField>

      <FormField label="">
        <div className="text-2xl font-bold text-gray-900"></div>
        <div className="space-y-2">{renderSkills(skills.softSkills)}</div>
      </FormField>

      <FormField
          label="Descreva um pouco das suas habilidades"
          required>
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
