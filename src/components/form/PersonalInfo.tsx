import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { FormField } from "@/components/form/FormField";
import InputMask from "react-input-mask";

const PersonalInfo = ({ formData, setFormData }) => {
  const handleChange = (e) => {
    let rawValue = e.target.value;

    if (e.target.name != "name")
      rawValue = e.target.value.replace(/\D/g, ""); // Remove tudo que não for número
    
    setFormData({ ...formData, [e.target.name]: rawValue });
  };

  return (
    <div className="space-y-6">
      <h2 className="text-xl font-semibold text-gray-900">
        Informações Pessoais
      </h2>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <FormField
          label="Nome Completo"
          required
          tooltip="Digite seu nome completo como consta nos documentos oficiais"
        >
          <Input
            name="name"
            value={formData.name || ""}
            onChange={handleChange}
            placeholder="Nome Completo"
          />
        </FormField>

        <FormField label="Registro Acadêmico (RA)" required tooltip="Seu Registro Acadêmico">
          <Input
            name="ra"
            value={formData.ra || ""}
            onChange={handleChange}
            placeholder="1999999"
          />
        </FormField>

        <FormField
          label="CPF"
          required
          tooltip="Digite apenas os números do seu CPF"
        >
          <InputMask
            mask="999.999.999-99"
            value={formData.cpf || ""}
            onChange={handleChange}
          >
            {(inputProps) => (
              <Input
                {...inputProps}
                name="cpf"
                placeholder="123.456.789-00"
              />
            )}
          </InputMask>
        </FormField>

        <FormField label="RG" required>
        <Input
            name="rg"
            value={formData.rg || ""}
            onChange={handleChange}
            placeholder="11234123X"
          />
        </FormField>

        <FormField
          label="Celular"
          required
          tooltip="Digite seu número de celular com DDD"
        >
          <InputMask
            mask="(99) 99999-9999"
            value={formData.cellphone || ""}
            onChange={handleChange}
          >
            {(inputProps) => (
              <Input
                {...inputProps}
                name="cellphone"
                placeholder="(41) 99999-9999"
              />
            )}
          </InputMask>
        </FormField>

        <FormField
          label="Email"
          required
          tooltip="Seu email institucional @alunos.utfpr.edu.br"
        >
          <Input
            name="email"
            value={sessionStorage.getItem("userEmail")}
            onChange={handleChange}
            disabled // Campo imutável
          />
        </FormField>
      </div>
    </div>
  );
};

export default PersonalInfo;