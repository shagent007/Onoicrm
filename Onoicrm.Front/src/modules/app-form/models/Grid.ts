export class Grid {
    public lg?: string;
    public md?: string;
    public sm?: string;
    public xs?: string;

    public constructor(init?: Partial<Grid>) {
        Object.assign(this, init);
    }
}
