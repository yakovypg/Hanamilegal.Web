import "styles/icon.css";
import "styles/pages/services-page.css";

import supportedConfig from "@config/supported.json";
import { faDocker } from "@fortawesome/free-brands-svg-icons";
import {
  faBug,
  faDatabase,
  faDesktop,
  faGlobe,
  faNetworkWired,
  faRobot,
  faServer,
  faShieldHalved,
  faTerminal
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Service } from "components";
import React from "react";
import { useTranslation } from "react-i18next";
import { type NavigateFunction, useNavigate } from "react-router-dom";
import { SERVICE_PANELS, VIEW_PANELS } from "types";
import { getRoutePath, getServiceRoutePath } from "utils";

export const ServicesPage: React.FC = () => {
  const navigate: NavigateFunction = useNavigate();
  const { t } = useTranslation();

  return (
    <div>
      <h3 className="text-center text-decoration-underline">{t("title.services")}</h3>
      <div className="mt-5 services-row">
        <Service
          name={t("service.desktopDevelopment.name")}
          icon={<FontAwesomeIcon className="service-icon" icon={faDesktop} />}
          onClick={() => navigate(getServiceRoutePath(SERVICE_PANELS.desktopDevelopment))}
        />
        <Service
          name={t("service.cliDevelopment.name")}
          icon={<FontAwesomeIcon className="service-icon" icon={faGlobe} />}
          onClick={() => navigate(getServiceRoutePath(SERVICE_PANELS.cliDevelopment))}
        />
        <Service
          name={t("service.webDevelopment.name")}
          icon={<FontAwesomeIcon className="service-icon" icon={faTerminal} />}
          onClick={() => navigate(getServiceRoutePath(SERVICE_PANELS.webDevelopment))}
        />
        <Service
          name={t("service.apiDevelopment.name")}
          icon={<FontAwesomeIcon className="service-icon" icon={faNetworkWired} />}
          onClick={() => navigate(getServiceRoutePath(SERVICE_PANELS.apiDevelopment))}
        />
        <Service
          name={t("service.automationScriptsDevelopment.name")}
          icon={<FontAwesomeIcon className="service-icon" icon={faRobot} />}
          onClick={() => navigate(getServiceRoutePath(SERVICE_PANELS.automationScriptsDevelopment))}
        />
        <Service
          name={t("service.testsDevelopment.name")}
          icon={<FontAwesomeIcon className="service-icon" icon={faBug} />}
          onClick={() => navigate(getServiceRoutePath(SERVICE_PANELS.testsDevelopment))}
        />
        <Service
          name={t("service.databaseCreation.name")}
          icon={<FontAwesomeIcon className="service-icon" icon={faDatabase} />}
          onClick={() => navigate(getServiceRoutePath(SERVICE_PANELS.databaseCreation))}
        />
        <Service
          name={t("service.serverConfiguration.name")}
          icon={<FontAwesomeIcon className="service-icon" icon={faServer} />}
          onClick={() => navigate(getServiceRoutePath(SERVICE_PANELS.serverConfiguration))}
        />
        <Service
          name={t("service.dockerDeployment.name")}
          icon={<FontAwesomeIcon className="service-icon" icon={faDocker} />}
          onClick={() => navigate(getServiceRoutePath(SERVICE_PANELS.dockerDeployment))}
        />
        <Service
          name={t("service.httpsConfiguration.name")}
          icon={<FontAwesomeIcon className="service-icon" icon={faShieldHalved} />}
          onClick={() => navigate(getServiceRoutePath(SERVICE_PANELS.httpsConfiguration))}
        />
      </div>

      <div className="mt-5 p-2 fs-5 text-center border border-secondary">
        <p className="m-0">{t("sentense.refineProjects")}.</p>
        <p className="mt-3 mb-0 text-decoration-underline">{t("sentense.languageStack")}</p>
        <p className="m-0">{supportedConfig.languageStack.join(", ")}</p>
        <p className="mt-3 mb-0 text-decoration-underline">{t("sentense.techStack")}</p>
        <p className="m-0">{supportedConfig.techStack.join(", ")}</p>
      </div>

      <p className="mt-4 mb-0 fs-5 text-center">
        {t("sentense.anyQuestions")}?{" "}
        <a href={getRoutePath(VIEW_PANELS.contacts)}>{t("action.contact")}</a> {t("word.withUs")}!
      </p>
    </div>
  );
};
