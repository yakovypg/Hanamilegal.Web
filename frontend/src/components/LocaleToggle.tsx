import "styles/components/locale-toggle.css";

import React, { useState } from "react";
import { changeLanguage, DEFAULT_LANGUAGES, isCurrentLanguageEnglish } from "utils";

interface Props {
  readonly className?: string;
}

export const LocaleToggle: React.FC<Props> = ({ className }: Props) => {
  const [engLocaleUsed, setEngLocaleUsed] = useState<boolean>(isCurrentLanguageEnglish());

  function changeLocale(newLanguage: string) {
    setEngLocaleUsed(newLanguage === DEFAULT_LANGUAGES.english);
    changeLanguage(newLanguage);
  }

  return (
    <div className={className ?? ""}>
      <span
        className={`locale-label ${!engLocaleUsed ? "active-locale-label" : ""}`}
        onClick={() => changeLocale(DEFAULT_LANGUAGES.russian)}>
        РУ
      </span>

      <span className="mx-2">/</span>

      <span
        className={`locale-label ${engLocaleUsed ? "active-locale-label" : ""}`}
        onClick={() => changeLocale(DEFAULT_LANGUAGES.english)}>
        EN
      </span>
    </div>
  );
};
