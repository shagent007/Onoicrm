import { ClinicEntity } from './ClinicEntity';

export class UserProfile extends ClinicEntity {
    public firstName!: string;
    public lastName!: string;
    public patronymic!: string;
    public fullName!: string;
    public userId!: string;
    public birthDate!: string;
    public phoneNumber!:string;
    public whatsAppNumber!:string;
    public address!:string;
    public clinicId:any

    public description!:string;
    constructor(instance?: Partial<UserProfile>) {
        super();
        Object.assign(this, instance);
    }
}
