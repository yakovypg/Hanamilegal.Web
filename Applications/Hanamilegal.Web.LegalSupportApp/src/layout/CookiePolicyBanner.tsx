import "styles/layout/cookie-policy-banner.css";

import { COOKIE_CONSENT_KEY, COOKIE_POLICY } from "config";
import React, { useState } from "react";
import { useTranslation } from "react-i18next";

interface Props {
  readonly onAccept?: () => void;
}

export const CookiePolicyBanner: React.FC<Props> = ({ onAccept }: Props) => {
  const { t } = useTranslation();

  const [isVisible, setIsVisible] = useState<boolean>(
    () => localStorage.getItem(COOKIE_CONSENT_KEY) !== "true",
  );

  const handleAccept = (): void => {
    localStorage.setItem(COOKIE_CONSENT_KEY, "true");
    setIsVisible(false);
    onAccept?.();
  };

  if (!isVisible) {
    return null;
  }

  const processCookieLabel: React.ReactNode = (
    <p className="cookie-policy-banner-text">
      {t("sentense.cookieBanner.consent")}{" "}
      <a className="cookie-policy-banner-link" href={COOKIE_POLICY} target="_blank" rel="noreferrer">
        {t("sentense.cookieBanner.cookiePolicy")}
      </a>
    </p>
  );

  return (
    <aside className="cookie-policy-banner-container">
      {processCookieLabel}

      <button className="cookie-policy-banner-button" type="button" onClick={handleAccept}>
        {t("phrase.understand")}
      </button>
    </aside>
  );
};
