import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { Routes, Route, Navigate } from "react-router-dom";
import { Toaster } from "./components/ui/toaster";
import { AuthProvider, useAuth } from "./contexts/AuthContext";
import { SidebarProvider, SidebarTrigger } from "./components/ui/sidebar";
import { AppSidebar } from "./components/AppSidebar";
import { useEffect } from "react";
import Login from "./pages/Login";
import Dashboard from "./pages/Dashboard";
import Skills from "./pages/Skills";
import Projects from "./pages/Projects";
import ProjectSkills from "./pages/ProjectSkills";
import GeneralConfigs from "./pages/Configs";
import Rankings from "./pages/Rankings";

const queryClient = new QueryClient();

const PrivateRoute = ({ children }: { children: React.ReactNode }) => {
  const { isAuthenticated } = useAuth();

  useEffect(() => {
    const loadFont = async () => {
      const font = new FontFace(
        'Open Sans',
        'url(https://fonts.gstatic.com/s/opensans/v34/memvYaGs126MiZpBA-UvWbX2vVnXBbObj2OVTS-mu0SC55I.woff2)'
      );

      try {
        await font.load();
        document.fonts.add(font);
        console.log('Open Sans font loaded successfully');
      } catch (error) {
        console.error('Error loading Open Sans font:', error);
      }
    };

    loadFont();
  }, []);

  return isAuthenticated ? (
    <SidebarProvider>
      <div className="flex min-h-screen w-full">
        <AppSidebar />
        <main className="flex-1 p-6">
          <SidebarTrigger className="mb-4" />
          {children}
        </main>
      </div>
    </SidebarProvider>
  ) : (
    <Navigate to="/Admin/Login" /> // Ajuste o caminho para /admin/login
  );
};

const App = () => {
  return (
    <QueryClientProvider client={queryClient}>
      <AuthProvider>
        <Routes>
          <Route path="/Login" element={<Login />} />
          <Route
            path="/dashboard"
            element={
              <PrivateRoute>
                <Dashboard />
              </PrivateRoute>
            }
          />
          <Route
            path="/skills"
            element={
              <PrivateRoute>
                <Skills />
              </PrivateRoute>
            }
          />
          <Route
            path="/projects"
            element={
              <PrivateRoute>
                <Projects />
              </PrivateRoute>
            }
          />
          <Route
            path="/project-skills"
            element={
              <PrivateRoute>
                <ProjectSkills />
              </PrivateRoute>
            }
          />
          <Route
            path="/configs"
            element={
              <PrivateRoute>
                <GeneralConfigs />
              </PrivateRoute>
            }
          />
          <Route
            path="/rankings"
            element={
              <PrivateRoute>
                <Rankings />
              </PrivateRoute>
            }
          />
          <Route path="/" element={<Navigate to="/admin/dashboard" replace />} />
        </Routes>
        <Toaster />
      </AuthProvider>
    </QueryClientProvider>
  );
};

export default App;
