import "styles/pages/service-details-page.css"

import React from "react";
import { useTranslation } from "react-i18next";

interface Props {
  readonly name: string;
  readonly description: string;
  readonly exampleImageSource: string | undefined;
}

export const ServiceDetailsPage: React.FC<Props> = ({
  name,
  description,
  exampleImageSource
}: Props) => {
  const { t } = useTranslation();
  const descriptionLines: string[] = description.split("\n");

  return (
    <div className="text-center">
      <h3 className="mb-4">{name}</h3>
      {descriptionLines.map((line: string, idx: number) => {
        const margin: string = idx == descriptionLines.length - 1 ? "mb-4" : "mb-0";

        return (
          <p key={idx} className={`fs-5 ${margin}`}>
            {line === "" ? "\u00A0" : line}
          </p>
        );
      })}
      <h4 className="mb-4">{t("title.example")}</h4>
      <img className="mb-4 service-example-img" src={exampleImageSource} alt="example" />
    </div>
  );
};
