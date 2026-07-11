import "styles/pages/home-page.css";
import "styles/pages/page.css";

import contactsConfig from "@config/contacts.json";
import React from "react";
import { Nav, Navbar } from "react-bootstrap";
import { useTranslation } from "react-i18next";
import { LinkContainer } from "react-router-bootstrap";
import { VIEW_PANELS } from "types";
import { formatPhone, getRoutePath } from "utils";

export const HomePage: React.FC = () => {
  const { t } = useTranslation();

  return (
    <div className="page-center-container">
      <div className="page-center-left-subcontainer">
        <h3 className="page-title color-primary">{t("organization.name")}</h3>

        <Navbar className="my-0 py-0">
          <div className="d-flex justify-content-center align-items-center">
            <Nav className="align-items-center big-text color-secondary">
              <LinkContainer
                className="mx-0 px-0 page-nav-link-container"
                to={getRoutePath(VIEW_PANELS.jurisprudence)}>
                <Nav.Link>{t("title.jurisprudence")}</Nav.Link>
              </LinkContainer>
              <LinkContainer
                className="mx-4 page-nav-link-container"
                to={getRoutePath(VIEW_PANELS.developing)}>
                <Nav.Link>{t("title.developing")}</Nav.Link>
              </LinkContainer>
            </Nav>
          </div>
        </Navbar>

        <hr className="my-2 mb-3 w-100" />

        <h3 className="page-subtitle">{t("phrase.aboutUs")}</h3>
        <p className="normal-text">{t("organization.about.p1")}</p>
        <p className="normal-text">{t("organization.about.p2")}</p>

        <hr className="my-2 w-100" />

        <div className="contacts-container">
          <a className="contacts-item big-text color-primary" href={`mailto:${contactsConfig.email}`}>
            {contactsConfig.email}
          </a>
          <a className="contacts-item big-text color-primary" href={`tel:${contactsConfig.phone}`}>
            {formatPhone(contactsConfig.phone)}
          </a>
        </div>
      </div>
    </div>
  );
};
