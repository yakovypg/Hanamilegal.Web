import "styles/components/contacts-menu.css";

import contactsConfig from "@config/contacts.json";
import { faFacebook, faTelegram, faVk } from "@fortawesome/free-brands-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React from "react";
import { useTranslation } from "react-i18next";
import { formatPhone } from "utils";

interface Props {
  readonly className?: string;
}

export const ContactsMenu: React.FC<Props> = ({ className }: Props) => {
  const { t } = useTranslation();

  return (
    <div className={`contacts-menu-wrapper ${className}`} role="region">
      <div className="contacts-menu">
        <h5 className="m-0 text-center">
          {t("sentense.waitCall")} {t("word.from")} {t("organization.workingHoursStart")}{" "}
          {t("word.to")} {t("organization.workingHoursEnd")}
        </h5>

        <div className="contacts-menu-2-col-grid">
          <div className="contacts-menu-box" role="group">
            <h3 className="contacts-menu-box-title">{t("phrase.email")}</h3>
            <a
              className="text-primary text-decoration-none"
              href={`mailto:${contactsConfig.email}`}>
              {contactsConfig.email}
            </a>
          </div>

          <div className="contacts-menu-box" role="group">
            <h3 className="contacts-menu-box-title">{t("phrase.phone")}</h3>
            <a className="text-primary text-decoration-none" href={`tel:${contactsConfig.phone}`}>
              {formatPhone(contactsConfig.phone)}
            </a>
          </div>
        </div>

        <div className="contacts-menu-box" role="group">
          <h3 className="contacts-menu-box-title">{t("sentense.followSocialMedia")}</h3>
          <div className="d-flex justify-content-center gap-3">
            <a
              className="text-muted fs-4"
              href={contactsConfig.social.vk}
              target="_blank"
              rel="noreferrer">
              <FontAwesomeIcon icon={faVk} size="lg" />
            </a>
            <a
              className="text-muted fs-4"
              href={contactsConfig.social.telegram}
              target="_blank"
              rel="noreferrer">
              <FontAwesomeIcon icon={faTelegram} size="lg" />
            </a>
            <a
              className="text-muted fs-4"
              href={contactsConfig.social.facebook}
              target="_blank"
              rel="noreferrer">
              <FontAwesomeIcon icon={faFacebook} size="lg" />
            </a>
          </div>
        </div>
      </div>
    </div>
  );
};
