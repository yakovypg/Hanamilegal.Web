import "styles/pages/page.css";
import "styles/link.css";

import React from "react";
import { useTranslation } from "react-i18next";
import { VIEW_PANELS } from "types";
import { getRoutePath } from "utils";

export const NotFoundPage: React.FC = () => {
  const { t } = useTranslation();

  return (
    <div className="page-center-container">
      <div className="page-center-left-subcontainer">
        <div className="text-center">
          <h3>{t("title.pageNotFound")}</h3>
          <a className="big-text color-common home-link" href={getRoutePath(VIEW_PANELS.home)}>
            {t("action.backHome")}
          </a>
        </div>
      </div>
    </div>
  );
};
