import "styles/pages/page.css";

import React from "react";
import { Nav, Navbar } from "react-bootstrap";
import { useTranslation } from "react-i18next";
import { LinkContainer } from "react-router-bootstrap";
import { VIEW_PANELS } from "types";
import { getRoutePath } from "utils";

export const DevelopingPage: React.FC = () => {
  const { t } = useTranslation();

  const servicesRaw: unknown = t("developing.services", { returnObjects: true });
  const services: string[] = Array.isArray(servicesRaw) ? servicesRaw as string[] : [];

  return (
    <div className="page-center-container">
      <div className="page-center-left-subcontainer">
        <h3 className="page-title color-primary">{t("organization.name")}</h3>

        <Navbar className="my-0 py-0">
          <div className="d-flex justify-content-center align-items-center">
            <Nav className="align-items-center big-text">
              <LinkContainer
                className="mx-0 px-0 page-nav-link-container"
                to={getRoutePath(VIEW_PANELS.home)}>
                <Nav.Link>{t("title.contacts")}</Nav.Link>
              </LinkContainer>
              <LinkContainer
                className="mx-4 page-nav-link-container"
                to={getRoutePath(VIEW_PANELS.jurisprudence)}>
                <Nav.Link>{t("title.jurisprudence")}</Nav.Link>
              </LinkContainer>
            </Nav>
          </div>
        </Navbar>

        <hr className="my-2 mb-3 w-100" />
        <h3 className="page-subtitle">{t("title.developing")}</h3>

        <ul className="service-list normal-text">
          {
            services.map((service: string) => (
              <li key={service}>{service}</li>
            ))
          }
        </ul>
      </div>
    </div>
  );
};
