
## Plano: Limite de 6 Habilidades com Toggle e Validacao

### Objetivo
Modificar a etapa de habilidades do formulario para:
1. Exigir exatamente 6 habilidades selecionadas
2. Permitir desmarcar habilidade clicando na estrela marcada
3. Adicionar texto informativo e contador visual
4. Validar antes de avancar para proxima etapa

---

### Arquivos a Modificar

| Arquivo | Alteracao |
|---------|-----------|
| `unesp-meiu-front-end/src/components/ui/star-rating.tsx` | Adicionar logica de toggle |
| `unesp-meiu-front-end/src/components/form/SkillsInfo.tsx` | Limite 6, contador, texto informativo |
| `unesp-meiu-front-end/src/pages/Form.tsx` | Validacao na etapa 5 |

---

### Alteracao 1: star-rating.tsx

Adicionar logica de toggle - clicar na mesma estrela desmarca:

```tsx
export const StarRating = ({ max, value, onChange, disabled = false }) => {
  const stars = Array.from({ length: max }, (_, index) => index + 1);

  const handleClick = (star: number) => {
    if (disabled) return;
    
    // Toggle: se clicar na mesma estrela, desmarca (volta para 0)
    if (star === value) {
      onChange(0);
    } else {
      onChange(star);
    }
  };

  return (
    <div className="flex items-center">
      {stars.map((star) => (
        <button
          key={star}
          onClick={() => handleClick(star)}
          disabled={disabled}
          className={`h-6 w-6 transition-colors ${
            star <= value ? "text-yellow-500" : "text-gray-300"
          } ${disabled ? "opacity-50 cursor-not-allowed" : "hover:text-yellow-400"}`}
        >
          ★
        </button>
      ))}
    </div>
  );
};
```

---

### Alteracao 2: SkillsInfo.tsx

Mudancas principais:
- Limite alterado de 5 para 6
- Texto informativo no topo
- Contador visual de habilidades selecionadas
- Barra de progresso visual

```tsx
const SkillsInfo = ({ formData, setFormData }) => {
  const [skills, setSkills] = useState({ softSkills: [], hardSkills: [] });
  const REQUIRED_SKILLS = 6;
  
  // ... fetchSkills permanece igual ...

  const handleSkillLevelChange = (name, level) => {
    const currentSkills = [...(formData.studentSkills || [])];
    const existingSkillIndex = currentSkills.findIndex((s) => s.skill.name === name);
    const selectedCount = currentSkills.filter((s) => (s.level || 0) > 0).length;

    if (existingSkillIndex > -1) {
      const prevLevel = currentSkills[existingSkillIndex].level || 0;
      const isActivating = prevLevel === 0 && level > 0;
      
      if (isActivating && selectedCount >= REQUIRED_SKILLS) {
        alert("Voce ja selecionou 6 habilidades. Para adicionar outra, desmarque uma existente clicando na estrela marcada.");
        return;
      }
      currentSkills[existingSkillIndex].level = level;
    } else {
      if (level > 0 && selectedCount >= REQUIRED_SKILLS) {
        alert("Voce ja selecionou 6 habilidades. Para adicionar outra, desmarque uma existente clicando na estrela marcada.");
        return;
      }
      currentSkills.push({ skill: { name }, level });
    }

    setFormData({ ...formData, studentSkills: currentSkills });
  };

  // Calcular contagem de selecionadas
  const selectedCount = (formData.studentSkills || []).filter((s) => (s.level || 0) > 0).length;

  return (
    <div className="space-y-6">
      <h2 className="text-xl font-semibold text-gray-900">
        Habilidades e Competencias
      </h2>

      {/* Texto informativo */}
      <div className="bg-blue-50 border border-blue-200 rounded-lg p-4">
        <p className="text-blue-800 text-sm">
          <strong>Instrucoes:</strong> Preencher 6 habilidades obrigatoriamente. 
          Caso selecione uma habilidade errada, clicar na estrela marcada que a mesma sera desmarcada.
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

      {/* Hard Skills */}
      <FormField label="">
        <div className="text-lg font-bold text-gray-900 mb-2">Hard Skills</div>
        <div className="space-y-2">{renderSkills(skills.hardSkills)}</div>
      </FormField>

      {/* Soft Skills */}
      <FormField label="">
        <div className="text-lg font-bold text-gray-900 mb-2">Soft Skills</div>
        <div className="space-y-2">{renderSkills(skills.softSkills)}</div>
      </FormField>

      {/* Descricao */}
      <FormField label="Descreva um pouco das suas habilidades" required>
        <Input
          name="skillsDescription"
          onChange={handleChange}
          value={formData.skillsDescription || ""}
          placeholder="Descricao de Habilidades"
        />
      </FormField>
    </div>
  );
};
```

