import { Cache } from "@/mock/overview";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Typography } from "@/components/ui/typography";

interface IDatasourceCardProps {
  title: string;
  description: string;
  upNum: number;
  totalNum: number;
  viewAllURL: string;
  datasourceList: Cache[];
}

const DatasourceCard = ({
  title,
  description,
  upNum,
  totalNum,
  viewAllURL,
  datasourceList,
}: IDatasourceCardProps) => {
  return (
    <Card>
      <CardHeader>
        <CardTitle className="flex justify-between">
          {title}: {upNum}/{totalNum} up
          <Button asChild variant="outline" className="-mt-1">
            <a href={viewAllURL}>View all</a>
          </Button>
        </CardTitle>
        <CardDescription>{description}</CardDescription>
      </CardHeader>
      <CardContent className="grid gap-6">
        {datasourceList.slice(0, 3).map((datasource) => (
          <Typography key={datasource.url} variant="large">
            {datasource.type} on{" "}
            <Button className="m-0 p-0" variant="link">
              {datasource.url}
            </Button>
          </Typography>
        ))}
      </CardContent>
    </Card>
  );
};

export default DatasourceCard;