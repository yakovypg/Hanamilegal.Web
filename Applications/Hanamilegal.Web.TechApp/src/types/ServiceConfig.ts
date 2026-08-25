import projectImage from "assets/example-desktop.png";

import { SERVICE_PANELS, type ServiceSlug } from "./ServicePanels";

export interface ServiceConfig {
  readonly translationName: string;
  readonly exampleImageSource: string | undefined;
}

export const SERVICE_CONFIGS: Record<ServiceSlug, ServiceConfig> = {
  [SERVICE_PANELS.desktopDevelopment]: {
    translationName: "desktopDevelopment",
    exampleImageSource: projectImage
  },
  [SERVICE_PANELS.cliDevelopment]: {
    translationName: "cliDevelopment",
    exampleImageSource: projectImage
  },
  [SERVICE_PANELS.webDevelopment]: {
    translationName: "webDevelopment",
    exampleImageSource: projectImage
  },
  [SERVICE_PANELS.apiDevelopment]: {
    translationName: "apiDevelopment",
    exampleImageSource: projectImage
  },
  [SERVICE_PANELS.automationScriptsDevelopment]: {
    translationName: "automationScriptsDevelopment",
    exampleImageSource: projectImage
  },
  [SERVICE_PANELS.testsDevelopment]: {
    translationName: "testsDevelopment",
    exampleImageSource: projectImage
  },
  [SERVICE_PANELS.databaseCreation]: {
    translationName: "databaseCreation",
    exampleImageSource: projectImage
  },
  [SERVICE_PANELS.serverConfiguration]: {
    translationName: "serverConfiguration",
    exampleImageSource: projectImage
  },
  [SERVICE_PANELS.dockerDeployment]: {
    translationName: "dockerDeployment",
    exampleImageSource: projectImage
  },
  [SERVICE_PANELS.httpsConfiguration]: {
    translationName: "httpsConfiguration",
    exampleImageSource: projectImage
  }
};
