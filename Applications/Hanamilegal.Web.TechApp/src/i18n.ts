import i18n from "i18next";
import LanguageDetector from "i18next-browser-languagedetector";
import HttpBackend from "i18next-http-backend";
import { initReactI18next } from "react-i18next";

const packageVersion: string = import.meta.env.PACKAGE_VERSION ?? "dev";

i18n
  .use(HttpBackend)
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    fallbackLng: "en",
    load: "languageOnly",
    ns: ["translation"],
    debug: false,
    interpolation: { escapeValue: false },
    react: { useSuspense: false },
    backend: {
      loadPath: `/locales/{{lng}}/{{ns}}.json?v=${packageVersion}`
    }
  });

export default i18n;
