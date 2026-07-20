import "styles/pages/home-page.css";
import "styles/pages/page.css";
import "styles/services.css";

import contactsConfig from "@config/contacts.json";
import { OrganizationTitle } from "components";
import React from "react";
import { useTranslation } from "react-i18next";
import { formatPhone } from "utils";

export const HomePage: React.FC = () => {
  type AboutSectionConfig = {
    name: string;
    description: string;
  };

  const { t } = useTranslation();

  const servicesRaw: unknown = t("general.services", { returnObjects: true });
  const services: string[] = Array.isArray(servicesRaw) ? (servicesRaw as string[]) : [];

  const aboutSectionsRaw: AboutSectionConfig[] = t("organization.about.sections", {
    returnObjects: true
  }) as AboutSectionConfig[];

  const aboutSections: AboutSectionConfig[] = Array.isArray(aboutSectionsRaw)
    ? aboutSectionsRaw.map((section: AboutSectionConfig) => ({
        name: t(section.name),
        description: t(section.description)
      }))
    : [];

  return (
    <div className="page-center-container">
      <div className="page-center-left-subcontainer">
        <OrganizationTitle />

        <p className="mb-0 page-subtitle">{t("organization.slogan")}</p>

        <div className="info-container">
          <div className="info-item">
            <ul className="service-list full-width-list normal-text">
              {services.map((service: string) => (
                <li key={service}>{service}</li>
              ))}
            </ul>
          </div>

          <div className="vertical-line info-item" />

          <div className="info-item">
            <h3 className="page-subtitle about-title">{t("phrase.aboutUs")}</h3>
            <p className="normal-text">{t("organization.about.prolog")}</p>
            {aboutSections.map((section: AboutSectionConfig) => (
              <p className="normal-text">
                <strong>{section.name}</strong>
                <br />
                {section.description}
              </p>
            ))}
            <p className="normal-text">{t("organization.about.epilog")}</p>
          </div>
        </div>

        <div className="contacts-wrapper">
          <div className="mt-4 contacts-container">
            <a
              className="contacts-item big-text color-primary"
              href={`mailto:${contactsConfig.email}`}>
              {contactsConfig.email}
            </a>
            <a
              className="contacts-item big-text color-primary"
              href={`tel:${contactsConfig.phone}`}>
              {formatPhone(contactsConfig.phone)}
            </a>
          </div>
        </div>
      </div>
    </div>
  );
};
