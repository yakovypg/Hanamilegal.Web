import { SnakeNav } from "components";
import React from "react";
import { useTranslation } from "react-i18next";

export const HomePage: React.FC = () => {
  const { t } = useTranslation();

  return (
    <div>
      <h3 className="mt-5 text-center text-uppercase">{t("organization.activity")}</h3>
      <p className="text-center fs-4">{t("organization.activityNote")}</p>
      <SnakeNav className="mt-5" />
    </div>
  );
};
