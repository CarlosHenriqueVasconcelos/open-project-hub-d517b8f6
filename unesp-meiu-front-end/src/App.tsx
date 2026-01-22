import { Toaster } from "@/components/ui/toaster";
import { Toaster as Sonner } from "@/components/ui/sonner";
import { TooltipProvider } from "@/components/ui/tooltip";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Index from "./pages/Index";
import Form from "./pages/Form";
import Success from "./pages/Success";
import Verify from "./pages/Verify";
import Confirmation from "./pages/Confirmation";
import AdminPage from "../AdminPage/src/App"; // Importe o AdminPage

const queryClient = new QueryClient();

const RequireAuth = ({ children }: { children: JSX.Element }) => {
  const isAuthed = !!sessionStorage.getItem("userEmail");
  return isAuthed ? children : <Navigate to="/" replace />;
};

const App = () => (
  <QueryClientProvider client={queryClient}>
    <TooltipProvider>
      <Toaster />
      <Sonner />
      <BrowserRouter>
        <Routes>
          {/* Public Routes */}
          <Route path="/" element={<Index />} />
          <Route path="/verify" element={<Verify />} />
          <Route
            path="/form"
            element={
              <RequireAuth>
                <Form />
              </RequireAuth>
            }
          />
          <Route
            path="/success"
            element={
              <RequireAuth>
                <Success />
              </RequireAuth>
            }
          />
          <Route
            path="/confirmacao"
            element={
              <RequireAuth>
                <Confirmation />
              </RequireAuth>
            }
          />
          <Route path="/index" element={<Navigate to="/" replace />} />

          {/* admin Routes */}
          <Route path="/admin/*" element={<AdminPage />} /> {/* Rota para o AdminPage */}
        </Routes>
      </BrowserRouter>
    </TooltipProvider>
  </QueryClientProvider>
);

export default App;
