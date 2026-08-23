export interface Application {
  readonly type: string;
  readonly senderName: string;
  readonly organization: string;
  readonly email: string;
  readonly text: string;
}
