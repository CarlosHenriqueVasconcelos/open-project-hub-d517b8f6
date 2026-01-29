import { API_CONFIG } from "@/config/api";

export interface GeneralConfig {
  configHeader?: string;
  configBody?: string;
  configEmailDomainAvaliable?: string;
  configConsent?: string;
  stage?: string;
  confirmationDeadline?: string;
  confirmationDeadlinePhase2?: string;
}

export async function fetchGeneralConfig(): Promise<GeneralConfig> {
  const response = await fetch(`${API_CONFIG.baseUrl}/general-configs`, {
    method: "GET",
    headers: API_CONFIG.headers,
  });

  if (!response.ok) {
    throw new Error("Falha ao carregar configurações");
  }

  const data = await response.json();
  return data.data || {};
}
