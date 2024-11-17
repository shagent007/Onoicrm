import { Validation } from '~/modules/app-form/models/Validation';
import moment from "moment";

export const regexList: any = {
    email: /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/,
    url: /^(?:(?:(?:https?|ftp):)?\/\/)(?:\S+(?::\S*)?@)?(?:(?!(?:10|127)(?:\.\d{1,3}){3})(?!(?:169\.254|192\.168)(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)(?:\.(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)*(?:\.(?:[a-z\u00a1-\uffff]{2,})))(?::\d{2,5})?(?:[/?#]\S*)?$/i,
    personName: /^\s+$/,
    minSec: /\d+:[0-5]\d/,
    requireDigit: /\d/,
    requireLowerCaseLatin: /[a-z]/,
    requireUpperCaseLatin: /[A-Z]/,
    requireSymbol: /[`!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/,
};

export const required = (msg: any = 'Поле не может быть пустым'): Validation => new Validation({ name: 'required', value: true, errorMessage: msg });

export const checkUserNameAsync = (name:string = "email",msg:any= 'Данное имя пользователя уже занято придумайте другой'):Validation =>
  new Validation({
    name: 'userIsExist',
    value: name,
    errorMessage: msg,
    trigger:"change"
  });
export const emailAddres = (msg: any = 'Введите корректный email'): Validation =>
    new Validation({
        name: 'regular',
        value: 'email',
        errorMessage: msg,
    });

export const requireSymbol = (msg: any = 'Поле должно содержать хотябы 1 специальный символ'): Validation =>
    new Validation({
        name: 'regular',
        value: 'requireSymbol',
        errorMessage: msg,
    });

export const requireDigit = (msg: any = 'Поле должно содержать хотябы 1 цифру'): Validation =>
    new Validation({
        name: 'regular',
        value: 'requireDigit',
        errorMessage: msg,
    });

export const requireUpperCaseLatin = (msg: any = 'Поле должно содержать хотябы 1 букву латиницу верхнего регистра'): Validation =>
    new Validation({
        name: 'regular',
        value: 'requireUpperCaseLatin',
        errorMessage: msg,
    });

export const requireLowerCaseLatin = (msg: any = 'Поле должно содержать хотябы 1 букву латиницу нижнего регистра'): Validation =>
    new Validation({
        name: 'regular',
        value: 'requireLowerCaseLatin',
        errorMessage: msg,
    });

export const isCorrectecTime = (msg: any = 'Введите корректный минуты и секунды'): Validation =>
    new Validation({
        name: 'regular',
        value: 'minSec',
        errorMessage: msg,
    });

export const min = (min: any, msg?: any): Validation =>
    new Validation({
        name: 'min',
        value: min,
        errorMessage: msg ?? `Поле не может сожержать меньше чем ${min} символов`,
    });

export const like = (targetFieldName: string, msg: string): Validation =>
    new Validation({
        name: 'like',
        value: targetFieldName,
        errorMessage: msg,
    });

export const compareDates = (date1:string, date2:string)=>{
  return moment(date1).format("DD.MM.YYYY") == moment(date2).format("DD.MM.YYYY")
}

export const sortByPriority = (p: any, n: any) => n.priority - p.priority;

export const requiredArgument = (errorText: string = 'Поле не может быть пустым') => {
    throw new Error(errorText);
};

