export class LoginModel {
  public email!: string;
  public password!: string;
  public clinicId!:number;
  constructor(init?:Partial<LoginModel>) {
    Object.assign(this, init)
  }
}
