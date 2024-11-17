export class FieldAttribute {
  public caption?: string;
  public hint?: string;
  public cssClass: string = "";
  public iconPosition:string = "";
  public icon:string = "";

  public constructor(init?: Partial<FieldAttribute>) {
    Object.assign(this, init);
  }
}
