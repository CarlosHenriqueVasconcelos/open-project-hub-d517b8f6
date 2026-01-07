export interface Project {
  id?: number;
  description: string;
  internalCode: string;
  presencial: boolean;
}

export interface Skill {
  id?: number;
  name: string;
  tag: string;
  isSoftSkill: boolean;
}

export interface GeneralConfig {
  id?: number;
  config_header: string;
  config_body: string;
  config_consent: string;
  config_email_domain_avaliable: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  user: {
    id: number;
    email: string;
    name: string;
  };
}

export interface ApiResponse<T> {
  data: T;
  message?: string;
  success: boolean;
}

export interface Course {
  id: number;
  description: string;
}

export interface Student {
  id: number;
  name: string;
  ra: string;
  currentCourse?: Course;
}

export interface StudentSkill {
  skill: Skill;
  level: number;
}

export interface StudentRegistrationScore {
  performanceCoefficient: number;
  scientificInitiationProgramScore: number;
  institutionalMonitoringProgramScore: number;
  juniorEnterpriseExperienceScore: number;
}

export interface StudentRegistration {
  id: number;
  student: Student;
  semester: string;
  registrationDate: string;
  presencial: boolean;
  subject: number;
  studentSkills: StudentSkill[];
  studentRegistrationScore: StudentRegistrationScore;
  filePath?: string;
}
