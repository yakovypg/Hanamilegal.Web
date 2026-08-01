import "styles/components/contact-us-element.css";

import { useTranslation } from "react-i18next";
import { createValidityHandler, type ValidityMessages } from "utils";

interface Props {
  readonly className?: string;
  readonly name: string;
  readonly required?: boolean;
  readonly type: string;
  readonly autoComplete?: string;
  readonly placeholder?: string;
}

export const ContactUsField: React.FC<Props> = ({
  className,
  name,
  required,
  type,
  autoComplete,
  placeholder
}: Props) => {
  const { t } = useTranslation();

  const validityMessages: ValidityMessages = {
    valueMissing: t("validation.valueMissing"),
    typeMismatch: t("validation.typeMismatch"),
    patternMismatch: t("validation.patternMismatch"),
    badInput: t("validation.badInput")
  };

  return (
    <div className={`contact-us-element ${className}`}>
      <input
        title=""
        name={name}
        required={required}
        type={type}
        placeholder={placeholder}
        autoComplete={autoComplete}
        onInvalid={createValidityHandler(validityMessages, required)}
        onInput={createValidityHandler(validityMessages, required)}
      />
    </div>
  );
};
