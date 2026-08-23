export interface EndpointsConfig {
  readonly applicationsApiBaseUrl: string;
}

const applicationsApiBaseUrl: string = import.meta.env.VITE_APPLICATIONS_API_BASE_URL;

if (!applicationsApiBaseUrl) {
  throw new Error("VITE_APPLICATIONS_API_BASE_URL is not defined");
}

export const ENDPOINTS_CONFIG: EndpointsConfig = {
  applicationsApiBaseUrl: applicationsApiBaseUrl,
};
