import { useState } from "react";
import { Dialog, DialogContent, DialogTrigger } from "./ui/dialog";
import { Button } from "./ui/button";
import { FileText } from "lucide-react";
import { API_BASE_URL } from "../config/api";

interface PDFViewerProps {
  pdfUrl?: string;
}

const PDFViewer = ({ pdfUrl }: PDFViewerProps) => {
  const [isOpen, setIsOpen] = useState(false);
  pdfUrl = `${API_BASE_URL}/${pdfUrl}`;
  if (!pdfUrl) return null;

  return (
    <Dialog open={isOpen} onOpenChange={setIsOpen}>
      <DialogTrigger asChild>
        <Button variant="outline" size="sm" className="gap-2">
          <FileText className="h-4 w-4" />
          Ver PDF
        </Button>
      </DialogTrigger>
      <DialogContent className="max-w-4xl h-[80vh]">
        <iframe
          src={pdfUrl}
          className="w-full h-full"
          title="PDF Viewer"
        />
      </DialogContent>
    </Dialog>
  );
};

export default PDFViewer;