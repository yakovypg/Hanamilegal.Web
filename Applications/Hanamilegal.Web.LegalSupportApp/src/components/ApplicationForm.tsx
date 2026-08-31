import "styles/components/application-form.css";

import { CREATE_APPLICATION_ENDPOINT } from "api";
import {
  ApplicationCheckBox,
  ApplicationComboBox,
  ApplicationField,
  ApplicationTextArea,
  type ComboBoxOption,
  NoticeMessage
} from "components";
import { PRIVACY_POLICY } from "config";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import { type Application, APPLICATION_TYPES, type ApplicationType, Notice } from "types";

interface Props {
  readonly className?: string;
}

interface ApplicationFieldNames {
  readonly type: string;
  readonly senderName: string;
  readonly organization: string;
  readonly email: string;
  readonly text: string;
  readonly hasPersonalDataProcessingConsent: string;
}

const applicationFieldNames: ApplicationFieldNames = {
  type: "applicationType",
  senderName: "senderName",
  email: "email",
  organization: "organization",
  text: "applicationDescription",
  hasPersonalDataProcessingConsent: "hasPersonalDataProcessingConsent"
};

export const ApplicationForm: React.FC<Props> = ({ className }: Props) => {
  const { t } = useTranslation();

  const [hasPersonalDataProcessingConsent, setHasPersonalDataProcessingConsent] =
    useState<boolean>(false);

  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
  const [notice, setNotice] = useState<Notice | null>(null);

  const applicationTypes: ApplicationType[] = Object.values(APPLICATION_TYPES);

  const applicationTypeOptions: ComboBoxOption[] = applicationTypes.map((x: ApplicationType) => {
    return {
      value: x,
      label: t(`application.type.${x}`)
    };
  });

  const handleSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (isSubmitting) {
      return;
    }

    setNotice(null);
    setIsSubmitting(true);

    const form: HTMLFormElement = e.currentTarget;
    const formData: FormData = new FormData(form);

    const application: Application = {
      type: String(formData.get(applicationFieldNames.type) ?? ""),
      senderName: String(formData.get(applicationFieldNames.senderName) ?? ""),
      email: String(formData.get(applicationFieldNames.email) ?? ""),
      organization: String(formData.get(applicationFieldNames.organization) ?? ""),
      text: String(formData.get(applicationFieldNames.text) ?? ""),
      hasPersonalDataProcessingConsent: formData.has(applicationFieldNames.hasPersonalDataProcessingConsent),
    };

    function isRequiredFieldSpecified(value: unknown): boolean {
      if (typeof value === "string") {
        return value.trim() !== "";
      }

      if (typeof value === "boolean") {
        return value === true;
      }

      return value != null;
    };

    const allRequiredFieldsSpecified: boolean = Object
      .values(application)
      .every(isRequiredFieldSpecified);

    if (!allRequiredFieldsSpecified) {
      setNotice(new Notice(t("error.requiredFieldsNotSpecified"), true));
      setIsSubmitting(false);
      return;
    }

    const request: RequestInit = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(application)
    };

    let response: Response | null = null;

    try {
      response = await fetch(CREATE_APPLICATION_ENDPOINT, request);
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
      {t("sentense.personalData.consent")}{" "}
      <a className="color-primary" href={PRIVACY_POLICY} target="_blank" rel="noreferrer">
        {t("sentense.personalData.privacyPolicy")}
      </a>
    </div>
  );

  return (
    <form className={className ?? ""} onSubmit={handleSubmit}>
      <div className="application-elements-container">
        <ApplicationField
          required
          name={applicationFieldNames.senderName}
          type="text"
          autoComplete="name"
          placeholder={t("placeholder.name")}
        />
        <ApplicationField
          required
          name={applicationFieldNames.email}
          type="email"
          autoComplete="email"
          placeholder={t("placeholder.email")}
        />
        <ApplicationField
          required
          name={applicationFieldNames.organization}
          type="text"
          autoComplete="organization"
          placeholder={t("placeholder.organization")}
        />
        <ApplicationComboBox
          required
          name={applicationFieldNames.type}
          options={applicationTypeOptions}
          placeholder={t("placeholder.applicationType")}
        />
        <ApplicationTextArea
          required
          className="span2"
          name={applicationFieldNames.text}
          placeholder={t("placeholder.applicationDescription")}
        />
        <ApplicationCheckBox
          required
          className="span2"
          name={applicationFieldNames.hasPersonalDataProcessingConsent}
          label={processPersonalDataLabel}
          onChange={(checked: boolean) => setHasPersonalDataProcessingConsent(checked)}
        />

        <button
          className="mt-2 send-request-button span2"
          type="submit"
          disabled={isSubmitting || !hasPersonalDataProcessingConsent}>
          {isSubmitting ? "..." : t("phrase.sendRequest")}
        </button>

        <NoticeMessage className="span2" notice={notice} />
      </div>
    </form>
  );
};
