export class Watcher {
  public changeModel?: string;
  public changeAttrs?: string;
  public changeItems?: string;
  public fields: string[] = [];
  public callOnMounted: boolean = false;
  public constructor(init?: Partial<Watcher>) {
    Object.assign(this, init);
  }
}
