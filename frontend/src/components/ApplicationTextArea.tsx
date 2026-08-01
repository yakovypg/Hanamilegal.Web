import "styles/components/application-element.css";

import { useTranslation } from "react-i18next";
import { createValidityHandler, type ValidityMessages } from "utils";

interface Props {
  readonly className?: string;
  readonly name: string;
  readonly required?: boolean;
  readonly placeholder?: string;
}

export const ApplicationTextArea: React.FC<Props> = ({
  className,
  name,
  required,
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
    <div className={`application-element ${className}`}>
      <textarea
        title=""
        name={name}
        required={required}
        onInvalid={createValidityHandler(validityMessages, required)}
        onInput={createValidityHandler(validityMessages, required)}
        placeholder={placeholder}
      />
    </div>
  );
};
