interface ApplicationTypes {
  readonly softwareDevelopment: string;
  readonly otherQuestion: string;
}

export const APPLICATION_TYPES: ApplicationTypes = {
  softwareDevelopment: "softwareDevelopment",
  otherQuestion: "otherQuestion"
};

export type ApplicationType = (typeof APPLICATION_TYPES)[keyof typeof APPLICATION_TYPES];
