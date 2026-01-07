import { API_CONFIG } from "@/config/api";

export interface StudentRegistration {
  id: number;
  student: {
    id: number;
    name: string;
    ra: string;
    currentCourse?: {
      id: number;
      description: string;
    };
    user?: {
      email: string;
    };
  };
  semester: string;
  registrationDate: string;
  presencial: boolean;
  subject: number;
  filePath?: string;
}

export async function fetchAllRegistrations(): Promise<StudentRegistration[]> {
  const response = await fetch(`${API_CONFIG.baseUrl}/student-registrations`, {
    method: "GET",
    headers: API_CONFIG.headers,
  });

  if (!response.ok) {
    throw new Error("Falha ao buscar inscrições");
  }

  const data = await response.json();
  return data.data || [];
}

export async function checkRegistrationByEmail(email: string): Promise<StudentRegistration | null> {
  const registrations = await fetchAllRegistrations();
  
  // Filtra pela inscrição mais recente do email
  const userRegistrations = registrations.filter(
    (reg) => reg.student?.user?.email?.toLowerCase() === email.toLowerCase()
  );

  if (userRegistrations.length === 0) {
    return null;
  }

  // Retorna a mais recente (ordenada por data de inscrição)
  return userRegistrations.sort(
    (a, b) => new Date(b.registrationDate).getTime() - new Date(a.registrationDate).getTime()
  )[0];
}

export async function downloadRegistrationPDF(registrationId: number): Promise<Blob> {
  const response = await fetch(
    `${API_CONFIG.baseUrl}/student-registrations/file/${registrationId}`,
    {
      method: "GET",
      headers: API_CONFIG.headers,
    }
  );

  if (!response.ok) {
    throw new Error("Falha ao baixar comprovante");
  }

  return response.blob();
}
