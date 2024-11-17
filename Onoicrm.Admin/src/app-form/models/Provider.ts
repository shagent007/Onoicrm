export class Provider {
  public name!: string;

  public constructor(init?: Partial<Provider>) {
    Object.assign(this, init);
  }
}
