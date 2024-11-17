import { Grid } from '../models/Grid';

export class GridService {
    private grid: Grid;
    public getGrid() {
        return Object.entries(this.grid)
            .filter(([k, v]) => !!v)
            .map(([k, v]) => v)
            .join(' ');
    }

    constructor(grid: Grid) {
        this.grid = grid;
    }
}
