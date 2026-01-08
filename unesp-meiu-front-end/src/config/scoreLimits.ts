export type ScoreLimit = {
  min: number;
  max: number | null;
  step?: number;
};

export const SCORE_LIMITS: Record<string, ScoreLimit> = {
  performanceCoefficient: { min: 1, max: 2, step: 0.01 },
  scientificInitiationProgramScore: { min: 0, max: 6 },
  institutionalMonitoringProgramScore: { min: 0, max: 6 },
  juniorEnterpriseExperienceScore: { min: 0, max: 2 },
  projectInTechnologicalHotelScore: { min: 0, max: 2 },
  internshipEmploymentScore: { min: 0, max: 8 },
  volunteeringScore: { min: 0, max: 2, step: 0.5 },
  highGradeDisciplineScore: { min: 0, max: null },
  technologyCertificationScore: { min: 0, max: 6 },
  lowLevelTechScore: { min: 0, max: 24 },
  highGradeCoursesScore: { min: 0, max: 6 },
  aiProjectsScore: { min: 0, max: 30 },
};
