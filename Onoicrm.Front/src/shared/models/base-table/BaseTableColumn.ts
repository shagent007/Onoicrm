import {getGUID} from "../../lib/getGUID";

export class BaseTableColumn{
    public readonly id:string = getGUID()
    public name!:string;
    public caption!:string;
    public className!:string;

    constructor(init?:Partial<BaseTableColumn>) {
        Object.assign(this, init)
    }
}

