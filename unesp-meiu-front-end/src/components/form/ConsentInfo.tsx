import { FormField } from "@/components/form/FormField";
import { Input } from "@/components/ui/input";
import { Checkbox } from "@/components/ui/checkbox";
import { Label } from "@/components/ui/label";

const ConsentInfo = ({ formData, setFormData }) => {
  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      if (file.size > 5 * 1024 * 1024) { // 5MB in bytes
        alert("O arquivo deve ter no máximo 5MB");
        e.target.value = "";
        return;
      }
      if (file.type !== "application/pdf") {
        alert("Por favor, selecione um arquivo PDF");
        e.target.value = "";
        return;
      }
      setFormData({ ...formData, file_registration: file });
    }
  };

  const handleConsentChange = (checked) => {
    setFormData({ ...formData, agreed: checked });
  };

  return (
    <div className="space-y-6">
      <h2 className="text-xl font-semibold text-gray-900">
        Documentação e Consentimento
      </h2>

      <div className="space-y-6">
        <FormField
          label="Um único arquivo PDF contendo, nessa ordem: (1°) Histórico Escolar atualizado, obtido no Portal do Aluno (http://portal.utfpr.edu.br/secretaria). Certifique-se de que não há páginas cortadas e que apareça o link e data de acesso em todas as páginas. (2°) Ficha de Pontuação do Anexo III do Edital, preenchida. (3°) Documentação comprobatória referente aos pontos declarados na Ficha de Pontuação. Para juntar (merge) arquivos PDF, há serviços on-line. Por exemplo: https://smallpdf.com/pt/juntar-pdf. Nomear o arquivo PDF da seguinte maneira: Nome Completo do Estudante RA.pdf. Por exemplo: Sebastião Rodrigues Maia 1234567.pdf"
          required
          tooltip="Faça upload do seu documento de registro em formato PDF (máx. 5MB)"
        >
          <Input
            type="file"
            accept=".pdf"
            onChange={handleFileChange}
            className="cursor-pointer"
          />
        </FormField>

        <div className="flex items-start space-x-2">
          <Checkbox
            id="agreed"
            checked={formData.agreed || false}
            onCheckedChange={handleConsentChange}
            className="mt-1"
          />
          <Label
            htmlFor="agreed"
            className="text-sm text-gray-600 leading-relaxed"
          >
            Declaro ter conhecimento e aceitar as normas e condições previstas no Edital, responsabilizo-me pela exatidão e veracidade das informações prestadas e, caso selecionado, SOLICITO MINHA MATRÍCULA para o primeiro período letivo de 2026 na(s) disciplina(s) assinalada(s) neste formulário eletrônico de inscrição.
          </Label>
        </div>
      </div>
    </div>
  );
};

export default ConsentInfo;
