export class Validation {
  public name: string;
  public value: any;
  public errorMessage: string;
  public successMessage: string = "";

  public constructor(init?: Partial<Validation>) {
    Object.assign(this, init);
  }
}
