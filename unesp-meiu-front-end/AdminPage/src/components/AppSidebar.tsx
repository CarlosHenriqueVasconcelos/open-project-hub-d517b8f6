import { Home, BookOpen, Boxes, BoxesIcon, PackageCheck } from "lucide-react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../contexts/AuthContext";
import {
  Sidebar,
  SidebarContent,
  SidebarGroup,
  SidebarGroupContent,
  SidebarGroupLabel,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarFooter,
} from "./ui/sidebar";
import { Button } from "./ui/button";

const menuItems = [
  {
    title: "Painel",
    path: "/admin/dashboard",
    icon: Home,
  },
  {
    title: "Habilidades",
    path: "/admin/skills",
    icon: BookOpen,
  },
  {
    title: "Projetos",
    path: "/admin/projects",
    icon: Boxes,
  },
  {
    title: "Habilidades do Projeto",
    path: "/admin/project-skills",
    icon: PackageCheck,
  },
  {
    title: "Configurações Gerais",
    path: "/admin/configs",
    icon: PackageCheck,
  }
];

export function AppSidebar() {
  const navigate = useNavigate();
  const { logout } = useAuth();

  return (
    <Sidebar>
      <SidebarContent>
        <SidebarGroup>
          <SidebarGroupLabel>Navegação</SidebarGroupLabel>
          <SidebarGroupContent>
            <SidebarMenu>
              {menuItems.map((item) => (
                <SidebarMenuItem key={item.title}>
                  <SidebarMenuButton
                    onClick={() => navigate(item.path)}
                    tooltip={item.title}
                  >
                    <item.icon className="h-4 w-4" />
                    <span>{item.title}</span>
                  </SidebarMenuButton>
                </SidebarMenuItem>
              ))}
            </SidebarMenu>
          </SidebarGroupContent>
        </SidebarGroup>
      </SidebarContent>
      <SidebarFooter>
        <Button
          variant="ghost"
          className="w-full justify-start"
          onClick={logout}
        >
          Sair
        </Button>
      </SidebarFooter>
    </Sidebar>
  );
}