import "styles/components/locale-toggle.css";

import React from "react";
import { useTranslation } from "react-i18next";
import {
  changeLanguage,
  DEFAULT_LANGUAGES,
  type DefaultLanguages,
  isCurrentLanguageEnglish
} from "utils";

interface Props {
  readonly className?: string;
}

export const LocaleToggle: React.FC<Props> = ({ className }: Props) => {
  const { i18n } = useTranslation();

  const engLocaleUsed: boolean = isCurrentLanguageEnglish(i18n);

  function handleChangeLanguage(newLanguage: DefaultLanguages): void {
    void changeLanguage(i18n, newLanguage);
  }

  return (
    <div className={className ?? ""}>
      <span
        className={`locale-label ${!engLocaleUsed ? "active-locale-label" : ""}`}
        onClick={() => handleChangeLanguage(DEFAULT_LANGUAGES.russian)}>
        РУ
      </span>

      <span className="mx-2">/</span>

      <span
        className={`locale-label ${engLocaleUsed ? "active-locale-label" : ""}`}
        onClick={() => handleChangeLanguage(DEFAULT_LANGUAGES.english)}>
        EN
      </span>
    </div>
  );
};
