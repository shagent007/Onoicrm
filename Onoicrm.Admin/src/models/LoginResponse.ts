export class LoginResponse {
    public errors: Array<string> = [];
    public expireDate: string;
    public isSuccess: boolean;
    public message: string;

    public constructor(init?: Partial<LoginResponse>) {
        Object.assign(this, init);
    }
}
