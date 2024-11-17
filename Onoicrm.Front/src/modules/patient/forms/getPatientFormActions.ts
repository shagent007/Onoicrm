import {UserProfile} from "~/modules/user-profile/UserProfile";
import moment from "moment/moment";
import {collectTreeBranchParents, findElementInTree, getValue, remove, treeToFlat} from "~/shared/lib/helpers";
import {UserRegisterModel} from "~/modules/user/models/UserResisterModel";
import {cloneDeep} from "lodash";
import {IListDataSource, ITreeDataSource} from "~/modules/data-sources";

export const getPatientFormActions = (informationSourceDataSource:IListDataSource<any>) => ({
  getGenders: () => [
    { caption: "Женский", value: 0 },
    { caption: "Мужской", value: 1 },
  ],
  getDate:(model:UserProfile, name:string)=>{
    const value = moment(getValue(model,name));
    if(!value.isValid()) return "";
    return value.format("DD.MM.YYYY")
  },
  setDate:(model:UserProfile, name:string, newValue:any)=>{
    const value = moment(newValue, "DD.MM.YYYY");
    if(value.isSame(moment(model.birthDate))) return ;
    if(!value.isValid()) return "";
    model.birthDate = value.utc().format('YYYY-MM-DDTHH:mm:ss.SSS[Z]');
  },
  setRegisterDate:(model:any, name:string, newValue:any)=>{
    const value = moment(newValue, "DD.MM.YYYY");
    if(!value.isValid()) return "";
    model.birthDate = value.utc().format('YYYY-MM-DDTHH:mm:ss.SSS[Z]');
  },
  getInformationSources:async ()=> {
    if(!(informationSourceDataSource.items.value.length > 0)) {
      await informationSourceDataSource.get();
    }
    return informationSourceDataSource.items.value;
  },
})
