import { IncorrectDataFormatError } from "errors";

export function formatPhone(phoneNumber: string): string {
  const countryCodePrefix: string = "+";

  const hasCountryCodePrefix: boolean =
    phoneNumber.length > 0 && phoneNumber[0] == countryCodePrefix;

  const digits: string = phoneNumber.replace(/\D/g, "");

  if (digits.length !== 11) {
    throw new IncorrectDataFormatError("Recieved phone number has incorrect format");
  }

  const firstDigit: string = digits[0];
  const firstDigitPrefix: string = hasCountryCodePrefix ? countryCodePrefix : "";

  const nums: string = digits.slice(1);
  const part1: string = nums.slice(0, 3);
  const part2: string = nums.slice(3, 6);
  const part3: string = nums.slice(6, 8);
  const part4: string = nums.slice(8, 10);

  return `${firstDigitPrefix}${firstDigit} (${part1}) ${part2}-${part3}-${part4}`;
}
