export const StarRating = ({ max, value, onChange, disabled = false }) => {
  const stars = Array.from({ length: max }, (_, index) => index + 1);

  const handleClick = (star: number) => {
    if (disabled) return;
    
    // Toggle: se clicar na mesma estrela, desmarca (volta para 0)
    if (star === value) {
      onChange(0);
    } else {
      onChange(star);
    }
  };

  return (
    <div className="flex items-center">
      {stars.map((star) => (
        <button
          key={star}
          onClick={() => handleClick(star)}
          disabled={disabled}
          className={`h-6 w-6 transition-colors ${
            star <= value ? "text-yellow-500" : "text-gray-300"
          } ${disabled ? "opacity-50 cursor-not-allowed" : "hover:text-yellow-400"}`}
        >
          ★
        </button>
      ))}
    </div>
  );
};
