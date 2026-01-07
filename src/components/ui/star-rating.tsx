export const StarRating = ({ max, value, onChange, disabled = false }) => {
  const stars = Array.from({ length: max }, (_, index) => index + 1);

  return (
    <div className="flex items-center">
      {stars.map((star) => (
        <button
          key={star}
          onClick={() => {
            if (!disabled) onChange(star);
          }}
          disabled={disabled}
          className={`h-6 w-6 ${
            star <= value ? "text-yellow-500" : "text-gray-300"
          } ${disabled ? "opacity-50 cursor-not-allowed" : ""}`}
        >
          ★
        </button>
      ))}
    </div>
  );
};
