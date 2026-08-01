import type React from "react";

export interface ValidityMessages {
  valueMissing?: string;
  typeMismatch?: string;
  patternMismatch?: string;
  badInput?: string;
}

export function createValidityHandler(messages: ValidityMessages, required?: boolean) {
  return (
    e: React.InvalidEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>
  ): void => {
    const target: HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement = e.currentTarget;

    target.setCustomValidity("");
    console.log(target.validity);

    if (required && target.validity.valueMissing) {
      if (messages.valueMissing) {
        target.setCustomValidity(messages.valueMissing);
      }

      return;
    }

    if (target.validity.typeMismatch) {
      if (messages.typeMismatch) {
        target.setCustomValidity(messages.typeMismatch);
      }

      return;
    }

    if (target.validity.patternMismatch) {
      if (messages.patternMismatch) {
        target.setCustomValidity(messages.patternMismatch);
      }

      return;
    }

    if (target.validity.badInput) {
      if (messages.badInput) {
        target.setCustomValidity(messages.badInput);
      }

      return;
    }

    target.setCustomValidity("");
  };
}
