import {getGUID} from "~/shared/lib/getGUID";
import {Week} from "~/modules/booking/entities/Week";

export class Month{
    id:string = getGUID();
    weeks!:Week[]

    constructor(init?:Partial<Month>) {
        Object.assign(this,init)
    }
}
