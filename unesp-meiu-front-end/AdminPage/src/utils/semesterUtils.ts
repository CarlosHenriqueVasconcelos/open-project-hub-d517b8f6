export const getCurrentSemester = () => {
  const now = new Date();
  const year = now.getFullYear();
  const month = now.getMonth() + 1; // JavaScript months are 0-based
  const semester = month <= 6 ? "1" : "2";
  return `${year}-${semester}`;
};

export const formatSemester = (semester: string) => {
  const [year, period] = semester.split("-");
  return `${year}.${period}`;
};

export const generateSemesterOptions = () => {
  const currentDate = new Date();
  const currentYear = currentDate.getFullYear();
  const semesters: string[] = [];
  
  // Generate options for current year and previous 2 years
  for (let year = currentYear; year >= currentYear - 2; year--) {
    semesters.push(`${year}-2`, `${year}-1`);
  }
  
  return semesters;
};