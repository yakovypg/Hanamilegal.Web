import "styles/pages/page.css";

import React from "react";
import { useTranslation } from "react-i18next";
import { type Location, useLocation } from "react-router-dom";
import { VIEW_PANELS } from "types";
import type { ErrorLocationState } from "types/ErrorLocationState";
import { getRoutePath } from "utils";

export const ErrorPage: React.FC = () => {
  const location: Location = useLocation();
  const { t } = useTranslation();

  const state: ErrorLocationState | null = location.state as ErrorLocationState | null;
  const message: string = state?.error ?? t("error.internal");

  return (
    <div className="page-center-container">
      <div className="text-center">
        <h3>{t("title.error")}</h3>
        <p className="normal-text">{message}</p>
        <a className="big-text color-common home-link" href={getRoutePath(VIEW_PANELS.home)}>
          {t("action.backHome")}
        </a>
      </div>
    </div>
  );
};
