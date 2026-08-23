interface ApplicationTypes {
  readonly legalSupport: string;
  readonly softwareDevelopment: string;
}

export const APPLICATION_TYPES: ApplicationTypes = {
  legalSupport: "legalSupport",
  softwareDevelopment: "softwareDevelopment"
};

export type ApplicationType = (typeof APPLICATION_TYPES)[keyof typeof APPLICATION_TYPES];
