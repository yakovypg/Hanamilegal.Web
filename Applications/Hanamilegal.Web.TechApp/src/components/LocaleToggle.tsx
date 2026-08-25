import "styles/components/locale-toggle.css";

import React from "react";
import { currentLanguage, isCurrentLanguageRussian, switchLanguage } from "utils";

interface Props {
  readonly className?: string;
}

export const LocaleToggle: React.FC<Props> = ({ className }: Props) => {
  const isCurrentLanguageRu: boolean = isCurrentLanguageRussian();
  const localeClass: string = isCurrentLanguageRu ? "locale-ru" : "locale-en";

  return (
    <button
      className={`locale-toggle-button btn btn-outline-secondary p-0 ${className ?? ""}`}
      type="button"
      role="switch"
      aria-checked={isCurrentLanguageRu}
      onClick={switchLanguage}>
      <div className={`locale-toggle ${localeClass}`} aria-hidden="true">
        <div className="locale-track">
          <div className="locale-thumb">
            <span className="locale-thumb-label">{currentLanguage()}</span>
          </div>
        </div>
      </div>
    </button>
  );
};
