export class ErrorResponse {
    public message: string;
    public innerMessage: string;

    public constructor(init?: Partial<ErrorResponse>) {
        Object.assign(this, init);
    }
}
