import { useState } from "react";
import { useAuth } from "../contexts/AuthContext";
import { Button } from "../components/ui/button";
import { Input } from "../components/ui/input";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "../components/ui/card";

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const { login } = useAuth();
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    console.log("Chegando aqui");
    e.preventDefault();
    setIsLoading(true);
    try {
      await login({ email, password });
    } catch (error) {
      console.error("Login falhou:", error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 px-4">
      <Card className="w-full max-w-md animate-fadeIn bg-white shadow-lg">
        <CardHeader className="space-y-1 border-b border-gray-200 bg-secondary/30 p-6">
          <CardTitle className="text-2xl font-bold text-primary text-center">Sistema de Habilidades</CardTitle>
          <CardDescription className="text-center text-gray-600">
            Universidade Tecnológica Federal do Paraná
          </CardDescription>
        </CardHeader>
        <CardContent className="p-6">
          <form onSubmit={handleSubmit} className="space-y-4">
            <div className="space-y-2">
              <Input
                type="email"
                placeholder="Email institucional"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
                className="w-full border-gray-300 focus:border-primary focus:ring-primary"
              />
            </div>
            <div className="space-y-2">
              <Input
                type="password"
                placeholder="Senha"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
                className="w-full border-gray-300 focus:border-primary focus:ring-primary"
              />
            </div>
            <Button
              type="submit"
              className="w-full bg-primary hover:bg-primary/90 text-white font-semibold"
              disabled={isLoading}
            >
              {isLoading ? "Entrando..." : "Entrar"}
            </Button>
          </form>
        </CardContent>
      </Card>
    </div>
  );
};

export default Login;