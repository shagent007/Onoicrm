import {ClinicEntity} from '~/shared/entities/ClinicEntity';

export class UserProfile extends ClinicEntity {
    public lastName: string = "";
    public fullName: string = "";
    public userId!: string;
    public birthDate: string | null = null;
    public whatsAppNumber:string = "";
    public address:string = "";
    public description:string = "";
    public informationSourceId: number | null = null
    public jobPosition:string = "";
    public stateId:number = 2;
    constructor(instance?: Partial<UserProfile>) {
        super();
        Object.assign(this, instance);
    }
}



