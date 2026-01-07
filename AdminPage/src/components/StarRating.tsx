import { Star } from "lucide-react";
import { cn } from "@admin/lib/utils";

interface StarRatingProps {
  value: number;
  onChange: (value: number) => void;
}

export const StarRating = ({ value, onChange }: StarRatingProps) => {
  return (
    <div className="flex space-x-1">
      {[1, 2, 3, 4, 5].map((star) => (
        <button
          key={star}
          type="button"
          onClick={() => onChange(star)}
          className="focus:outline-none"
        >
          <Star
            className={cn(
              "w-6 h-6",
              star <= value
                ? "fill-yellow-400 text-yellow-400"
                : "fill-transparent text-gray-300"
            )}
          />
        </button>
      ))}
    </div>
  );
};