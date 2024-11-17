import { FieldAttribute } from "./FieldAttribute";
import { Grid } from "./Grid";
import { Provider } from "./Provider";
import { Validation } from "./Validation";
import { Watcher } from "./Watcher";
import { FieldConfig } from "./FieldConfig";
import {getGUID} from "~/shared/lib/getGUID";

export abstract class Field {
  public id: string = getGUID();
  public name!: string;
  public abstract editor: string;
  public abstract attrs: FieldAttribute;
  public abstract config: FieldConfig;
  public canShow!: string;
  public get?: string;
  public set?: string;
  public grid: string = "col-12";
  public validations: Validation[] = [];
  public watchers: Watcher[] = [];
  public provider: Provider = new Provider({ name: "default" });
  public slot: string;
  public onMounted: string[] = [];
  public isEmpty: boolean = false;
  public initialValue:any = null;


  protected constructor(name: string, init?: Partial<Field>) {
    this.name = name;
    Object.assign(this, init);
    this.slot = init?.slot ?? name;
  }
}
