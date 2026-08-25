interface ServicePanels {
  readonly desktopDevelopment: string;
  readonly cliDevelopment: string;
  readonly webDevelopment: string;
  readonly apiDevelopment: string;
  readonly automationScriptsDevelopment: string;
  readonly testsDevelopment: string;
  readonly databaseCreation: string;
  readonly serverConfiguration: string;
  readonly dockerDeployment: string;
  readonly httpsConfiguration: string;
}

export const SERVICE_PANELS: ServicePanels = {
  desktopDevelopment: "desktop",
  cliDevelopment: "cli",
  webDevelopment: "web",
  apiDevelopment: "api",
  automationScriptsDevelopment: "scripts",
  testsDevelopment: "tests",
  databaseCreation: "database",
  serverConfiguration: "server",
  dockerDeployment: "docker",
  httpsConfiguration: "https"
};

export type ServiceSlug = (typeof SERVICE_PANELS)[keyof typeof SERVICE_PANELS];
