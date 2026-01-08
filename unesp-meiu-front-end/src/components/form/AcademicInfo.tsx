import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { FormField } from "@/components/form/FormField";

const AcademicInfo = ({ formData, setFormData }) => {
  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSelectChange = (name, value) => {
    setFormData({ ...formData, [name]: value });
  };

  return (
    <div className="space-y-6">
      <h2 className="text-xl font-semibold text-gray-900">
        Informações Acadêmicas
      </h2>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <FormField
          label="Modalidade do Curso"
          required
          tooltip="Selecione a modalidade do seu curso atual"
        >
          <Select
            value={formData.courseMode || ""}
            onValueChange={(value) => handleSelectChange("courseMode", value)}
          >
            <SelectTrigger>
              <SelectValue placeholder="Modalidade do curso em que se encontra matriculado" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="engenharia">Engenharia</SelectItem>
              <SelectItem value="bacharelado">Bacharelado</SelectItem>
              <SelectItem value="tecnologia">Tecnologia</SelectItem>
              <SelectItem value="licenciatura">Licenciatura</SelectItem>
            </SelectContent>
          </Select>
        </FormField>

        <FormField
          label="Nome do curso em que se encontra matriculado"
          required
          tooltip="Digite o nome completo do seu curso"
        >
          <Input
            name="courseDescription"
            value={formData.courseDescription || ""}
            onChange={handleChange}
            placeholder="Engenharia de Software"
          />
        </FormField>

        <FormField
          label="Período"
          required
          tooltip="Em qual período você está atualmente"
        >
          <Select
            value={formData.period || ""}
            onValueChange={(value) => handleSelectChange("period", value)}
          >
            <SelectTrigger>
              <SelectValue placeholder="Período em que se encontra no curso" />
            </SelectTrigger>
            <SelectContent>
              {[1, 2, 3, 4, 5, 6, 7, 8, 9, 10].map((period) => (
                <SelectItem key={period} value={period.toString()}>
                  {period}º Período
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </FormField>

        <FormField
          label="Campus"
          required
          tooltip="Selecione seu campus atual"
        >
          <Select
            value={formData.campus || ""}
            onValueChange={(value) => handleSelectChange("campus", value)}
          >
            <SelectTrigger>
              <SelectValue placeholder="Campus" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="Apucarana">Apucarana</SelectItem>
              <SelectItem value="CampoMourao">Campo Mourão</SelectItem>
              <SelectItem value="CornelioProcopio">Cornélio Procópio</SelectItem>
              <SelectItem value="Curitba">Curitiba</SelectItem>
              <SelectItem value="DoisVizinhos">Dois Vizinhos</SelectItem>
              <SelectItem value="FranciscoBeltrao">Francisco Beltrão</SelectItem>
              <SelectItem value="Guarapuava">Guarapuava</SelectItem>
              <SelectItem value="Londrina">Londrina</SelectItem>
              <SelectItem value="Medianeira">Medianeira</SelectItem>
              <SelectItem value="PatoBranco">Pato Branco</SelectItem>
              <SelectItem value="PontaGrossa">Ponta Grossa</SelectItem>
              <SelectItem value="SantaHelena">Santa Helena</SelectItem>
              <SelectItem value="Toledo">Toledo</SelectItem>
              
            </SelectContent>
          </Select>
        </FormField>
      </div>
    </div>
  );
};

export default AcademicInfo;