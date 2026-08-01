import "styles/components/contact-us-element.css";

import { useId, useState } from "react";
import { useTranslation } from "react-i18next";
import { createValidityHandler, type ValidityMessages } from "utils";

interface Props {
  readonly className?: string;
  readonly name: string;
  readonly required?: boolean;
  readonly label: string | React.ReactNode;
  readonly onChange?: (checked: boolean) => void;
}

export const ContactUsCheckBox: React.FC<Props> = ({
  className,
  name,
  required,
  label,
  onChange
}: Props) => {
  const id: string = useId();
  const { t } = useTranslation();
  const [checked, setChecked] = useState<boolean>(false);

  const validityMessages: ValidityMessages = {
    valueMissing: t("validation.valueMissing"),
    typeMismatch: t("validation.typeMismatch"),
    patternMismatch: t("validation.patternMismatch"),
    badInput: t("validation.badInput")
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const next: boolean = e.target.checked;
    setChecked(next);
    onChange?.(next);
  };

  return (
    <div className={`contact-us-element contact-us-checkbox ${className}`}>
      <div className="d-flex align-items-center gap-2 w-100">
        <input
          type="checkbox"
          title=""
          id={id}
          name={name}
          required={required}
          checked={checked}
          onChange={handleChange}
          onInvalid={createValidityHandler(validityMessages, required)}
          onInput={createValidityHandler(validityMessages, required)}
        />
        <label htmlFor={id}>{label}</label>
      </div>
    </div>
  );
};
