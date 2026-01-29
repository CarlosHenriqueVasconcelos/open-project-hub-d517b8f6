import { FormField } from "@/components/form/FormField";
import { useState, useEffect } from "react";

const Registration = ({ formData, setFormData }) => {
    const [selectedSubjects, setSelectedSubjects] = useState(formData.subjects || []);

    useEffect(() => {
        if (formData.subjects && formData.subjects.length > 0) {
            setSelectedSubjects(formData.subjects);
        }
    }, [formData.subjects]);

    const MAX_SUBJECTS = 2;
    const CONFLICTS = {
        1: [32],
        32: [1],
    };

    const handleCheckboxChange = (event) => {
        const { value, checked } = event.target;
        const parsedValue = parseInt(value, 10);

        let updatedSubjects = [...selectedSubjects];
        if (checked) {
            if (updatedSubjects.length >= MAX_SUBJECTS) {
                alert("Você pode selecionar no máximo duas matérias.");
                return;
            }

            const conflicts = CONFLICTS[parsedValue] || [];
            if (conflicts.some((conflict) => updatedSubjects.includes(conflict))) {
                alert("Essas duas matérias ocorrem no mesmo dia e não podem ser selecionadas juntas.");
                return;
            }
            updatedSubjects.push(parsedValue);
        } else {
            updatedSubjects = updatedSubjects.filter((v) => v !== parsedValue);
        }

        setSelectedSubjects(updatedSubjects);
        setFormData((prevData) => ({
            ...prevData,
            subjects: updatedSubjects,
            subject: updatedSubjects.reduce((acc, curr) => acc + curr, 0),
        }));
    };

    useEffect(() => {
        // Atualiza o campo subject com a soma dos valores selecionados
        setFormData((prev) => ({
            ...prev,
            subject: selectedSubjects.reduce((acc, curr) => acc + curr, 0),
        }));
    }, [selectedSubjects]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        const normalizedValue =
            name === "cursarUmaOuDuas" ? value === "true" : value;
        // Atualiza o estado para refletir a seleção do rádio
        setFormData((prevData) => ({
            ...prevData,
            [name]: normalizedValue, // Atualiza o valor selecionado para o campo
        }));
    };

    const calculateSemester = () => {
        const now = new Date();
        const year = now.getFullYear(); // Ano atual
        const month = now.getMonth() + 1; // Mês atual (1 a 12)

        // Se o mês é entre janeiro e junho, é o primeiro semestre (ex: 2024-1)
        // Caso contrário, é o segundo semestre (ex: 2024-2)
        const semester = month <= 6 ? `${year}-1` : `${year}-2`;

        return semester;
    };

    useEffect(() => {
        const semester = calculateSemester();
        const email = sessionStorage.getItem("userEmail")
        setFormData((prevData) => ({
            ...prevData,
            semester, // Adiciona o semestre ao formData
            email 
        }));
    }, [setFormData]);

    return (
        <div className="space-y-6">
            <h2 className="text-xl font-semibold text-gray-900">
                Informações sobre a Matrícula da Disciplina
            </h2>

            <div className="grid grid-cols-1 md:grid-cols-1 gap-6">
                <FormField
                    label="Selecionar até duas matérias. Não é permitido selecionar Engineering Design Process e Manutenção 4.0 - Desafios Colaborativos ao mesmo tempo."
                    required
                >
                    <div className="space-y-2">
                        {[
                            { id: "Engineering", label: "Engineering Design Process (ENG01B) - Campus Ponta Grossa ( Segunda-feira de 18h40 às 20h20(2N1, 2N2)", value: "1" },
                            { id: "Industry", label: "Industry 4.0 e 5.0 (ENG01C) - Campus Ponta Grossa (Quarta-feira de 18h40 às 20h20(4N1, 4N2))", value: "4" },
                            { id: "Design", label: "Design de Soluções para Problemas Reais (DPR01MEIU) - Campus Apucarana  (Quinta-feira de 13h50 às 15h30 (5T2,5T3))", value: "2" },
                            { id: "Engenharia", label: "Engenharia Colaborativa (OPETH003) - Campus Toledo (Terça-feira de 18h40 às 20h20(3N1, 3N2))", value: "8" },
                            { id: "Processo", label: "Processo de Projeto em Engenharia (OP69B) - Campus Londrina (Sexta-feira de 13h50 às 15h30(6T2, 6T3))", value: "16" },
                            { id: "Manutencao", label: "Manutenção 4.0 - Desafios Colaborativos (DC46M) - Campus Pato Branco (Segunda-feira de 18h40 às 20h20 (2N1, 2N2))", value: "32" },
                        ].map((subject) => (
                            <div key={subject.id} className="flex items-center space-x-2">
                                <input
                                    type="checkbox"
                                    id={subject.id}
                                    value={subject.value}
                                    checked={selectedSubjects.includes(parseInt(subject.value, 10))}
                                    onChange={handleCheckboxChange}
                                    disabled={
                                        selectedSubjects.length >= MAX_SUBJECTS &&
                                        !selectedSubjects.includes(parseInt(subject.value, 10))
                                    }
                                    className="h-4 w-4"
                                />
                                <label htmlFor={subject.id} className="text-sm text-gray-700">
                                    {subject.label}
                                </label>
                            </div>
                        ))}
                    </div>
                </FormField>


                <FormField
                    label="Escolher prioridade - Selecionar Matéria de maior prioridade, indepentende de ter selecionado uma ou duas matérias para candidatura"
                >
                    <div className="space-y-2">
                        {[
                            { id: "EngineeringchoicePriority", label: "Engineering Design Process (ENG01B) - Campus Ponta Grossa ", value: "1" },
                            { id: "Industry4", label: "Industry 4.0 e 5.0 (ENG01C) - Campus Ponta Grossa ", value: "2" },
                            { id: "DesignchoicePriority", label: "Design de Soluções para Problemas Reais (DPR01MEIU) - Campus Apucarana", value: "3" },
                            { id: "EngColaborativa", label: "Engenharia Colaborativa (OPETH003) - Campus Toledo ", value: "4" },
                            { id: "ProcessochoicePriority", label: "Processo de Projeto em Engenharia (OP69B) - Campus Londrina ", value: "5" },
                            { id: "ManutencaochoicePriority", label: "Manutenção 4.0 - Desafios Colaborativos (DC46M) - Campus Pato Branco ", value: "6" },
                        ].map((choicePriority_type) => (
                            <div key={choicePriority_type.id} className="flex items-center space-x-2">
                                <input
                                    type="radio"
                                    id={choicePriority_type.id}
                                    name="choicePriority" // Todos têm o mesmo 'name' para que apenas um seja selecionado
                                    value={choicePriority_type.value}
                                    checked={formData.choicePriority === choicePriority_type.value} // Verifica se a opção foi selecionada
                                    onChange={handleChange} // Atualiza o estado com a opção selecionada
                                    className="h-4 w-4"
                                />
                                <label htmlFor={choicePriority_type.id} className="text-sm text-gray-700">
                                    {choicePriority_type.label}
                                </label>
                            </div>
                        ))}
                    </div>
                </FormField>

                <FormField
                    label="Caso tenha selecionado mais de uma matéria para candidatura, tem interesse de cursar ambas ?"
                    required
                >
                    <div className="space-y-2">
                        {[
                            { id: "cursarUmaOuDuasSim", label: "Sim", value: "true" },
                            { id: "cursarUmaOuDuasNao", label: "Não", value: "false" },
                        ].map((option) => (
                            <div key={option.id} className="flex items-center space-x-2">
                                <input
                                    type="radio"
                                    id={option.id}
                                    name="cursarUmaOuDuas"
                                    value={option.value}
                                    checked={formData.cursarUmaOuDuas === (option.value === "true")}
                                    onChange={handleChange}
                                    className="h-4 w-4"
                                />
                                <label htmlFor={option.id} className="text-sm text-gray-700">
                                    {option.label}
                                </label>
                            </div>
                        ))}
                    </div>
                </FormField>

            </div>
        </div>
    );
};

export default Registration;
