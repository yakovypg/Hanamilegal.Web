import "styles/components/locale-toggle.css";

import React from "react";
import { useTranslation } from "react-i18next";
import { currentLanguage, isCurrentLanguageRussian, switchLanguage } from "utils";

interface Props {
  readonly className?: string;
}

export const LocaleToggle: React.FC<Props> = ({ className }: Props) => {
  const { i18n } = useTranslation();

  function handleSwitchLanguage(): void {
    void switchLanguage(i18n);
  }

  const isCurrentLanguageRu: boolean = isCurrentLanguageRussian(i18n);
  const localeClass: string = isCurrentLanguageRu ? "locale-ru" : "locale-en";

  return (
    <button
      className={`locale-toggle-button btn btn-outline-secondary p-0 ${className ?? ""}`}
      type="button"
      role="switch"
      aria-checked={isCurrentLanguageRu}
      onClick={handleSwitchLanguage}>
      <div className={`locale-toggle ${localeClass}`} aria-hidden="true">
        <div className="locale-track">
          <div className="locale-thumb">
            <span className="locale-thumb-label">{currentLanguage(i18n)}</span>
          </div>
        </div>
      </div>
    </button>
  );
};