---

### Alteracao 3: Form.tsx

Adicionar validacao na etapa 5 antes de avancar:

```tsx
const handleNext = () => {
  if (step < totalSteps) {
    // Validacao da etapa 5 (Habilidades)
    if (step === 5) {
      const selectedSkills = (formData.studentSkills || []).filter(
        (s) => (s.level || 0) > 0
      );
      
      if (selectedSkills.length !== 6) {
        alert(`Voce precisa selecionar exatamente 6 habilidades para continuar. Atualmente: ${selectedSkills.length}/6`);
        toast({
          variant: "destructive",
          title: "Habilidades insuficientes",
          description: `Selecione exatamente 6 habilidades. Voce selecionou ${selectedSkills.length}.`,
        });
        return;
      }
    }

    // Validacao da etapa 6 (Scores) - ja existente
    if (step === 6) {
      // ... codigo existente ...
    }

    setStep(step + 1);
    window.scrollTo(0, 0);
  }
};
```

---

### Interface Visual Final

```
+--------------------------------------------------+
|  Habilidades e Competencias                      |
|                                                  |
|  +--------------------------------------------+  |
|  | Instrucoes: Preencher 6 habilidades        |  |
|  | obrigatoriamente. Caso selecione uma       |  |
|  | habilidade errada, clicar na estrela       |  |
|  | marcada que a mesma sera desmarcada.       |  |
|  +--------------------------------------------+  |
|                                                  |
|  Habilidades selecionadas:              4/6     |
|  [=========>              ]                      |
|                                                  |
|  Hard Skills                                     |
|  +--------------------------------------------+  |
|  | Python                    *****            |  |
|  | JavaScript                ***              |  |
|  | Machine Learning          ****             |  |
|  | SQL                       **               |  |
|  +--------------------------------------------+  |
|                                                  |
|  Soft Skills                                     |
|  +--------------------------------------------+  |
|  | Comunicacao               ****             |  |
|  | Trabalho em Equipe        ***              |  |
|  +--------------------------------------------+  |
|                                                  |
+--------------------------------------------------+
```

---

### Fluxo de Interacao

**Selecionar habilidade:**
1. Usuario clica em estrela 3 de Python
2. Python recebe level = 3
3. Contador atualiza: 1/6

**Desmarcar habilidade (Toggle):**
1. Python tem level = 3
2. Usuario clica na estrela 3 novamente
3. Python recebe level = 0
4. Contador atualiza: 0/6

**Tentar selecionar 7a habilidade:**
1. Usuario ja tem 6 habilidades
2. Clica em nova habilidade
3. Alert aparece + botao fica desabilitado

**Tentar avancar sem 6 habilidades:**
1. Usuario tem 4 habilidades
2. Clica em "Proximo"
3. Alert + Toast de erro
4. Permanece na etapa 5

---

### Resumo das Alteracoes

| Arquivo | Linhas Modificadas | Descricao |
|---------|-------------------|-----------|
| `star-rating.tsx` | 9-10 | Logica de toggle |
| `SkillsInfo.tsx` | 53-68, 99-114 | Limite 6, contador, texto |
| `Form.tsx` | 168-200 | Validacao etapa 5 |
