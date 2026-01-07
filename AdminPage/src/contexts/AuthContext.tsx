import React, { createContext, useContext, useState, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { login as apiLogin } from "@admin/services/api";
import { LoginRequest } from "@admin/types";
import { useToast } from "@admin/hooks/use-toast";

interface AuthContextType {
  isAuthenticated: boolean;
  login: (credentials: LoginRequest) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const navigate = useNavigate();
  const location = useLocation();
  const { toast } = useToast();

  // Check for existing session on mount
  useEffect(() => {
    const token = localStorage.getItem("token");
    if (token) {
      setIsAuthenticated(true);
      // Only redirect to dashboard if user is on login page or root path
      if (location.pathname === "/admin/login" || location.pathname === "/admin") {
        navigate("/admin/dashboard");
      }
    }
  }, [navigate, location.pathname]);

  const login = async (credentials: LoginRequest) => {
    try {
      const response = await apiLogin(credentials);
      localStorage.setItem("token", response.token);
      setIsAuthenticated(true);
      navigate("/Admin/dashboard");
      toast({
        title: "Logado com sucesso",
        description: "Bem vindo de volta!",
      });
    } catch (error) {
      toast({
        title: "Login falhou",
        description: "Por favor, verifique sua senha e tente novamente.",
        variant: "destructive",
      });
      throw error;
    }
  };

  const logout = () => {
    localStorage.removeItem("token");
    setIsAuthenticated(false);
    navigate("/Admin/Login");
    toast({
      title: "Deslogado",
      description: "Você foi deslogado com sucesso.",
    });
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};