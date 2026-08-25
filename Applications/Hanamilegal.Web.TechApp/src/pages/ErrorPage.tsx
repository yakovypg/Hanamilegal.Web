import React from "react";
import { Button } from "react-bootstrap";
import { useTranslation } from "react-i18next";
import { type Location, type NavigateFunction, useLocation, useNavigate } from "react-router-dom";
import { VIEW_PANELS } from "types";
import type { ErrorLocationState } from "types/ErrorLocationState";
import { getRoutePath } from "utils";

export const ErrorPage: React.FC = () => {
  const location: Location = useLocation();
  const navigate: NavigateFunction = useNavigate();
  const { t } = useTranslation();

  const state: ErrorLocationState | null = location.state as ErrorLocationState | null;
  const message: string = state?.error ?? t("error.internal");

  return (
    <div className="d-flex flex-column align-items-center justify-content-center text-center w-100 min-vh-100 p-3">
      <h1 className="text-primary mb-2">{t("title.error")}</h1>
      <p className="text-secondary fs-4 mb-3">{message}</p>

      <Button
        className="mt-3 px-4"
        variant="primary"
        onClick={() => navigate(getRoutePath(VIEW_PANELS.home), { replace: true })}>
        {t("action.backHome")}
      </Button>
    </div>
  );
};
