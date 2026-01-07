
import axios from "axios";
import { API_BASE_URL } from "@admin/config/api";
import { LoginRequest, LoginResponse, ApiResponse, StudentRegistration, Skill, Project, GeneralConfig } from "@admin/types";

const api = axios.create({
  baseURL: API_BASE_URL,
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export const login = async (credentials: LoginRequest): Promise<LoginResponse> => {
  const response = await api.post<LoginResponse>("/accounts/login", credentials);
  return response.data;
};

export const getStudentRegistrations = async (pageNumber: number = 1, pageSize: number = 10): Promise<ApiResponse<StudentRegistration[]>> => {
  const response = await api.get<ApiResponse<StudentRegistration[]>>(`/student-registrations?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  return response.data;
};

export const getSkills = async (): Promise<ApiResponse<Skill[]>> => {
  const response = await api.get<ApiResponse<Skill[]>>("/skills");
  return response.data;
};

export const getSkillById = async (id: number): Promise<ApiResponse<Skill>> => {
  const response = await api.get<ApiResponse<Skill>>(`/skills/${id}`);
  return response.data;
};

export const createSkill = async (skill: Omit<Skill, "id">): Promise<ApiResponse<Skill>> => {
  const response = await api.post<ApiResponse<Skill>>("/skills", skill);
  return response.data;
};

export const updateSkill = async (id: number, skill: Omit<Skill, "id">): Promise<ApiResponse<Skill>> => {
  const response = await api.put<ApiResponse<Skill>>(`/skills/${id}`, skill);
  return response.data;
};

export const deleteSkill = async (id: number): Promise<void> => {
  await api.delete(`/skills/${id}`);
};

export const getProjects = async (): Promise<ApiResponse<Project[]>> => {
  const response = await api.get<ApiResponse<Project[]>>("/projects");
  return response.data;
};

export const getProjectById = async (id: number): Promise<ApiResponse<Project>> => {
  const response = await api.get<ApiResponse<Project>>(`/projects/${id}`);
  return response.data;
};

export const createProject = async (project: Omit<Project, "id">): Promise<ApiResponse<Project>> => {
  const response = await api.post<ApiResponse<Project>>("/projects", project);
  return response.data;
};

export const updateProject = async (id: number, project: Omit<Project, "id">): Promise<ApiResponse<Project>> => {
  const response = await api.put<ApiResponse<Project>>(`/projects/${id}`, project);
  return response.data;
};

export const deleteProject = async (id: number): Promise<void> => {
  await api.delete(`/projects/${id}`);
};

export const getGeneralConfigs = async (): Promise<ApiResponse<GeneralConfig[]>> => {
  const response = await api.get<ApiResponse<GeneralConfig[]>>("/general-configs");
  return response.data;
};

export const getGeneralConfigById = async (id: number): Promise<ApiResponse<GeneralConfig>> => {
  const response = await api.get<ApiResponse<GeneralConfig>>(`/general-configs/${id}`);
  return response.data;
};

export const createGeneralConfig = async (config: Omit<GeneralConfig, "id">): Promise<ApiResponse<GeneralConfig>> => {
  const response = await api.post<ApiResponse<GeneralConfig>>("/general-configs", config);
  return response.data;
};

export const updateGeneralConfig = async (id: number, config: Omit<GeneralConfig, "id">): Promise<ApiResponse<GeneralConfig>> => {
  const response = await api.put<ApiResponse<GeneralConfig>>(`/general-configs/${id}`, config);
  return response.data;
};

export const deleteGeneralConfig = async (id: number): Promise<void> => {
  await api.delete(`/general-configs/${id}`);
};
