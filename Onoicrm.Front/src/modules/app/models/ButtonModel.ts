import {ButtonProps} from "primevue/button";
import {getGUID} from "~/shared/lib/getGUID";

export class ButtonModel{
  public id = getGUID()
  public attrs!:ButtonProps;
  public action!:Function
  constructor(init?: Partial<ButtonModel>) {
    Object.assign(this,init)
  }
}
