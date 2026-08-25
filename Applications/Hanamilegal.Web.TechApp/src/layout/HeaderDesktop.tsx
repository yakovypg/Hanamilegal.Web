import "styles/icon.css";
import "styles/layout/header.css";

import favicon from "assets/favicon.ico";
import { LocaleToggle } from "components";
import React from "react";
import { Container, Nav, Navbar } from "react-bootstrap";
import { useTranslation } from "react-i18next";
import { LinkContainer } from "react-router-bootstrap";
import { Link } from "react-router-dom";
import { VIEW_PANELS } from "types";
import { getRoutePath, removeNavLinkActive } from "utils";

export const HeaderDesktop: React.FC = () => {
  const { t } = useTranslation();

  return (
    <header>
      <Navbar bg="light" className="py-2">
        <Container className="d-flex justify-content-center">
          <div className="d-flex align-items-center">
            <Link
              onClick={() => removeNavLinkActive()}
              to={getRoutePath(VIEW_PANELS.home)}
              className="d-flex align-items-center text-decoration-none me-4">
              <img className="me-2 icon-48" src={favicon} alt="favicon" />
              <div className="d-flex flex-column">
                <span className="fw-bold text-dark">{t("organization.name")}</span>
                <small className="text-muted">{t("phrase.officialSite")}</small>
              </div>
            </Link>

            <Nav className="mx-1 align-items-center">
              <LinkContainer to={getRoutePath(VIEW_PANELS.services)}>
                <Nav.Link>{t("title.services")}</Nav.Link>
              </LinkContainer>
              <LinkContainer className="ms-3" to={getRoutePath(VIEW_PANELS.projects)}>
                <Nav.Link>{t("title.projects")}</Nav.Link>
              </LinkContainer>
              <LinkContainer className="ms-3" to={getRoutePath(VIEW_PANELS.reviews)}>
                <Nav.Link>{t("title.reviews")}</Nav.Link>
              </LinkContainer>
              <LinkContainer className="ms-3" to={getRoutePath(VIEW_PANELS.contacts)}>
                <Nav.Link>{t("title.contacts")}</Nav.Link>
              </LinkContainer>
            </Nav>
          </div>
        </Container>
        <div className="d-flex align-items-center me-3">
          <LocaleToggle />
        </div>
      </Navbar>
    </header>
  );
};
