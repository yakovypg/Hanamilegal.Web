export class IncorrectDataFormatError extends Error {
  constructor(message?: string) {
    super(message || "Incorrect data format");
    this.name = "IncorrectDataFormatError";

    Object.setPrototypeOf(this, IncorrectDataFormatError.prototype);
  }
}
