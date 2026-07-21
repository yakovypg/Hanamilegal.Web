import "styles/pages/page.css";

import React, { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { TWO_LINES_ORGANIZATION_TITLE_MAX_WIDTH } from "utils";

interface Props {
  readonly className?: string;
}

const useWindowSize = () => {
  const [windowSize, setWindowSize] = useState({
    width: window.innerWidth,
    height: window.innerHeight
  });

  useEffect(() => {
    const handleResize = () => {
      setWindowSize({
        width: window.innerWidth,
        height: window.innerHeight
      });
    };

    window.addEventListener("resize", handleResize);

    return () => {
      window.removeEventListener("resize", handleResize);
    };
  }, []);

  return windowSize;
};

export const OrganizationTitle: React.FC<Props> = ({ className }: Props) => {
  const { t } = useTranslation();
  const { width } = useWindowSize();

  return width < TWO_LINES_ORGANIZATION_TITLE_MAX_WIDTH ? (
    <h3 className={`page-title two-lines-title uppercase-title color-highlight ${className}`}>
      <span className="word1">{t("organization.splitName").split(" ")[0]}</span>
      <span className="word2">{t("organization.splitName").split(" ").slice(1).join(" ")}</span>
    </h3>
  ) : (
    <h3 className="page-title uppercase-title color-highlight">{t("organization.name")}</h3>
  );
};
