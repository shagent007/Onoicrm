import { UserProfile } from '../entities/UserProfile';

export class UserRegisterModel {
    public email!: string;
    public password!: string;
    public confirmPassword!: string;
    public userProfile: UserProfile = new UserProfile();
    public roles:any[] = []
    public userProfileGroups:any=[]
    constructor(instance?: Partial<UserRegisterModel>) {
        Object.assign(this, instance);
    }
}
