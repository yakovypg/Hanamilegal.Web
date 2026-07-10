import "styles/link.css"

import { LocaleToggle } from "components";
import { Nav } from "react-bootstrap";
import { useTranslation } from "react-i18next";
import { LinkContainer } from "react-router-bootstrap";
import { VIEW_PANELS } from "types";
import { getRoutePath } from "utils";

export const Header: React.FC = () => {
  const { t } = useTranslation();

  return (
    <div className="d-flex flex-row p-4">
      <LinkContainer to={getRoutePath(VIEW_PANELS.home)}>
        <Nav.Link className="home-nav-link">{t("organization.fullName")}</Nav.Link>
      </LinkContainer>

      <LocaleToggle className="mx-4" />
    </div>
  );
};
