import "styles/link.css";

import contactsConfig from "@config/contacts.json";
import copyrightConfig from "@config/copyright.json";
import { faTelegram, faVk } from "@fortawesome/free-brands-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React from "react";
import { Container } from "react-bootstrap";
import { useTranslation } from "react-i18next";
import { formatPhone } from "utils";

export const Footer: React.FC = () => {
  const { t } = useTranslation();

  return (
    <footer className="bg-light py-3 d-flex align-items-center">
      <Container className="d-flex justify-content-center gap-5 align-items-center flex-wrap">
        <div className="text-center">
          <p className="fs-6 my-0">
            {copyrightConfig.symbol} {copyrightConfig.year} {t("organization.name")}
          </p>
          <p className="text-muted fs-6 my-0">{t("organization.activity")}</p>
          <a className="text-muted fs-6 phone-link" href={`tel:${contactsConfig.phone}`}>
            {formatPhone(contactsConfig.phone)}
          </a>
        </div>

        <div className="text-center">
          <p className="fs-6 mb-2">{t("sentense.followSocialMedia")}</p>
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
          </div>
        </div>
      </Container>
    </footer>
  );
};
