export type TConfirm = (options: any) => Promise<boolean>;
export const defaultConfirm = (options: any): Promise<boolean> => {
  throw new Error('Плагин не передан');
};
