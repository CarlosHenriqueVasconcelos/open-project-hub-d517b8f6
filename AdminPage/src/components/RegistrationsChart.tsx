import { Card, CardContent, CardHeader, CardTitle } from "@admin/components/ui/card";
import { ChartContainer, ChartTooltip } from "@admin/components/ui/chart";
import { Line, LineChart, XAxis, YAxis, ResponsiveContainer } from "recharts";
import { StudentRegistration } from "@admin/types";

interface RegistrationsChartProps {
  registrations: StudentRegistration[];
}

const RegistrationsChart = ({ registrations }: RegistrationsChartProps) => {
  const formatDateToBrazilian = (date: Date) => {
    return date.toLocaleDateString('pt-BR', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric'
    });
  };

  const dailyRegistrations = registrations
    .reduce((acc: { date: string; count: number }[], reg) => {
      const date = formatDateToBrazilian(new Date(reg.registrationDate));
      const existingDate = acc.find(item => item.date === date);
      
      if (existingDate) {
        existingDate.count += 1;
      } else {
        acc.push({ date, count: 1 });
      }
      
      return acc;
    }, [])
    .sort((a, b) => {
      const [dayA, monthA, yearA] = a.date.split('/').map(Number);
      const [dayB, monthB, yearB] = b.date.split('/').map(Number);
      const dateA = new Date(yearA, monthA - 1, dayA);
      const dateB = new Date(yearB, monthB - 1, dayB);
      return dateA.getTime() - dateB.getTime();
    });

  return (
    <Card className="col-span-2">
      <CardHeader>
        <CardTitle>Inscrições por Dia</CardTitle>
      </CardHeader>
      <CardContent>
        <div className="h-[300px]">
          <ChartContainer
            config={{
              line: {
                color: "hsl(var(--primary))",
              },
            }}
          >
            <LineChart data={dailyRegistrations}>
              <XAxis 
                dataKey="date"
                interval="preserveStartEnd"
                angle={-45}
                textAnchor="end"
                height={60}
              />
              <YAxis />
              <ChartTooltip />
              <Line
                type="monotone"
                dataKey="count"
                strokeWidth={2}
                dot={false}
              />
            </LineChart>
          </ChartContainer>
        </div>
      </CardContent>
    </Card>
  );
};

export default RegistrationsChart;