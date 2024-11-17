import { IMessage } from './IMessage';
import { ConfirmationOptions } from 'primevue/confirmationoptions';
export type Confirm = (options: ConfirmationOptions) => Promise<boolean>;
export const defaultConfirm = (options: ConfirmationOptions): Promise<boolean> => {
  throw new Error('Плагин не передан');
};
