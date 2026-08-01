import "styles/components/contact-us-form.css";

import { APPLICATIONS_ENDPOINT } from "api";
import {
  ContactUsCheckBox,
  ContactUsComboBox,
  ContactUsField,
  ContactUsTextArea,
  NoticeMessage
} from "components";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import { Notice } from "types";

interface SenderData {
  readonly name: string;
  readonly email: string;
  readonly organization: string;
  readonly applicationType: string;
  readonly applicationDescription: string;
}

interface Props {
  readonly className?: string;
}

export const ContactUsForm: React.FC<Props> = ({ className }: Props) => {
  const { t } = useTranslation();

  const [hasConsentWithProcessingPersonalData, setHasConsentWithProcessingPersonalData] =
    useState<boolean>(false);

  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
  const [notice, setNotice] = useState<Notice | null>(null);

  const contactUsApplicationTypesRaw: unknown = t("contactUsApplication.types", {
    returnObjects: true
  });

  const contactUsApplicationTypes: string[] = Array.isArray(contactUsApplicationTypesRaw)
    ? (contactUsApplicationTypesRaw as string[])
    : [];

  const handleSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (isSubmitting) {
      return;
    }

    setNotice(null);
    setIsSubmitting(true);

    const form: HTMLFormElement = e.currentTarget;
    const formData: FormData = new FormData(form);

    const payload: SenderData = {
      name: String(formData.get("name") ?? ""),
      email: String(formData.get("email") ?? ""),
      organization: String(formData.get("organization") ?? ""),
      applicationType: String(formData.get("applicationType") ?? ""),
      applicationDescription: String(formData.get("applicationDescription") ?? "")
    };

    const allRequiredFieldsSpecified: boolean = Object.values(payload).every(
      (value: string) => value.trim() !== ""
    );

    if (!allRequiredFieldsSpecified) {
      setNotice(new Notice(t("error.requiredFieldsNotSpecified"), true));
      setIsSubmitting(false);
      return;
    }

    const request: RequestInit = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(payload)
    };

    let response: Response | null = null;

    try {
      response = await fetch(APPLICATIONS_ENDPOINT, request);
    } catch {
      /* empty */
    }

    if (!response?.ok) {
      setNotice(new Notice(t("error.sendData"), true));
    } else {
      form.reset();
      setNotice(new Notice(t("sentense.applicationSent"), false));
    }

    setIsSubmitting(false);
  };

  const processPersonalDataLabel: React.ReactNode = (
    <div>
      {t("sentense.personalData.consent")}
      <a className="color-primary" href="/docs/privacy-policy.pdf" target="_blank" rel="noreferrer">
        {t("sentense.personalData.privacyPolicy")}
      </a>
    </div>
  );

  return (
    <form className={className ?? ""} onSubmit={handleSubmit}>
      <div className="contact-us-elements-container">
        <ContactUsField
          required
          name="name"
          type="text"
          autoComplete="name"
          placeholder={t("placeholder.name")}
        />
        <ContactUsField
          required
          name="email"
          type="email"
          autoComplete="email"
          placeholder={t("placeholder.email")}
        />
        <ContactUsField
          required
          name="organization"
          type="text"
          autoComplete="organization"
          placeholder={t("placeholder.organization")}
        />
        <ContactUsComboBox
          required
          name="applicationType"
          options={contactUsApplicationTypes}
          placeholder={t("placeholder.applicationType")}
        />
        <ContactUsTextArea
          required
          name="applicationDescription"
          placeholder={t("placeholder.applicationDescription")}
        />
        <ContactUsCheckBox
          required
          name="consentWithProcessingPersonalData"
          label={processPersonalDataLabel}
          onChange={(checked: boolean) => setHasConsentWithProcessingPersonalData(checked)}
        />

        <button
          type="submit"
          className="mt-2 send-request-button"
          disabled={isSubmitting || !hasConsentWithProcessingPersonalData}>
          {isSubmitting ? "..." : t("phrase.sendRequest")}
        </button>

        <NoticeMessage notice={notice} />
      </div>
    </form>
  );
};
