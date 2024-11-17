import {ProgressCallBack} from "~/modules/data-sources/interfaces/ProgressCallBack";

export interface IUploadService {
  upload(formData: FormData, onLoad: ProgressCallBack): Promise<any>;
}
