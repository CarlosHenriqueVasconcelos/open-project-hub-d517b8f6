import { Card, CardContent, CardHeader, CardTitle } from "@admin/components/ui/card";
import { ChartContainer, ChartTooltip } from "@admin/components/ui/chart";
import { Bar, BarChart, XAxis, YAxis } from "recharts";
import { StudentRegistration } from "@admin/types";

interface SkillsDistributionChartProps {
  registrations: StudentRegistration[];
}

const SkillsDistributionChart = ({ registrations }: SkillsDistributionChartProps) => {
  const skillsDistribution = registrations.reduce((acc: { name: string; count: number }[], reg) => {
    reg.studentSkills.forEach((skill) => {
      const existingSkill = acc.find(item => item.name === skill.skill.name);
      if (existingSkill) {
        existingSkill.count += 1;
      } else {
        acc.push({ name: skill.skill.name, count: 1 });
      }
    });
    return acc;
  }, []).sort((a, b) => b.count - a.count).slice(0, 10);

  return (
    <Card className="col-span-2">
      <CardHeader>
        <CardTitle>Top 10 Habilidades</CardTitle>
      </CardHeader>
      <CardContent>
        <div className="h-[300px]">
          <ChartContainer
            config={{
              bar: {
                color: "hsl(var(--primary))",
              },
            }}
          >
            <BarChart data={skillsDistribution} layout="vertical">
              <XAxis type="number" />
              <YAxis dataKey="name" type="category" width={150} />
              <ChartTooltip />
              <Bar dataKey="count" fill="currentColor" />
            </BarChart>
          </ChartContainer>
        </div>
      </CardContent>
    </Card>
  );
};

export default SkillsDistributionChart;