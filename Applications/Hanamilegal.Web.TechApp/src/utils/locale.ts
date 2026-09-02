import { type i18n } from "i18next";

interface Language {
  readonly english: string;
  readonly russian: string;
}

export const DEFAULT_LANGUAGES: Language = {
  english: "en",
  russian: "ru"
};

export type DefaultLanguages = (typeof DEFAULT_LANGUAGES)[keyof typeof DEFAULT_LANGUAGES];

export function currentLanguageCode(i18nInstance: i18n): string {
  return i18nInstance.resolvedLanguage ?? i18nInstance.language;
}

export function currentLanguage(i18nInstance: i18n): string {
  const languageCode: string = currentLanguageCode(i18nInstance);
  return languageCode ? languageCode.split(/[-_]/)[0].toLowerCase() : languageCode;
}

export function isCurrentLanguageEnglish(i18nInstance: i18n): boolean {
  return currentLanguage(i18nInstance) == DEFAULT_LANGUAGES.english;
}

export function isCurrentLanguageRussian(i18nInstance: i18n): boolean {
  return currentLanguage(i18nInstance) == DEFAULT_LANGUAGES.russian;
}

export async function changeLanguage(
  i18nInstance: i18n,
  newLanguage: DefaultLanguages
): Promise<void> {
  await i18nInstance.changeLanguage(newLanguage);
}

export async function switchLanguage(i18nInstance: i18n): Promise<void> {
  const next: string =
    currentLanguage(i18nInstance) === DEFAULT_LANGUAGES.russian
      ? DEFAULT_LANGUAGES.english
      : DEFAULT_LANGUAGES.russian;

  await i18nInstance.changeLanguage(next);
}
