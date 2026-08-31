interface ApplicationTypes {
  readonly legalSupportForBusiness: string;
  readonly generalQuestion: string;
  readonly commercialDispute: string;
  readonly corporateDispute: string;
  readonly transactionStructuring: string;
  readonly compliance: string;
  readonly itIpPd: string;
  readonly otherQuestion: string;
}

export const APPLICATION_TYPES: ApplicationTypes = {
  legalSupportForBusiness: "legalSupportForBusiness",
  generalQuestion: "generalQuestion",
  commercialDispute: "commercialDispute",
  corporateDispute: "corporateDispute",
  transactionStructuring: "transactionStructuring",
  compliance: "compliance",
  itIpPd: "itIpPd",
  otherQuestion: "otherQuestion"
};

export type ApplicationType = (typeof APPLICATION_TYPES)[keyof typeof APPLICATION_TYPES];
