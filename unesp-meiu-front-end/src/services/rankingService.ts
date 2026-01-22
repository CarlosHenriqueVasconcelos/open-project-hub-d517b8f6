import { API_CONFIG } from "@/config/api";

export interface RankingCourse {
  description?: string;
  mode?: string;
  period?: string;
  campus?: string;
}

export interface RankingStudent {
  id: number;
  name: string;
  ra: string;
  cpf?: string;
  rg?: string;
  cellphone?: string;
  email?: string;
  currentCourse?: RankingCourse;
}

export interface RankingEntry {
  id: number;
  studentRegistrationId: number;
  semester: string;
  subjectValue: number;
  rankPosition: number;
  classification: string;
  status: string;
  totalScore: number;
  performanceCoefficient: number;
  confirmBy?: string;
  registrationDate?: string;
  student?: RankingStudent;
}

export async function fetchRankings(subjectValue: number, semester: string): Promise<RankingEntry[]> {
  const response = await fetch(
    `${API_CONFIG.baseUrl}/rankings?subject=${subjectValue}&semester=${encodeURIComponent(semester)}`,
    {
      method: "GET",
      headers: API_CONFIG.headers,
    }
  );

  if (!response.ok) {
    throw new Error("Falha ao buscar classificação");
  }

  const data = await response.json();
  return data.data || [];
}

export async function updateRankingStatus(id: number, email: string, status: string): Promise<RankingEntry> {
  const response = await fetch(`${API_CONFIG.baseUrl}/rankings/${id}/status`, {
    method: "POST",
    headers: API_CONFIG.headers,
    body: JSON.stringify({ email, status }),
  });

  if (!response.ok) {
    const errorBody = await response.json().catch(() => ({}));
    throw new Error(errorBody?.message || "Falha ao atualizar status");
  }

  const data = await response.json();
  return data.data;
}
