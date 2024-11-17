export class Validation {
  public name!: string;
  public value: any;
  public errorMessage!: string;
  public successMessage: string = "";
  public trigger:string|null = null
  public constructor(init?: Partial<Validation>) {
    Object.assign(this, init);
  }
}
