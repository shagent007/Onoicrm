import { Filter } from '../models/Filter';
import { TransformInfo } from '../models/TransformInfo';
import { requiredArgument } from './consts';
import { ArrayCallBack } from '../models/ArrayCallBack';
import { fork } from 'child_process';

export const getGUID = () => {
  const _p8 = (s?: boolean) => {
    var p = (Math.random().toString(16) + '000000000').substr(2, 8);
    return s ? '-' + p.substr(0, 4) + '-' + p.substr(4, 4) : p;
  };
  return (_p8() + _p8(true) + _p8(true) + _p8()).toUpperCase();
};

export const toQueryString = (object: any) => (object ? '?' + new URLSearchParams(object).toString() : '');

export const getValue = (object: any, fieldPath: string) => {
  const regex = new RegExp(/[\.]/gm);
  if (!regex.test(fieldPath)) {
    return object[fieldPath];
  }
  let schema = object;

  const pList = fieldPath.split('.');
  const len = pList.length;
  for (var i = 0; i < len - 1; i++) {
    var elem = pList[i];
    if (!schema[elem]) schema[elem] = {};
    schema = schema[elem];
  }
  return schema[pList[len - 1]];
};

export const setValue = (object: any, fieldPath: string, value: any) => {
  const regex = new RegExp(/[\.]/gm);
  if (!regex.test(fieldPath)) {
    object[fieldPath] = value;
  }
  let schema = object;

  const pList = fieldPath.split('.');
  const len = pList.length;
  for (var i = 0; i < len - 1; i++) {
    var elem = pList[i];
    if (!schema[elem]) schema[elem] = {};
    schema = schema[elem];
  }
  schema[pList[len - 1]] = value;
};

export const generatePassword = () => {
  const chars = '0123456789abcdefghijklmnopqrstuvwxyz!@#$%^&*()ABCDEFGHIJKLMNOPQRSTUVWXYZ';
  const passwordLength = 8;
  let password = '';
  for (let i = 0; i <= passwordLength; i++) {
    const randomNumber = Math.floor(Math.random() * chars.length);
    password += chars.substring(randomNumber, randomNumber + 1);
  }
  return password;
};

export const parseErrorResponse = <ResponseType>(error: any) => {
  const data = error?.response?.data as ResponseType | null;
  return data;
};

export const toPascalCase = (string: string) => `${string}`.charAt(0).toUpperCase() + string.slice(1);

export const toServerCase = (str: string) =>
  str.includes('.')
    ? str
      .split('.')
      .map((s: string) => toPascalCase(s))
      .join('.')
    : toPascalCase(str);

export const toFilterArray = (object: any) =>
  Object.entries(object)
    .filter(([, v]: any) => !!v.value)
    .map(([n, v]: any) => new Filter(n, v.value));

export const remove = (source: any[], cb: any) => {
  const exists = source.some(cb);
  if (exists) {
    const index = source.findIndex(cb);
    source.splice(index, 1);
  }
};



export const transformObject = (src:any, info:TransformInfo[] ) => {
  const res:Record<string, any> = {};
  for (const infoItem of info) {
    res[infoItem.to] = src[infoItem.from];
  }
  return res;
}

export const deepTransformTree = (src:any, info:TransformInfo[], childrenInfo:TransformInfo) => {
  const treeRes = [];
  for (const srcElement of src) {
    const itemRes = transformObject(srcElement, info);
    if(srcElement[childrenInfo.from].length){
      itemRes[childrenInfo.to] = deepTransformTree(srcElement[childrenInfo.from], info, childrenInfo);
    }
    treeRes.push(itemRes);
  }
  return treeRes;
}

export const deepFilterTree = (tree:any[], filterCallBack:Function) => {
  const resTree = tree.filter(b => filterCallBack(b));
  for (const treeElement of resTree) {
    if(treeElement.children?.length){
      treeElement.children = deepFilterTree(treeElement.children, filterCallBack);
    }
  }
  return resTree;
}

export const findElementInTree = (
  key: any = requiredArgument("значения для поиска не передано"),
  tree: any = requiredArgument("Дерево для поиска не передано"),
  keyName: string = "id",
): any => {
  let _defined: any = null;

  (function find(key: number, tree: any) {
    for (const branch of tree) {
      if (key === branch[keyName]) {
        _defined = branch;
      }

      if (branch?.children?.length > 0) {
        find(key, branch.children);
      }
    }
  })(key, tree);

  return _defined;
};

export const removeElementInTree = (
  key: any = requiredArgument("значения для поиска не передано"),
  tree: any = requiredArgument("Дерево для поиска не передано"),
  keyName: string = "id",
): any => {
  (function find(key: number, tree: any) {
    for (const [index, branch] of tree.entries()) {
      if (key === branch[keyName]) {
        tree.splice(index, 1);
      }
      if (branch?.children?.length > 0) {
        find(key, branch.children);
      }
    }
  })(key, tree);
};

export const updateElementInTree = (
  data: any = requiredArgument("Данные для обновления не передано"),
  tree: any = requiredArgument("Дерево для поиска не передано"),
  update: Function = requiredArgument("CallBack для update а не передано"),
  keyName: string = "id",
): any => {
  for (const branch of tree) {
    if (data[keyName] === branch[keyName]) return update(branch);
    if (!branch?.children) continue;
    updateElementInTree(data, branch.children, update);
  }
};

export const collectTreeBranchParents = (branch:any, tree:any)=>{
  const branchList:any = [branch]
  let currentBranch = branch;
  while (currentBranch.parentId !== null){
    const parentBranch = findElementInTree(currentBranch.parentId, tree);
    if(parentBranch!=null) branchList.push(parentBranch)
    currentBranch = parentBranch;
  }
  return branchList;
}

export const treeToFlat = (tree:any, childrenKeyName:string="children") => {
  let flatten = [Object.assign({}, tree)];
  delete flatten[0][childrenKeyName];

  if (tree[childrenKeyName] && tree[childrenKeyName].length > 0) {
    return flatten.concat(tree[childrenKeyName]
      .map((child:any)=>treeToFlat(child, childrenKeyName))
      .reduce((a:any, b:any)=>a.concat(b), [])
    );
  }

  return flatten;
};
