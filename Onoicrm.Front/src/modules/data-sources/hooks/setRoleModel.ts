export class SetRoleModel {
  public roleName!: string;
  public userId!: string;

  constructor(init?: Partial<SetRoleModel>) {
    Object.assign(this, init)
  }
}
