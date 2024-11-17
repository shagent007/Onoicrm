import {Entity} from '~/shared/entities/Entity';
export enum CompanyType {Clinic,Company}
export class Clinic extends Entity {
    public caption!: string;
    public groupUserProfile:any
    public wappiProfileId:any
    public wappiToken:any
    public workStartTime:any
    public workEndTime:any
    public bookingTimeDuration:any
    public useArmchairForBooking:any
    public bookingType:any
    public type:CompanyType = CompanyType.Clinic;
    constructor(instance?: Partial<Clinic>) {
        super();
        Object.assign(this, instance);
    }
}


