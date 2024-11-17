import { IBookingObjectDataSource } from "../interfaces/IBookingObjectDataSource";
import axios from "axios";
import { useObjectDataSource } from "~/modules/data-sources";
import { ObjectDataSourceConfig } from "~/modules/data-sources";
import { ProgressCallBack } from "~/modules/data-sources/interfaces/ProgressCallBack";
import { checkDuplicateAuthError } from "~/modules/data-sources/checkDuplicateAuthError";

export const useBookingObjectDataSource = (
  config: ObjectDataSourceConfig = new ObjectDataSourceConfig({}),
): IBookingObjectDataSource => {
  const dataSource = useObjectDataSource(
    new ObjectDataSourceConfig({
      ...config,
      className: "booking",
    }),
  );

  const upload = async (formData: FormData, onLoad?: ProgressCallBack) => {
    try {
      const { data } = await axios.post(
        `/api/v1/public/booking/${config.id}/upload/`,
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
            Accept: "*/*",
          },
          onUploadProgress(e) {
            if (onLoad) onLoad(e);
          },
        },
      );

      return data;
    } catch (e) {
      await checkDuplicateAuthError(e);
    }
  };

  return {
    ...dataSource,
    upload,
  };
};
