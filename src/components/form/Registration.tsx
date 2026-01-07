import { FormField } from "@/components/form/FormField";
import { useState, useEffect } from "react";

const Registration = ({ formData, setFormData }) => {
    const [selectedSubjects, setSelectedSubjects] = useState(formData.subjects || []);

    useEffect(() => {
        if (formData.subjects && formData.subjects.length > 0) {
            setSelectedSubjects(formData.subjects);
        }
    }, [formData.subjects]);

    const handleCheckboxChange = (event, group) => {
        const { value, checked } = event.target;
        const parsedValue = parseInt(value, 10);

        let updatedSubjects = [...selectedSubjects];
        if (checked) {
            if (group === "exclusive") {
                updatedSubjects = updatedSubjects.filter((v) => v !== 1 && v !== 2);
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
        // Atualiza o estado para refletir a seleção do rádio
        setFormData((prevData) => ({
            ...prevData,
            [name]: value, // Atualiza o valor selecionado para o campo 'subject'
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
                    label="Selecionar até três matérias, escolha apenas uma entre as matérias: Engineering Design Process e Design de Soluções para Problemas Reais"
                    required
                >
                    <div className="space-y-2">
                        {[
                            { id: "Engineering", label: "Engineering Design Process", value: "1", group: "exclusive" },
                            { id: "Design", label: "Design de Soluções Reais", value: "2", group: "exclusive" },
                            { id: "Industry", label: "Industry 4.0 e 5.0", value: "4", group: "general" },
                            { id: "Engenharia", label: "Engenharia Colaborativa", value: "8", group: "general" },
                        ].map((subject) => (
                            <div key={subject.id} className="flex items-center space-x-2">
                                <input
                                    type="checkbox"
                                    id={subject.id}
                                    value={subject.value}
                                    checked={selectedSubjects.includes(parseInt(subject.value, 10))}
                                    onChange={(event) => handleCheckboxChange(event, subject.group)}
                                    disabled={
                                        selectedSubjects.length >= 3 &&
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
                    label="Escolher prioridade - Caso tenha escolhido concorrer em Ambas as disciplinas na questão acima, escolha a sua preferência caso seja selecionado nas duas."
                >
                    <div className="space-y-2">
                        {[
                            { id: "EngineeringchoicePriority", label: "Engineering Design Process", value: "1" },
                            { id: "DesignchoicePriority", label: "Design de Soluções Reais", value: "2" },
                            { id: "Industry4", label: "Industry 4.0 e 5.0", value: "3" },
                            { id: "EngColaborativa", label: "Engenharia Colaborativa", value: "4" },
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
                    label="O Regulamento da Organização Didático-Pedagógica dos Cursos De Graduação do UTFPR permite: (i) matrícula em até 3 disciplinas de enriquecimento curricular, (ii) carga horária máxima igual a do período com maior carga horária na matriz curricular, somada a 90 (noventa) horas, não excedendo um máximo de 600 (seiscentas) horas no semestre, (iii) matrícula em disciplinas cujos horários não sejam sobrepostos. Caso você seja selecionado no presente edital mas não atenda a um dos requisitos (i), (ii) ou (iii), a sua opção é:"
                >
                    <div className="space-y-2">
                        {[
                            { id: "doesNotMeetRequirements1", label: "Desistência da matrícula em uma ou mais disciplinas nas quais foi selecionado no presente edital", value: "0" },
                            { id: "doesNotMeetRequirements2", label: "Cancelamento da matrícula em uma ou mais disciplinas em que se encontra matriculado, para que seja possível a matrícula em uma ou mais disciplinas nas quais foi selecionado no presente edital", value: "1" },
                        ].map((doesNotMeetRequirements_type) => (
                            <div key={doesNotMeetRequirements_type.id} className="flex items-center space-x-2">
                                <input
                                    type="radio"
                                    id={doesNotMeetRequirements_type.id}
                                    name="doesNotMeetRequirements" // Todos têm o mesmo 'name' para que apenas um seja selecionado
                                    value={doesNotMeetRequirements_type.value}
                                    checked={formData.doesNotMeetRequirements === doesNotMeetRequirements_type.value} // Verifica se a opção foi selecionada
                                    onChange={handleChange} // Atualiza o estado com a opção selecionada
                                    className="h-4 w-4"
                                />
                                <label htmlFor={doesNotMeetRequirements_type.id} className="text-sm text-gray-700">
                                    {doesNotMeetRequirements_type.label}
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