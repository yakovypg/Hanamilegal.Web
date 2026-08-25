import React from "react";
import { useTranslation } from "react-i18next";

export const NotFoundPage: React.FC = () => {
  const { t } = useTranslation();

  return (
    <div>
      <h3 className="text-center">{t("title.pageNotFound")}</h3>
    </div>
  );
};
