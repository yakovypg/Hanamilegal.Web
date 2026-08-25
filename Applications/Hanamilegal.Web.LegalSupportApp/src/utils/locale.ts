import i18n from "i18next";

interface Language {
  readonly english: string;
  readonly russian: string;
}

export const DEFAULT_LANGUAGES: Language = {
  english: "en",
  russian: "ru"
};

export type DefaultLanguages = (typeof DEFAULT_LANGUAGES)[keyof typeof DEFAULT_LANGUAGES];

export function currentLanguageFullName(): string {
  return i18n.language;
}

export function currentLanguage(): string {
  return i18n.resolvedLanguage ?? i18n.language;
}

export function isCurrentLanguageEnglish(): boolean {
  return currentLanguage() == DEFAULT_LANGUAGES.english;
}

export function isCurrentLanguageRussian(): boolean {
  return currentLanguage() == DEFAULT_LANGUAGES.russian;
}

export function changeLanguage(newLanguage: DefaultLanguages): void {
  i18n.changeLanguage(newLanguage);
}

export function switchLanguage(): void {
  const next: string =
    currentLanguage() === DEFAULT_LANGUAGES.russian
      ? DEFAULT_LANGUAGES.english
      : DEFAULT_LANGUAGES.russian;

  i18n.changeLanguage(next);
}
