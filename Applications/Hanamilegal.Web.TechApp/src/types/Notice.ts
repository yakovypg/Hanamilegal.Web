export class Notice {
  public readonly message: string;
  public readonly isError: boolean;

  constructor(message: string, isError: boolean = false) {
    this.message = message;
    this.isError = isError;
  }
}
