
import { useState, useEffect } from "react";
import { API_CONFIG } from "@/config/api";

const Introduction = ({ formData, setFormData }) => {
  const [data, setData] = useState({ configHeader: "", configBody: "" });

  const fetchGeneralConfigs = async () => {
    try {
      const response = await fetch(`${API_CONFIG.baseUrl}/general-configs/`, {
        method: "GET",
        headers: API_CONFIG.headers
      });

      if (!response.ok) {
        throw new Error("Falha ao carregar as configurações gerais");
      }

      const responseData = await response.json();
      console.log("Dados recebidos:", responseData.data);

      setData({
        configHeader: responseData.data.configHeader || "Título Padrão",
        configBody: responseData.data.configBody || "Descrição Padrão",
      });

      setFormData({
        ...formData,
        configHeader: responseData.data.configHeader,
        configBody: responseData.data.configBody,
      });
    } catch (error) {
      console.error("Erro ao buscar configurações gerais:", error.message);
    }
  };

  useEffect(() => {
    fetchGeneralConfigs();
  }, []);

  return (
    <div className="space-y-6">
      <h1
        className="text-xl font-semibold text-gray-900"
        dangerouslySetInnerHTML={{ __html: data.configHeader }}
      />
      <h2 dangerouslySetInnerHTML={{ __html: data.configBody }} />
    </div>
  );
};

export default Introduction;
