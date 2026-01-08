import { Input } from "./ui/input";
import { Search } from "lucide-react";

interface StudentSearchProps {
  onSearch: (value: string) => void;
}

const StudentSearch = ({ onSearch }: StudentSearchProps) => {
  return (
    <div className="relative">
      <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
      <Input
        placeholder="Buscar por nome ou RA..."
        className="pl-10"
        onChange={(e) => onSearch(e.target.value)}
      />
    </div>
  );
};

export default StudentSearch;