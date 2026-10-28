const cookiePolicyName: string = import.meta.env.VITE_COOKIE_POLICY_NAME;

if (!cookiePolicyName) {
  throw new Error("VITE_COOKIE_POLICY_NAME is not defined");
}

const privacyPolicyName: string = import.meta.env.VITE_PRIVACY_POLICY_NAME;

if (!privacyPolicyName) {
  throw new Error("VITE_PRIVACY_POLICY_NAME is not defined");
}

export const DOCS_PATH: string = "/docs";

export const COOKIE_POLICY: string = `${DOCS_PATH}/${cookiePolicyName}`;
export const PRIVACY_POLICY: string = `${DOCS_PATH}/${privacyPolicyName}`;
