export class ValidationResult {
    public successMessages: string[] = [];
    public errorMessages: string[] = [];
    public get isValid() {
        return this.errorMessages.length === 0;
    }
    public constructor(init?: Partial<ValidationResult>) {
        Object.assign(this, init);
    }
}
