import "styles/icon.css";
import "styles/layout/header.css";

import icon from "assets/icon.svg";
import { LocaleToggle } from "components";
import React from "react";
import { Container, Nav, Navbar } from "react-bootstrap";
import { useTranslation } from "react-i18next";
import { LinkContainer } from "react-router-bootstrap";
import { Link } from "react-router-dom";
import { VIEW_PANELS } from "types";
import { getRoutePath, removeNavLinkActive } from "utils";

export const HeaderMobile: React.FC = () => {
  const { t } = useTranslation();

  return (
    <header>
      <Navbar bg="light" expand="md" className="py-2">
        <Container className="d-flex justify-content-between align-items-center">
          <div className="d-flex align-items-center">
            <Link
              onClick={() => removeNavLinkActive()}
              to={getRoutePath(VIEW_PANELS.home)}
              className="d-flex align-items-center text-decoration-none me-4">
              <img className="me-2 icon-48" src={icon} alt="icon" />
              <div className="d-flex flex-column">
                <span className="fw-bold text-dark">{t("organization.name")}</span>
                <small className="text-muted">{t("phrase.officialSite")}</small>
              </div>
            </Link>
          </div>

          <Navbar.Toggle aria-controls="main-navbar-collapse" />

          <Navbar.Collapse id="main-navbar-collapse" className="justify-content-center">
            <Nav className="mx-auto align-items-center">
              <LinkContainer to={getRoutePath(VIEW_PANELS.services)}>
                <Nav.Link>{t("title.services")}</Nav.Link>
              </LinkContainer>
              <LinkContainer to={getRoutePath(VIEW_PANELS.projects)}>
                <Nav.Link>{t("title.projects")}</Nav.Link>
              </LinkContainer>
              <LinkContainer to={getRoutePath(VIEW_PANELS.reviews)}>
                <Nav.Link>{t("title.reviews")}</Nav.Link>
              </LinkContainer>
              <LinkContainer to={getRoutePath(VIEW_PANELS.contacts)}>
                <Nav.Link>{t("title.contacts")}</Nav.Link>
              </LinkContainer>
            </Nav>
            <div className="my-2 d-flex justify-content-center">
              <LocaleToggle />
            </div>
          </Navbar.Collapse>
        </Container>
      </Navbar>
    </header>
  );
};
