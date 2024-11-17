export interface IMessage {
    success(message: string, text?: string): void;
    info(message: string, text?: string): void;
    error(message: string, text?: string): void;
}

export const defaultMessage: IMessage = {
    success: function (message: string, text?: string | undefined): void {
        throw new Error('Function not implemented.');
    },
    info: function (message: string, text?: string | undefined): void {
        throw new Error('Function not implemented.');
    },
    error: function (message: string, text?: string | undefined): void {
        throw new Error('Function not implemented.');
    },
};
