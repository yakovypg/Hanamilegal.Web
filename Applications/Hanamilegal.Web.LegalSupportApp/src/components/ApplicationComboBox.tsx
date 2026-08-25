import "styles/components/application-element.css";

import { useTranslation } from "react-i18next";
import { createValidityHandler, type ValidityMessages } from "utils";

export interface ComboBoxOption {
  value: string;
  label: string;
}

interface Props {
  readonly className?: string;
  readonly name: string;
  readonly required?: boolean;
  readonly placeholder?: string;
  readonly options: ComboBoxOption[];
}

export const ApplicationComboBox: React.FC<Props> = ({
  className,
  name,
  required,
  placeholder,
  options
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
      <select
        title=""
        defaultValue=""
        name={name}
        required={required}
        onInvalid={createValidityHandler(validityMessages, required)}
        onInput={createValidityHandler(validityMessages, required)}>
        <option value="" disabled hidden>
          {placeholder}
        </option>

        {options.map((option: ComboBoxOption) => (
          <option key={option.value} value={option.value}>
            {option.label}
          </option>
        ))}
      </select>
    </div>
  );
};
